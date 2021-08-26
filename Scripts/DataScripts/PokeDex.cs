using System;
using System.Collections.Generic;

[Serializable]
public class PokeDex
{
    public string dexName { get; set; }
    public int[] dexNumbers { get; set; }
    public string[] pokemonNames { get; set; }

    public PokeDex()
    {
        dexNumbers = new int[1];
        pokemonNames = new string[1];
    }

    public PokeDex(string dexName, int[] dexNumbers, string[] pokemonNames)
    {
        this.dexName = dexName;
        this.dexNumbers = dexNumbers;
        this.pokemonNames = pokemonNames;
    }

    public void Save()
    {
        DataControl.Save(dexName, DataControl.DataType.PokeDex, this);
    }
}
