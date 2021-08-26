using System;
using System.Collections.Generic;

[Serializable]
public class Game
{
    public string name { get; set; }
    public string regionName { get; set; }
    public int generation { get; set; }
    public bool shinyCharmAvailable { get; set; }
    public float baseShinyOdds { get; set; }
    public string[] playableConsoles { get; set; }
    public string[] locations { get; set; }
    public string[] availablePokemon { get; set; }
    public string[] availableMethods { get; set; }
    public string[] availableGoShinies { get; set; }
    public string[] availableGoEventOnlyShinies { get; set; }

    public Game()
    {
        playableConsoles = new string[1];
        locations = new string[1];
        availablePokemon = new string[1];
        availableMethods = new string[1];
        availableGoShinies = new string[1];
        availableGoEventOnlyShinies = new string[1];
    }

    public Game(string name, string regionName, int generation,  bool shinyCharmAvailable, float baseShinyOdds, 
        string[] playableConsoles, string[] locations, string[] availablePokemon, string[] availableMethods, 
        string[] availableGoShinies, string[] availableGoEventOnlyShinies)
    {
        this.name = name;
        this.regionName = regionName;
        this.generation = generation;
        this.shinyCharmAvailable = shinyCharmAvailable;
        this.baseShinyOdds = baseShinyOdds;
        this.playableConsoles = playableConsoles;
        this.locations = locations;
        this.availablePokemon = availablePokemon;
        this.availableMethods = availableMethods;
        this.availableGoShinies = availableGoShinies;
        this.availableGoEventOnlyShinies = availableGoEventOnlyShinies;
    }

    public void Save()
    {
        DataControl.Save(name, DataControl.DataType.Game, this);
    }

    public void Save(string filePath)
    {
        if (name.Contains(":"))
        {
            name = name.Replace(":", string.Empty);
        }

        //LogSave();

        DataControl.Save(name, DataControl.DataType.Game, this, filePath);
    }
}
