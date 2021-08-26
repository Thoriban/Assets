using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataControl : MonoBehaviour
{
    public enum DataType
    {
        HuntingMethod,
        Pokemon,
        Game,
        GameCollection,
        PokeDex,
        PokemonCollection,
        ShinyHunt
    }

    public static void Save(string name, DataType dataType, object data, string destination = "", string gameName = "")
    {
        if (destination == "")
        {
            destination = Application.dataPath + "/Data/";
        }

        switch (dataType)
        {
            case DataType.Game: destination += "Game/"; break;
            case DataType.GameCollection: destination += "Game Collection/"; break;
            case DataType.PokeDex: destination += "Poke Dex/"; break;
            case DataType.PokemonCollection: destination += "Collections/"; break;
            case DataType.Pokemon: destination += "Pokemon/"; break;
            case DataType.ShinyHunt: destination += "Shiny Hunts/"; break;
            case DataType.HuntingMethod:
                if (gameName != "")
                {
                    destination += "Shiny Hunting Method/Pokemon " + gameName + "/";
                }
                else
                {
                    Log.Write("ERROR: SHINY HUNTING METHOD SAVE: No Game Specified for method: " + name + ".");
                    Debug.LogError("ERROR: SHINY HUNTING METHOD SAVE: No Game Specified for method: " + name + ".");
                }

                break;
        }

        if (!Directory.Exists(destination))
        {
            Directory.CreateDirectory(destination);
        }

        if (name != "")
        {
            destination += name + ".dat";

            FileStream fileStream;

            if (File.Exists(destination))
            {
                fileStream = File.OpenWrite(destination);
            }
            else
            {
                fileStream = File.Create(destination);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }
    }

    public static ShinyMethod LoadShinyHuntingMethod(string filePath)
    {
        ShinyMethod method = new ShinyMethod();

        FileStream fileStream;

        if (File.Exists(filePath))
        {
            fileStream = File.OpenRead(filePath);

            BinaryFormatter bf = new BinaryFormatter();
            ShinyMethod data = (ShinyMethod)bf.Deserialize(fileStream);
            fileStream.Close();

            method = new ShinyMethod(data.methodName, data.game, data.methodType, data.effectedByShinyCharm,
                data.baseOddsKeys, data.baseOddsValues, data.shinyCharmOddsKeys, data.shinyCharmOddsValues,
                data.automaticallyApplied, data.chanceOfBeingApplied, data.availablePokemon, data.description);
        }

        return method;
    }

    public static Pokemon LoadPokemon(string filePath)
    {
        Pokemon pokemon = new Pokemon();

        FileStream fileStream;

        if (File.Exists(filePath))
        {
            fileStream = File.OpenRead(filePath);

            BinaryFormatter bf = new BinaryFormatter();
            Pokemon data = (Pokemon)bf.Deserialize(fileStream);
            fileStream.Close();

            pokemon = new Pokemon(data.name, data.dexNumber, data.generation, data.evolutionStage,
                data.eggGroup1, data.eggGroup2, data.type1, data.type2, data.evolutionTree, data.availableGames,
                data.availableInGo, data.availableShinyInGo, data.availableMethods, data.availableLocations,
                data.bestOdds, data.bestMethods, data.bestGames, data.alolanForm, data.galarianForm,
                data.alternateForms, data.hasMegaEvolution, data.megaEvolutionForms, data.hasGigantamaxForm);
        }

        return pokemon;
    }

    public static Game LoadGame(string filePath)
    {
        Game game = new Game();

        FileStream fileStream;
        //TextAsset txt = (TextAsset)Resources.Load(filePath, typeof(TextAsset));
        //string content = txt.text;

        //Log.Write(content);

        if (File.Exists(filePath))
        {
            fileStream = File.OpenRead(filePath);

            BinaryFormatter bf = new BinaryFormatter();
            Game data = (Game)bf.Deserialize(fileStream);
            fileStream.Close();

            game = new Game(data.name, data.regionName, data.generation, data.shinyCharmAvailable,
                data.baseShinyOdds, data.playableConsoles, data.locations, data.availablePokemon,
                data.availableMethods, data.availableGoShinies, data.availableGoEventOnlyShinies);
        }

        return game;
    }

    public static GameCollection LoadGameCollection(string filePath)
    {
        GameCollection gameCollection = new GameCollection();

        FileStream fileStream;

        if (File.Exists(filePath))
        {
            fileStream = File.OpenRead(filePath);

            BinaryFormatter bf = new BinaryFormatter();
            GameCollection data = (GameCollection)bf.Deserialize(fileStream);
            fileStream.Close();

            gameCollection = new GameCollection(data.name, data.gamesCollected);
        }

        return gameCollection;
    }

    public static PokeDex LoadPokeDex(string filePath)
    {
        PokeDex dex = new PokeDex();

        FileStream fileStream;

        if (File.Exists(filePath))
        {
            fileStream = File.OpenRead(filePath);

            BinaryFormatter bf = new BinaryFormatter();
            PokeDex data = (PokeDex)bf.Deserialize(fileStream);
            fileStream.Close();

            dex = new PokeDex(data.dexName, data.dexNumbers, data.pokemonNames);
        }

        return dex;
    }

    public static PokemonCollection LoadCollection(string filePath)
    {
        PokemonCollection collection = new PokemonCollection();

        FileStream fileStream;

        if (File.Exists(filePath))
        {
            fileStream = File.OpenRead(filePath);

            BinaryFormatter bf = new BinaryFormatter();
            PokemonCollection data = (PokemonCollection)bf.Deserialize(fileStream);
            fileStream.Close();

            collection = new PokemonCollection(data.collectionName, data.pokemonNames, data.dexNumbers);
        }

        return collection;
    }

    public static ShinyHunt LoadShinyHunt(string filePath)
    {
        ShinyHunt shinyHunt = new ShinyHunt(Application.dataPath);

        FileStream fileStream;

        if (File.Exists(filePath))
        {
            fileStream = File.OpenRead(filePath);

            BinaryFormatter bf = new BinaryFormatter();
            ShinyHunt data = (ShinyHunt)bf.Deserialize(fileStream);
            fileStream.Close();

            shinyHunt = new ShinyHunt(data.secretId, data.pokemon, data.game,data.method, data.counterTitles, data.counterValues, data.averageEncounterTime, data.counterTimers);
        }

        return shinyHunt;
    }
}
