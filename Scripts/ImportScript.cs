using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

public class ImportScript : MonoBehaviour
{
    public AppController appController;

    string appDataPath;
    string appStreamDataPath;

    public string[] columns;
    public string[] rows;

    public List<Pokemon> pokemonFiles;
    public Pokemon loadedPokemon;

    public List<ShinyMethod> shinyHuntingMethodFiles;
    public ShinyMethod shinyHuntingMethod;
    public ShinyMethod loadedShinyHuntingMethod;
    public string methodFilePath;

    public string savedMethodName;
    public string savedGame;
    public string savedMethodType;
    public bool savedEffectedByShinyCharm;
    public List<int> savedBaseOddsKeys;
    public List<float> savedBaseOddsValues;
    public List<int> savedShinyCharmOddsKeys;
    public List<float> savedShinyCharmOddsValues;
    public bool savedAutomaticallyApplied;
    public float savedChanceOfBeingApplied;
    public List<Pokemon> savedAvailablePokemon;
    public string savedDescription;

    public string loadedMethodName;
    public string loadedGame;
    public string loadedMethodType;
    public bool loadedEffectedByShinyCharm;
    public List<int> loadedBaseOddsKeys;
    public List<float> loadedBaseOddsValues;
    public List<int> loadedShinyCharmOddsKeys;
    public List<float> loadedShinyCharmOddsValues;
    public bool loadedAutomaticallyApplied;
    public float loadedChanceOfBeingApplied;
    public List<Pokemon> loadedAvailablePokemon;
    public string loadedDescription;

    List<int> dexNumbers = new List<int>();
    List<string> pokemonNames = new List<string>();
    public int dexType;
    public int generation;

    Text progressTextBox;
    int numProcessedFile;
    int numFilesToProcess;
    int numThreadsActive;

    List<string> gameTitles;

    Queue<LoadPokemonDataTask> loadPokemonDataQueue;
    List<LoadPokemonDataTask> activePokemonLoads;

    Queue<LoadPokemonGameDataTask> loadPokemonGameDataQueue;
    List<LoadPokemonGameDataTask> activePokemonGameDataLoads;

    Queue<LoadGameDataTask> loadGameDataQueue;
    List<LoadGameDataTask> activeGameDataLoads;

    Queue<LoadMethodDataTask> loadMethodDataQueue;
    List<LoadMethodDataTask> activeMethodDataLoads;

    void Start()
    {
        appController = GameObject.Find("AppController").GetComponent<AppController>();
        appDataPath = Application.persistentDataPath;
        appStreamDataPath = Application.streamingAssetsPath;
        pokemonFiles = new List<Pokemon>();
        gameTitles = new List<string>();

        progressTextBox = GameObject.Find("Progress Text").GetComponent<Text>();

        numProcessedFile = 0;

        loadPokemonDataQueue = new Queue<LoadPokemonDataTask>();
        activePokemonLoads = new List<LoadPokemonDataTask>();

        loadPokemonGameDataQueue = new Queue<LoadPokemonGameDataTask>();
        activePokemonGameDataLoads = new List<LoadPokemonGameDataTask>();

        loadGameDataQueue = new Queue<LoadGameDataTask>();
        activeGameDataLoads = new List<LoadGameDataTask>();

        loadMethodDataQueue = new Queue<LoadMethodDataTask>();
        activeMethodDataLoads = new List<LoadMethodDataTask>();
    }

    private void Update()
    {
        ProcessTaskUpdates();

        if (numFilesToProcess != 0)
        {
            float fraction = numProcessedFile / (1f * numFilesToProcess);
            int percentComplete = (int)(fraction * 100);

            progressTextBox.text = "Importing Pokemon Progress: " + numProcessedFile + " out of " + numFilesToProcess + " (" + percentComplete + "%)";
        }
    }

    void ProcessTaskUpdates()
    {
        if (loadPokemonDataQueue.Count != 0)
        {
            if (numThreadsActive < 5)
            {
                LoadPokemonDataTask task = loadPokemonDataQueue.Dequeue();
                task.Start();
                activePokemonLoads.Add(task);
                numThreadsActive++;
            }
        }
        else if (loadPokemonGameDataQueue.Count != 0)
        {
            if (numThreadsActive < 5)
            {
                LoadPokemonGameDataTask task = loadPokemonGameDataQueue.Dequeue();
                task.Start();
                activePokemonGameDataLoads.Add(task);
                numThreadsActive++;
            }
        }
        else if (loadGameDataQueue.Count != 0)
        {
            if (numThreadsActive < 5)
            {
                LoadGameDataTask task = loadGameDataQueue.Dequeue();
                task.Start();
                activeGameDataLoads.Add(task);
                numThreadsActive++;
            }
        }
        else if (loadMethodDataQueue.Count != 0)
        {
            if (numThreadsActive < 5)
            {
                LoadMethodDataTask task = loadMethodDataQueue.Dequeue();
                task.Start();
                activeMethodDataLoads.Add(task);
                numThreadsActive++;
            }
        }


        if (activePokemonLoads.Count != 0)
        {
            for (int i = 0; i < activePokemonLoads.Count; i++)
            {
                if (activePokemonLoads[i].Update())
                {
                    activePokemonLoads.RemoveAt(i);
                    numProcessedFile++;
                    numThreadsActive--;
                    break;
                }
            }
        }
        else if (activePokemonGameDataLoads.Count != 0)
        {
            for (int i = 0; i < activePokemonGameDataLoads.Count; i++)
            {
                if (activePokemonGameDataLoads[i].Update())
                {
                    activePokemonGameDataLoads.RemoveAt(i);
                    numProcessedFile++;
                    numThreadsActive--;
                    break;
                }
            }
        }
        else if (activeGameDataLoads.Count != 0)
        {
            for (int i = 0; i < activeGameDataLoads.Count; i++)
            {
                if (activeGameDataLoads[i].Update())
                {
                    activeGameDataLoads.RemoveAt(i);
                    numProcessedFile++;
                    numThreadsActive--;
                    break;
                }
            }
        }
        else if (activeMethodDataLoads.Count != 0)
        {
            for (int i = 0; i < activeMethodDataLoads.Count; i++)
            {
                if (activeMethodDataLoads[i].Update())
                {
                    activeMethodDataLoads.RemoveAt(i);
                    numProcessedFile++;
                    numThreadsActive--;
                    break;
                }
            }
        }
    }

    #region Pokemon Imports
    public void ImportPokemonData()
    {
        LoadPokemonData(Application.dataPath + "/Data/csv Files/Pokemon Import.csv");
    }

    public void ImportPokemonGameData()
    {
        LoadPokemonGameData(Application.dataPath + "/Data/csv Files/Pokemon Game Import.csv");
    }

    void LoadPokemonData(string filePath)
    {
        List<string> gameTitles = new List<string>();
        try
        {
            numProcessedFile = 0;
            string textParser = File.ReadAllText(filePath);
            rows = textParser.Split("\n"[0]);

            for (int index = 0; index < rows.Length; index++)
            {
                if (index != 0)
                {
                    columns = (rows[index].Trim()).Split(","[0]);

                    LoadPokemonDataTask loadData = new LoadPokemonDataTask();
                    loadData.columns = columns;
                    loadData.appDataPath = Application.dataPath;
                    loadPokemonDataQueue.Enqueue(loadData);
                }
                else
                {
                    columns = (rows[index].Trim()).Split(","[0]);

                    for (int i = 0; i < columns.Length; i++)
                    {
                        if (i > 2 & i < 35)
                        {
                            gameTitles.Add(columns[i]);
                        }
                    }
                }
            }

            numFilesToProcess = loadPokemonDataQueue.Count;
        }
        catch (Exception ex)
        {
            Log.Write("ERROR: Load: " + filePath + ":" + ex.Message);
            Log.Write(ex.StackTrace);

            Debug.LogError("ERROR: Load: " + filePath + ":" + ex.Message);
            Debug.LogError(ex.StackTrace);
        }
    }

    void LoadPokemonGameData(string filePath)
    {
        string[] allPokemonFilePaths = Directory.GetFiles(Application.dataPath + "/Data/Pokemon/", "*.dat", SearchOption.AllDirectories);
        List<string> existingPokemonFilePaths = new List<string>();

        foreach (string pokemonFilePath in allPokemonFilePaths)
        {
            if (!pokemonFilePath.Contains(".meta"))
            {
                existingPokemonFilePaths.Add(pokemonFilePath);
            }
        }

        try
        {
            numProcessedFile = 0;
            string textParser = File.ReadAllText(filePath);
            rows = textParser.Split("\n"[0]);

            for (int index = 0; index < rows.Length; index++)
            {
                if (index != 0)
                {
                    columns = (rows[index].Trim()).Split(","[0]);

                    LoadPokemonGameDataTask loadData = new LoadPokemonGameDataTask();
                    loadData.columns = columns;
                    loadData.appDataPath = Application.dataPath;
                    loadData.gameTitles = gameTitles.ToArray();
                    loadData.pokemonFilePaths = existingPokemonFilePaths.ToArray();
                    loadPokemonGameDataQueue.Enqueue(loadData);
                }
                else
                {
                    columns = (rows[index].Trim()).Split(","[0]);

                    for (int i = 4; i < 39; i++)
                    {
                        gameTitles.Add(columns[i]);
                    }
                }
            }

            numFilesToProcess = loadPokemonGameDataQueue.Count;
        }
        catch (Exception ex)
        {
            Log.Write("ERROR: Load: " + filePath + ":" + ex.Message);
            Log.Write(ex.StackTrace);

            Debug.LogError("ERROR: Load: " + filePath + ":" + ex.Message);
            Debug.LogError(ex.StackTrace);
        }
    }
    #endregion

    #region Game Imports
    public void ImportGameData()
    {
        LoadGameData(Application.dataPath + "/Data/csv Files/Game Import.csv");
    }

    void LoadGameData(string filePath)
    {
        string[] allGameFilePaths = Directory.GetFiles(Application.dataPath + "/Data/Game/", "*.dat", SearchOption.AllDirectories);
        List<string> existingGameFilePaths = new List<string>();

        foreach (string gameFilePath in allGameFilePaths)
        {
            if (!gameFilePath.Contains(".meta"))
            {
                existingGameFilePaths.Add(gameFilePath);
            }
        }

        try
        {
            numProcessedFile = 0;
            string textParser = File.ReadAllText(filePath);
            rows = textParser.Split("\n"[0]);

            appController.UpdateData("Game");

            for (int index = 0; index < rows.Length; index++)
            {
                if (index != 0)
                {
                    columns = (rows[index].Trim()).Split(","[0]);

                    LoadGameDataTask loadData = new LoadGameDataTask();
                    loadData.columns = columns;
                    loadData.appDataPath = Application.dataPath;
                    loadData.gameFilePaths = existingGameFilePaths.ToArray();
                    loadGameDataQueue.Enqueue(loadData);
                    //InterpretLoadedGameDataInfo(columns);
                }
            }

            numFilesToProcess = loadGameDataQueue.Count;
        }
        catch (Exception ex)
        {
            Log.Write("ERROR: Load: " + filePath + ":" + ex.Message);
            Log.Write(ex.StackTrace);

            Debug.LogError("ERROR: Load: " + filePath + ":" + ex.Message);
            Debug.LogError(ex.StackTrace);
        }
    }

    void InterpretLoadedGameDataInfo(string[] columns)
    {
        try
        {
            if (columns[0] != string.Empty && columns[0] != null && columns[0].Length != 0)
            {
                Game game = new Game();

                foreach (Game gameFile in appController.games)
                {
                    if (gameFile.name == columns[0])
                    {
                        Log.Write(gameFile.name + " File Found");
                        Debug.Log(gameFile.name + " File Found");
                        game = DataControl.LoadGame(appStreamDataPath + "/Resources/Game/Pokemon " + columns[0] + ".dat");
                        break;
                    }
                }

                game.name = columns[0];

                Log.Write("game name is " + columns[0]);
                Debug.Log("game name is " + columns[0]);
                List<string> playableConsoles = new List<string>();

                playableConsoles.Add(columns[1]); //Primary 
                playableConsoles.Add(columns[2]); //Secondary

                game.playableConsoles = playableConsoles.ToArray();
                game.regionName = columns[3];
                game.generation = CheckCellForInt(columns[4]);
                game.shinyCharmAvailable = CheckCellForBoolean(columns[5]);

                if (game.generation >= 6)
                {
                    game.baseShinyOdds = 1f / 4096f;
                }
                else
                {
                    game.baseShinyOdds = 1f / 8192f;
                }

                int minRouteNumber = CheckCellForInt(columns[6]);
                int maxRouteNumber = CheckCellForInt(columns[7]);

                Log.Write("setting Locations");
                Debug.Log("setting Locations");
                List<string> locations = new List<string>();

                for (int i = minRouteNumber; i < maxRouteNumber + 1; i++)
                {
                    locations.Add("Route " + i);
                }

                for (int i = 8; i < columns.Length; i++)
                {
                    if (columns[i] != "")
                    {
                        locations.Add(columns[i]);
                    }
                }

                game.locations = locations.ToArray();

                Log.Write("setting Pokemon");
                Debug.Log("setting pokemon");
                //Get all pokemon files
                //Foreach loop looking at each pokemon file and determining if it appears if the game.
                string[] pokemonFiles = Directory.GetFiles(appStreamDataPath + "/Resources/Pokemon/", "*.dat", SearchOption.AllDirectories);

                List<string> files = new List<string>();
                List<string> availablePokemon = new List<string>();

                foreach (string file in pokemonFiles)
                {
                    if (!file.Contains(".meta"))
                    {
                        files.Add(file);
                    }
                }

                foreach (string file in files)
                {
                    Pokemon pokemon = DataControl.LoadPokemon(file);

                    foreach (string gameName in pokemon.availableGames)
                    {
                        if (gameName != null)
                        {
                            if (gameName == "Pokemon " + game.name)
                            {
                                Debug.Log(pokemon.name + " Added to Game");

                                availablePokemon.Add(pokemon.name);
                                break;
                            }
                            else
                            {
                                Debug.Log(gameName + " =/=" + game.name);
                            }
                        }
                    }
                }

                game.availablePokemon = availablePokemon.ToArray();

                Debug.Log("setting methods");
                string shinyHuntingPath = appStreamDataPath + "/Resources/Shiny Hunting Method/Pokemon " + columns[0];

                if (Directory.Exists(shinyHuntingPath))
                {

                    string[] methodFiles = Directory.GetFiles(shinyHuntingPath, "*.dat", SearchOption.AllDirectories);
                    List<string> availableMethods = new List<string>();
                    files = new List<string>();

                    foreach (string file in methodFiles)
                    {
                        if (!file.Contains(".meta"))
                        {
                            files.Add(file);
                        }
                    }

                    foreach (string file in files)
                    {
                        ShinyMethod method = DataControl.LoadShinyHuntingMethod(file);
                        availableMethods.Add(method.methodName);
                    }

                    game.availableMethods = availableMethods.ToArray();
                }

                Debug.Log("Saving Game");
                game.Save(appDataPath + "/Resources/");

                numProcessedFile++;
            }
        }
        catch (Exception ex)
        {
            Log.Write("ERROR: LOAD GAME DATA INFO: " + ex.Message);
            Log.Write(ex.StackTrace);
            Debug.LogError("ERROR: LOAD GAME DATA INFO: " + ex.Message);
            Debug.LogError(ex.StackTrace);
        }
    }
    #endregion

    #region Method Imports
    public void ImportMethodData()
    {
        LoadMethodData(Application.dataPath + "/Data/csv Files/Method Import.csv");
    }

    public void ImportSpecificMethodData()
    {
        LoadSpecificMethodData(Application.dataPath + "/Data/csv Files/Varying Odds Shiny Hunting Methods/Capture Chain Method Import.csv");
        LoadSpecificMethodData(Application.dataPath + "/Data/csv Files/Varying Odds Shiny Hunting Methods/Chain Fishing Method Import.csv");
        LoadSpecificMethodData(Application.dataPath + "/Data/csv Files/Varying Odds Shiny Hunting Methods/Featured Limited Research Method Import.csv");
        LoadSpecificMethodData(Application.dataPath + "/Data/csv Files/Varying Odds Shiny Hunting Methods/PokeRadar Method Import.csv");
        LoadSpecificMethodData(Application.dataPath + "/Data/csv Files/Varying Odds Shiny Hunting Methods/SOS Battle Method Import.csv");
        LoadSpecificMethodData(Application.dataPath + "/Resources/csv Files/Varying Odds Shiny Hunting Methods/Enhanced Full Odds Method Import.csv");
    }

    void LoadMethodData(string filePath)
    {
        string[] allGameFilePaths = Directory.GetFiles(Application.dataPath + "/Data/Shiny Hunting Method/", "*.dat", SearchOption.AllDirectories);
        List<string> existingMethodFilePaths = new List<string>();

        foreach (string gameFilePath in allGameFilePaths)
        {
            if (!gameFilePath.Contains(".meta"))
            {
                existingMethodFilePaths.Add(gameFilePath);
            }
        }

        try
        {
            numProcessedFile = 0;
            string textParser = File.ReadAllText(filePath);
            rows = textParser.Split("\n"[0]);

            appController.UpdateData("Shiny Hunting Method");

            for (int index = 0; index < rows.Length; index++)
            {
                if (index != 0)
                {
                    columns = (rows[index].Trim()).Split(","[0]);

                    LoadMethodDataTask loadData = new LoadMethodDataTask();
                    loadData.columns = columns;
                    loadData.appDataPath = Application.dataPath;
                    loadData.methodFilePaths = existingMethodFilePaths.ToArray();
                    loadMethodDataQueue.Enqueue(loadData);
                    //LoadShinyHuntingMethodDataInfo(columns);
                }
            }

            numFilesToProcess = loadMethodDataQueue.Count;
        }
        catch (Exception ex)
        {
            Log.Write("ERROR: Load: " + filePath + ":" + ex.Message);
            Log.Write(ex.StackTrace);

            Debug.LogError("ERROR: Load: " + filePath + ":" + ex.Message);
            Debug.LogError(ex.StackTrace);
        }
    }

    void LoadSpecificMethodData(string filePath)
    {
        string[] allGameFilePaths = Directory.GetFiles(Application.dataPath + "/Data/Shiny Hunting Method/", "*.dat", SearchOption.AllDirectories);
        List<string> existingMethodFilePaths = new List<string>();

        foreach (string gameFilePath in allGameFilePaths)
        {
            if (!gameFilePath.Contains(".meta"))
            {
                existingMethodFilePaths.Add(gameFilePath);
            }
        }

        try
        {
            numProcessedFile = 0;
            string textParser = File.ReadAllText(filePath);
            rows = textParser.Split("\n"[0]);

            appController.UpdateData("Shiny Hunting Method");

            for (int index = 0; index < rows.Length; index++)
            {
                if (index != 0)
                {
                    columns = (rows[index].Trim()).Split(","[0]);

                    ShinyMethod method = new ShinyMethod();

                    foreach (string path in existingMethodFilePaths)
                    {
                        if (path.Contains("/" + columns[0] + ".dat"))
                        {
                            method = DataControl.LoadShinyHuntingMethod(path);
                            break;
                        }
                    }

                    if (columns.Length >= 9)
                    {
                        method.methodName = columns[0];
                        method.game = columns[1];
                        method.methodType = columns[2];
                        method.effectedByShinyCharm = CheckCellForBoolean(columns[3]);
                        method.automaticallyApplied = CheckCellForBoolean(columns[4]);
                        method.chancesOfBeingApplied = new List<float>();
                        method.baseOdds = new Dictionary<int, float>();
                        method.shinyCharmOdds = new Dictionary<int, float>();

                        if (method.automaticallyApplied)
                        {
                            for (int i = 6; i < columns.Length; i += 3)
                            {
                                method.chancesOfBeingApplied.Add(CheckCellForFloat(columns[5]));
                                method.baseOdds.Add(CheckCellForInt(columns[i]), CheckCellForFloat(columns[i + 1]));
                                method.shinyCharmOdds.Add(CheckCellForInt(columns[i]), CheckCellForFloat(columns[i + 2]));
                            }
                        }
                        else
                        {
                            for (int i = 5; i < columns.Length; i += 4)
                            {
                                method.chancesOfBeingApplied.Add(CheckCellForFloat(columns[i]));
                                method.baseOdds.Add(int.Parse(columns[i + 1]), float.Parse(columns[i + 2]));
                                method.shinyCharmOdds.Add(int.Parse(columns[i + 1]), float.Parse(columns[i + 3]));
                            }
                        }

                        method.chanceOfBeingApplied = method.chancesOfBeingApplied.ToArray();

                        method.baseOddsKeys = method.baseOdds.Keys.ToArray();
                        method.baseOddsValues = method.baseOdds.Values.ToArray();

                        method.shinyCharmOddsKeys = method.shinyCharmOdds.Keys.ToArray();
                        method.shinyCharmOddsValues = method.shinyCharmOdds.Values.ToArray();

                        method.Save(Application.dataPath + "/Data/", columns[1]);
                    }
                }
            }

            //numFilesToProcess = loadMethodDataQueue.Count;
        }
        catch (Exception ex)
        {
            Log.Write("ERROR: Load: " + filePath + ":" + ex.Message);
            Log.Write(ex.StackTrace);

            Debug.LogError("ERROR: Load: " + filePath + ":" + ex.Message);
            Debug.LogError(ex.StackTrace);
        }
    }

    void LoadShinyHuntingMethodDataInfo(string[] columns)
    {
        try
        {
            if (columns[0] != string.Empty && columns[0] != null && columns[0].Length != 0)
            {
                ShinyMethod method = new ShinyMethod();

                foreach (ShinyMethod huntingMethod in appController.methods)
                {
                    if (huntingMethod.methodName == columns[0])
                    {
                        Debug.Log(huntingMethod.methodName + " File Found");
                        method = DataControl.LoadShinyHuntingMethod(Application.dataPath + "/Data/Shiny Hunting Method/Pokemon " + columns[1] + "/" + columns[0] + ".dat");
                        break;
                    }
                }

                method.methodName = columns[0];
                method.game = columns[1];
                method.methodType = columns[2];
                method.effectedByShinyCharm = CheckCellForBoolean(columns[3]);
                method.automaticallyApplied = CheckCellForBoolean(columns[4]);
                method.chancesOfBeingApplied.Add(CheckCellForFloat(columns[5]));

                method.chanceOfBeingApplied = method.chancesOfBeingApplied.ToArray();

                int encountersRequired = int.Parse(columns[6]);
                float baseOdds = CheckCellForFloat(columns[7]);
                float shinyCharmOdds = CheckCellForFloat(columns[10]);

                if (baseOdds != 0)
                {
                    if (method.baseOdds.ContainsKey(encountersRequired))
                    {
                        method.baseOdds[encountersRequired] = baseOdds;
                    }
                    else
                    {
                        method.baseOdds.Add(encountersRequired, baseOdds);
                    }

                    List<int> baseOddsKeys = new List<int>();
                    List<float> baseOddsValues = new List<float>();

                    foreach (KeyValuePair<int, float> odds in method.baseOdds)
                    {
                        baseOddsKeys.Add(odds.Key);
                        baseOddsValues.Add(odds.Value);
                    }

                    method.baseOddsKeys = baseOddsKeys.ToArray();
                    method.baseOddsValues = baseOddsValues.ToArray();
                }
                else
                {
                    method.baseOddsKeys = new int[1];
                    method.baseOddsValues = new float[1];
                }

                if (shinyCharmOdds != 0f)
                {
                    if (method.shinyCharmOdds.ContainsKey(encountersRequired))
                    {
                        method.shinyCharmOdds[encountersRequired] = shinyCharmOdds;
                    }
                    else
                    {
                        method.shinyCharmOdds.Add(encountersRequired, shinyCharmOdds);
                    }

                    List<int> shinyCharmOddsKeys = new List<int>();
                    List<float> shinyCharmOddsValues = new List<float>();

                    foreach (KeyValuePair<int, float> odds in method.shinyCharmOdds)
                    {
                        shinyCharmOddsKeys.Add(odds.Key);
                        shinyCharmOddsValues.Add(odds.Value);
                    }

                    method.shinyCharmOddsKeys = shinyCharmOddsKeys.ToArray();
                    method.shinyCharmOddsValues = shinyCharmOddsValues.ToArray();
                }
                else
                {
                    method.shinyCharmOddsKeys = new int[1];
                    method.shinyCharmOddsValues = new float[1];
                }

                method.description = columns[13];

                method.description = method.description.Replace("", ",");

                List<string> availablePokemon = new List<string>();

                for (int i = 14; i < columns.Length; i++)
                {
                    if (columns[i] != string.Empty)
                    {
                        availablePokemon.Add(columns[i]);
                    }
                }

                method.availablePokemon = availablePokemon.ToArray();

                shinyHuntingMethodFiles.Add(method);

                method.Save(appDataPath + "/Resources/", columns[1]);

                numProcessedFile++;
            }
        }
        catch (Exception ex)
        {
            Log.Write("ERROR: LOAD SHINY HUNTING DATA INFO: " + ex.Message);
            Log.Write(ex.StackTrace);
            Debug.LogError("ERROR: LOAD SHINY HUNTING DATA INFO: " + ex.Message);
            Debug.LogError(ex.StackTrace);
        }
    }
    #endregion

    #region PokeDex Imports

    public void ImportPokeDexData(string dexName)
    {
        LoadPokeDexData(Application.dataPath + "/Data/csv Files/Poke Dex Import.csv", dexName, dexType, generation);
    }

    void LoadPokeDexData(string filePath, string dexName, int dexType, int generation)
    {
        dexNumbers = new List<int>();
        pokemonNames = new List<string>();

        PokeDex dex = new PokeDex();

        dex.dexName = dexName;

        try
        {
            string textParser = File.ReadAllText(filePath);
            rows = textParser.Split("\n"[0]);

            for (int index = 0; index < rows.Length; index++)
            {
                if (index != 0)
                {
                    columns = (rows[index].Trim()).Split(","[0]);

                    if (columns.Length > 6)
                    {
                        switch (dexType)
                        {
                            case 0: //Standard Regional or National PokeDex
                                if (generation != 0)
                                {
                                    if (CheckCellForInt(columns[2]) == generation)
                                    {
                                        dexNumbers.Add(CheckCellForInt(columns[1]));
                                        pokemonNames.Add(columns[0]);
                                    }
                                }
                                else
                                {
                                    dexNumbers.Add(CheckCellForInt(columns[1]));
                                    pokemonNames.Add(columns[0]);
                                }
                                break;

                            case 1: //Form PokeDex
                                if (generation != 0)
                                {
                                    if (CheckCellForInt(columns[2]) == generation)
                                    {
                                        if (columns[6] != "")
                                        {
                                            for (int i = 6; i < columns.Length; i++)
                                            {
                                                if (columns[i] != "")
                                                {
                                                    if (columns[i].Contains("Basic"))
                                                    {
                                                        AddPokemonToDex(columns[3], columns[1], columns[0], "");
                                                    }
                                                    else if (columns[i].Contains("Alolan"))
                                                    {
                                                        AddPokemonToDex(columns[3], columns[1], columns[0], "Alolan");
                                                    }
                                                    else if (columns[i].Contains("Galarian"))
                                                    {
                                                        AddPokemonToDex(columns[3], columns[1], columns[0], "Galarian");
                                                    }
                                                    else
                                                    {
                                                        AddPokemonToDex(columns[3], columns[1], columns[0], columns[i]);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            AddPokemonToDex(columns[3], columns[1], columns[0], "");
                                        }
                                    }
                                }
                                else
                                {
                                    if (columns[6] != "")
                                    {
                                        for (int i = 6; i < columns.Length; i++)
                                        {

                                            if (columns[i] != "")
                                            {
                                                if (columns[i].Contains("Basic"))
                                                {
                                                    AddPokemonToDex(columns[3], columns[1], columns[0], "");
                                                }
                                                else if (columns[i].Contains("Alolan"))
                                                {
                                                    AddPokemonToDex(columns[3], columns[1], columns[0], "Alolan");
                                                }
                                                else if (columns[i].Contains("Galarian"))
                                                {
                                                    AddPokemonToDex(columns[3], columns[1], columns[0], "Galarian");
                                                }
                                                else
                                                {
                                                    AddPokemonToDex(columns[3], columns[1], columns[0], columns[i]);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        AddPokemonToDex(columns[3], columns[1], columns[0], "");
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Write("ERROR: Load: " + filePath + ":" + ex.Message);
            Log.Write(ex.StackTrace);

            Debug.LogError("ERROR: Load: " + filePath + ":" + ex.Message);
            Debug.LogError(ex.StackTrace);
        }

        dex.dexNumbers = dexNumbers.ToArray();
        dex.pokemonNames = pokemonNames.ToArray();

        dex.Save();
    }

    void AddPokemonToDex(string sexualDimorphismCheck, string dexNumberCell, string pokemonName, string prefix)
    {
        if (CheckCellForBoolean(sexualDimorphismCheck))
        {
            dexNumbers.Add(CheckCellForInt(dexNumberCell));
            dexNumbers.Add(CheckCellForInt(dexNumberCell));

            if (prefix != "")
            {
                pokemonNames.Add("Male - " + prefix + " " + pokemonName);
                pokemonNames.Add("Female - " + prefix + " " + pokemonName);
            }
            else
            {
                pokemonNames.Add("Male - " + pokemonName);
                pokemonNames.Add("Female - " + pokemonName);
            }
        }
        else
        {
            dexNumbers.Add(CheckCellForInt(dexNumberCell));

            if (prefix != "")
            {
                pokemonNames.Add(prefix + " " + pokemonName);
            }
            else
            {
                pokemonNames.Add(pokemonName);
            }
        }
    }
    #endregion

    public static bool CheckCellForBoolean(string input)
    {
        string[] availableIdentifiers = new string[]
        {
            "Starter Only",
            "Evolve",
            "Event Only",
            "Yes",
            "Choice",
            "One",
            "Two",
            "Three",
            "Four"
        };

        foreach (string identifier in availableIdentifiers)
        {
            if (input == identifier)
            {
                return true;
            }
        }

        return false;
    }

    public static float CheckCellForFloat(string input)
    {
        float parsedValue;

        if (float.TryParse(input, out parsedValue))
        {
            return parsedValue;
        }
        else
        {
            return 0f;
        }
    }

    public static int CheckCellForInt(string input)
    {
        int parsedValue;

        if (int.TryParse(input, out parsedValue))
        {
            return parsedValue;
        }
        else
        {
            return -1;
        }
    }
}
