using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ViewPokeDexDataPage : MonoBehaviour
{
    public AppController appController;

    List<string> dexDropdownOptions = new List<string>();

    Dropdown selector;

    Text infoTextBox;

    void Start()
    {
        selector = GameObject.Find("Dex Selector").GetComponent<Dropdown>();
        infoTextBox = GameObject.Find("Info Text Box").GetComponent<Text>();

        dexDropdownOptions.Add("Select Poke Dex");

        string[] dexFiles = Directory.GetFiles(Application.dataPath + "/Resources/Poke Dex/", "*.dat", SearchOption.AllDirectories);

        List<string> dexes = new List<string>();
        
        foreach (string file in dexFiles)
        {
            if (!file.Contains(".meta"))
            {
                dexes.Add(file);
            }
        }

        for (int i = 0; i < dexes.Count; i++)
        {
            dexes[i] = dexes[i].Replace(Application.dataPath + "/Resources/Poke Dex/", "");
            dexes[i] = dexes[i].Replace(".dat", "");
        }

        foreach (string dex in dexes)
        {
            dexDropdownOptions.Add(dex);
        }

        selector.ClearOptions();
        selector.AddOptions(dexDropdownOptions);
    }

    public void ChangeDex()
    {
        if (selector.value != 0)
        {
            PokeDex dex = DataControl.LoadPokeDex(Application.dataPath + "/Resources/Poke Dex/" + selector.captionText.text + ".dat");

            string infoText = "";

            for (int i = 0; i < dex.dexNumbers.Length; i++)
            {
                if (dex.dexNumbers[i] < 10)
                {
                    infoText += "# 00" + dex.dexNumbers[i] + ": ";
                }
                else if (dex.dexNumbers[i] < 100)
                {
                    infoText += "# 0" + dex.dexNumbers[i] + ": ";
                }
                else
                {
                    infoText += "# " + dex.dexNumbers[i] + ": ";
                }

                infoText += dex.pokemonNames[i] + "\n";
            }

            Debug.Log("" + infoText.Length);

            infoTextBox.text = infoText;
        }
        else
        {
            ResetData();
        }
    }

    void ResetData()
    {
        infoTextBox.text = "";
    }
}
