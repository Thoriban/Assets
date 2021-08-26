using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class PokemonCollection
{
    public string collectionName { get; set; }
    public string[] pokemonNames { get; set; }
    public List<string> pokemonNamesList { get; set; }
    public int[] dexNumbers { get; set; }
    public List<int> dexNumbersList { get; set; }

    public PokemonCollection()
    {
        pokemonNames = new string[1];
        pokemonNamesList = new List<string>();

        dexNumbers = new int[1];
        dexNumbersList = new List<int>();
    }

    public PokemonCollection(string collectionName, string[] pokemonNames, int[] dexNumbers)
    {
        this.collectionName = collectionName;
        this.pokemonNames = pokemonNames;
        this.dexNumbers = dexNumbers;
    }

    public void ConvertArraysIntoLists()
    {
        pokemonNamesList = pokemonNames.ToList();
        dexNumbersList = dexNumbers.ToList();
    }

    public void ConvertListsIntoArrays()
    {
        pokemonNames = pokemonNamesList.ToArray();
        dexNumbers = dexNumbersList.ToArray();
    }

    public void AddPokemonToCollection(string pokemon, int dexNumber)
    {
        ConvertArraysIntoLists();
        pokemonNamesList.Add(pokemon);
        dexNumbersList.Add(dexNumber);
        ConvertListsIntoArrays();
    }

    public void SortCollectionByDexNumber()
    {
        Dictionary<string, int> unsortedCollection = new Dictionary<string, int>();

        if (pokemonNames.Length != 0 && dexNumbers.Length != 0)
        {
            for (int i = 0; i < dexNumbers.Length; i++)
            {
                unsortedCollection.Add(pokemonNames[i], dexNumbers[i]);
            }

            var sortedCollection = unsortedCollection.ToList();

            sortedCollection.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            pokemonNamesList.Clear();
            dexNumbersList.Clear();

            foreach (KeyValuePair<string, int> pair in sortedCollection)
            {
                pokemonNamesList.Add(pair.Key);
                dexNumbersList.Add(pair.Value);
            }

            pokemonNames = pokemonNamesList.ToArray();
            dexNumbers = dexNumbersList.ToArray();
        }
    }

    public void SortCollectionByPokemon()
    {
        Dictionary<string, int> unsortedCollection = new Dictionary<string, int>();

        if (pokemonNames.Length != 0 && dexNumbers.Length != 0)
        {
            for (int i = 0; i < dexNumbers.Length; i++)
            {
                unsortedCollection.Add(pokemonNames[i], dexNumbers[i]);
            }

            var sortedCollection = unsortedCollection.ToList();

            sortedCollection.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            pokemonNamesList.Clear();
            dexNumbersList.Clear();

            foreach (KeyValuePair<string, int> pair in sortedCollection)
            {
                pokemonNamesList.Add(pair.Key);
                dexNumbersList.Add(pair.Value);
            }

            pokemonNames = pokemonNamesList.ToArray();
            dexNumbers = dexNumbersList.ToArray();
        }
    }

    public void Save()
    {
        DataControl.Save(collectionName, DataControl.DataType.PokemonCollection, this);
    }
}
