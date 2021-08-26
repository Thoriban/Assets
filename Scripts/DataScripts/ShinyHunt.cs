using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ShinyHunt
{
    public int secretId { get; set; }
    public string pokemon { get; set; }
    public string game { get; set; }
    public string method { get; set; }
    public string[] counterTitles { get; set; }
    public int[] counterValues { get; set; }
    public float averageEncounterTime { get; set; }
    public float[] counterTimers { get; set; }

    System.Random random = new System.Random();

    public ShinyHunt(string dataPath)
    {
        bool idCreated = false;

        while (!idCreated)
        {
            bool found = false;

            string secretIdText = "";

            for (int i = 0; i < 8; i++)
            {
                secretIdText += random.Next(0, 10);
            }

            secretId = int.Parse(secretIdText);
            
            if (Directory.Exists(dataPath + "/Resources/Shiny Hunts/"))
            {
                string[] files = Directory.GetFiles(dataPath + "/Resources/Shiny Hunts/", "*.dat", SearchOption.AllDirectories);

                if (files.Length != 0)
                {
                    foreach (string file in files)
                    {
                        if (!file.EndsWith(".meta"))
                        {
                            if (file.EndsWith("- " + secretId + ".dat"))
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }
            
            idCreated = !found;
        }

        counterTitles = new string[1];
        counterValues = new int[1];
        counterTimers = new float[1];
    }

    public ShinyHunt(int secretId, string pokemon, string game, string method, string[] counterTitles, int[] counterValues, float averageEncounterTime, float[] counterTimers)
    {
        this.secretId = secretId;
        this.pokemon = pokemon;
        this.game = game;
        this.method = method;
        this.counterTitles = counterTitles;
        this.counterValues = counterValues;
        this.averageEncounterTime = averageEncounterTime;
        this.counterTimers = counterTimers;
    }

    public void Save()
    {
        DataControl.Save(pokemon + " - " + secretId, DataControl.DataType.ShinyHunt, this);
    }
}
