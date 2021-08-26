using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class AppController : MonoBehaviour
{
    public static AppController instance;
    public List<Game> games;
    public List<Pokemon> pokemon;
    public List<ShinyMethod> methods;

    string dataDirectory;

    public int generation;
    public int collectionType;
    public string collectionName;
    public string gameName;
    public string huntId;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
            dataDirectory = Application.dataPath + "/Data/";
            Log.StartUpEntry();
            UpdateData("All");
        }
    }

    public void UpdateData(string dataType)
    {
        if (dataType != "All")
        {
            switch (dataType)
            {
                case "Game":
                    games.Clear();
                    break;
                case "Pokemon":
                    pokemon.Clear();
                    break;
                case "Shiny Hunting Method":
                    methods.Clear();
                    break;
            }

            if (Directory.Exists(dataDirectory + dataType))
            {
                Log.Write("DataDirectory Exists");
                string[] files = Directory.GetFiles(dataDirectory + dataType, "*.dat", SearchOption.AllDirectories);

                if (files.Length == 0)
                {
                    Debug.Log("no " + dataType + " files exist");
                }
                else
                {
                    foreach (string file in files)
                    {
                        switch (dataType)
                        {
                            case "Game":
                                Game game = DataControl.LoadGame(file);
                                games.Add(game);
                                break;
                            case "Pokemon":
                                Pokemon pokemonFile = DataControl.LoadPokemon(file);
                                pokemon.Add(pokemonFile);
                                break;
                            case "Shiny Hunting Method":
                                ShinyMethod method = DataControl.LoadShinyHuntingMethod(file);
                                methods.Add(method);
                                break;
                        }
                    }
                }
            }
            else
            {
                Log.Write("DataDirectoryCreated");
                Directory.CreateDirectory(dataDirectory + dataType);
            }
        }
        else
        {
            UpdateData("Game");
            UpdateData("Pokemon");
            UpdateData("Shiny Hunting Method");
        }
    }

    public void UpdateDataEntry(string dataType, string entryName, object data)
    {
        switch (dataType)
        {
            case "Pokemon":
                for (int i = 0; i < pokemon.Count; i++)
                {
                    if (pokemon[i].name == entryName)
                    {
                        pokemon[i] = (Pokemon)data;
                    }
                }
                break;
            case "Game":
                for (int i = 0; i < games.Count; i++)
                {
                    if (games[i].name == entryName)
                    {
                        games[i] = (Game)data;
                    }
                }
                break;
            case "Shiny Hunting Method":
                for (int i = 0; i < methods.Count; i++)
                {
                    if (methods[i].methodName == entryName)
                    {
                        methods[i] = (ShinyMethod)data;
                    }
                }
                break;
        }
    }

}
