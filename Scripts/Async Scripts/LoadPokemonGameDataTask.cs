using System.Collections.Generic;

public class LoadPokemonGameDataTask : ThreadedJob
{
    public string[] columns;
    public string appDataPath;
    public string[] pokemonFilePaths;
    public string[] gameTitles;

    protected override void ThreadFunction()
    {
        Pokemon pokemon = new Pokemon();

        if (columns[0] != string.Empty)
        {
            foreach (string path in pokemonFilePaths)
            {
                if (path.Contains("/" + columns[0] + ".dat"))
                {
                    pokemon = DataControl.LoadPokemon(path);
                    break;
                }
            }

            List<string> availableGames = new List<string>();

            for (int i = 4; i < 39; i++)
            {
                bool available = ImportScript.CheckCellForBoolean(columns[i]);

                if (available)
                {
                    availableGames.Add("Pokemon " + gameTitles[i - 4]);
                }
            }

            if (ImportScript.CheckCellForBoolean(columns[2]))
            {
                availableGames.Add("Pokemon Go");
                pokemon.availableShinyInGo = columns[3];
            }

            pokemon.availableGames = availableGames.ToArray();

            List<string> alternateForms = new List<string>();

            if (columns[39] == "Yes")
            {
                alternateForms.Add("Male");
                alternateForms.Add("Female");
            }

            if (columns[40] == "Yes")
            {
                pokemon.hasMegaEvolution = true;
            }

            if (columns[41] == "Yes")
            {
                pokemon.hasGigantamaxForm = true;
            }

            List<string> megaEvolutionForms = new List<string>();

            for (int i = 42; i < columns.Length; i++)
            {
                if (columns[i] != string.Empty)
                {
                    if (columns[i].StartsWith("Mega "))
                    {
                        megaEvolutionForms.Add(columns[i]);
                    }
                    else if (columns[i].StartsWith("Alolan "))
                    {
                        pokemon.alolanForm = columns[i];
                    }
                    else if (columns[i].StartsWith("Galarian "))
                    {
                        pokemon.galarianForm = columns[i];
                    }
                    else
                    {
                        alternateForms.Add(columns[i]);
                    }
                }
            }

            pokemon.megaEvolutionForms = megaEvolutionForms.ToArray();
            pokemon.alternateForms = alternateForms.ToArray();

            pokemon.Save(appDataPath + "/Data/");
        }
    }

    protected override void OnFinished()
    {
        base.OnFinished();
    }
}
