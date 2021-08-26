using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameCollection
{
    public string name;
    public List<string> gamesCollectedList { get; set; }
    public string[] gamesCollected { get; set; }

    public GameCollection()
    {
        gamesCollected = new string[1];
        gamesCollectedList = new List<string>();
    }

    public GameCollection(string name, string[] gamesCollected)
    {
        this.name = name;
        this.gamesCollected = gamesCollected;
    }

    public void Save()
    {
        DataControl.Save(name, DataControl.DataType.GameCollection, this);
    }
}
