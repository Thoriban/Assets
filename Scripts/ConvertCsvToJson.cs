using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConvertCsvToJson : MonoBehaviour
{
    AppController appController;

    public string[] columns;
    public string[] rows;

    private void Start()
    {
        appController = GameObject.Find("AppController").GetComponent<AppController>();
        Debug.Log("count = "+appController.pokemon.Count);
        if (appController.pokemon.Count != 0)
        {
            SaveData();
        }
    }

    public void ReadCsv(string filePath)
    {
        try
        {
            string textParser = File.ReadAllText(filePath);
            rows = textParser.Split("\n"[0]);

            for (int index = 0; index < rows.Length; index++)
            {

            }
        }
        catch (Exception ex)
        {
            Log.Write("ERROR: Load: " + filePath + ":" + ex.Message);
            Log.Write(ex.StackTrace);

            Debug.LogError("ERROR: Load: " + filePath + ":" + ex.Message);
            Debug.LogError(ex.StackTrace);
        }
    }

    public void Read()
    {
        string path = "Assets/Resources/patients-data.json";

        StreamReader reader = new StreamReader(path);
        string data_text = reader.ReadToEnd();
        //Debug.Log(data_text);
        Game data = JsonUtility.FromJson<Game>(data_text);
        //patients_data = data;
        reader.Close();
    }

    void SaveData()
    {
        Debug.Log("here");
        object jsonData = Resources.Load(Application.dataPath + "/Resources/json Files/Pokemon Game Import.json");
        string jsonString = "";
        foreach (Pokemon pokemon in appController.pokemon)
        {
            jsonString = JsonUtility.ToJson(pokemon);
        }

        //string jsonString = File.ReadAllText(Application.dataPath + "/Resources/json Files/Pokemon Game Import.json");

        Debug.Log(Application.dataPath + "/Resources/json Files/Pokemon Game Import.json");
       
        if (jsonData != null)
        {
            Debug.Log("jsonData = " + jsonData);
        }
        else
        {
            Debug.Log("jsonData is null");
        }


        if (jsonString != "")
        {
            Debug.Log("jsonString = " + jsonData);
        }
        else
        {
            Debug.Log("jsonString is empty");
        }
    }

    //public void SaveData()
    //{
    //    string path = "Assets/Resources/patients-data.json";

    //    Pokemon[] pokemon_data = appController.pokemon.ToArray();

    //    Debug.Log(pokemon_data.Length);

    //    StreamWriter writer = new StreamWriter(path, false);
    //    string jsonData = JsonUtility.ToJson(pokemon_data[0].name, true);

    //    //foreach (Pokemon pokemon in pokemon_data)
    //    //{
    //    //    jsonData = JsonUtility.ToJson(pokemon.name, true);
    //    //}
    //    //string jsonData = JsonUtility.ToJson(pokemon_data[0], false);

    //    Debug.Log(jsonData);
    //    writer.Write(jsonData);
    //    writer.Close();
    //}
}
