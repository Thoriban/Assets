using System.Collections.Generic;
using System.IO;

public class LoadGameDataTask : ThreadedJob
{
    public string[] columns;
    public string appDataPath;
    public string[] gameFilePaths;

    protected override void ThreadFunction()
    {
        if (columns[0] != string.Empty && columns[0] != null && columns[0].Length != 0)
        {
            Game game = new Game();

            foreach (string path in gameFilePaths)
            {
                if (path.Contains("/" + columns[0] + ".dat"))
                {
                    game = DataControl.LoadGame(path);
                    break;
                }
            }

            game.name = columns[0];

            List<string> playableConsoles = new List<string>();

            playableConsoles.Add(columns[1]); //Primary 
            playableConsoles.Add(columns[2]); //Secondary

            game.playableConsoles = playableConsoles.ToArray();
            game.regionName = columns[3];
            game.generation = ImportScript.CheckCellForInt(columns[4]);
            game.shinyCharmAvailable = ImportScript.CheckCellForBoolean(columns[5]);

            if (game.generation >= 6)
            {
                game.baseShinyOdds = 1f / 4096f;
            }
            else
            {
                game.baseShinyOdds = 1f / 8192f;
            }

            int minRouteNumber = ImportScript.CheckCellForInt(columns[6]);
            int maxRouteNumber = ImportScript.CheckCellForInt(columns[7]);

            List<string> locations = new List<string>();

            if (minRouteNumber != maxRouteNumber)
            {
                for (int i = minRouteNumber; i < maxRouteNumber + 1; i++)
                {
                    locations.Add("Route " + i);
                } 
            }

            for (int i = 8; i < columns.Length; i++)
            {
                if (columns[i] != "")
                {
                    locations.Add(columns[i]);
                }
            }

            game.locations = locations.ToArray();

            string[] pokemonFiles = Directory.GetFiles(appDataPath + "/Data/Pokemon/", "*.dat", SearchOption.AllDirectories);

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
                            availablePokemon.Add(pokemon.name);
                            break;
                        }
                    }
                }
            }

            game.availablePokemon = availablePokemon.ToArray();

            string shinyHuntingPath = appDataPath + "/Data/Shiny Hunting Method/Pokemon " + columns[0];

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

            game.Save(appDataPath + "/Data/");
        }
    }

    protected override void OnFinished()
    {
        base.OnFinished();
    }
}
