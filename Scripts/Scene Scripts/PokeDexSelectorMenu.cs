using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PokeDexSelectorMenu : MonoBehaviour
{
    public GameObject dexButton;
    public GameObject blankObject;
    public GameObject gridLayout;

    /*####### TO DO #######
     * 
     * ----REGION LIST----
     * Add a region list somewhere that keeps track of the currently available regions and their generation number
     * This should replace the switch statement in Start
     * 
     * ####################
     */

    void Start()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Poke Dex/", "*.dat", SearchOption.AllDirectories);

        Dictionary<int, string> dexes = new Dictionary<int, string>();

        foreach (string file in files)
        {
            if (!file.Contains("Form") && !file.Contains(".meta"))
            {
                string region = file.Replace(Application.dataPath + "/Resources/Poke Dex/", "");
                region = region.Replace(" Dex.dat", "");

                switch (region)
                {
                    case "Kanto": dexes.Add(1, region); break;
                    case "Johto": dexes.Add(2, region); break;
                    case "Sinnoh": dexes.Add(3, region); break;
                    case "Hoenn": dexes.Add(4, region); break;
                    case "Unova": dexes.Add(5, region); break;
                    case "Kalos": dexes.Add(6, region); break;
                    case "Alola": dexes.Add(7, region); break;
                    case "Galar": dexes.Add(8, region); break;
                    default: dexes.Add(0, region); break;
                }
            }
        }

        var sortedCollection = dexes.ToList();

        sortedCollection.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));


        for (int i = 0; i < dexes.Count; i++)
        {
            InstantiatePokemonImages(sortedCollection[i].Key, sortedCollection[i].Value);
        }

        InstantiateBlankObjects();
    }

    void InstantiatePokemonImages(int generation, string dexName)
    {
        GameObject button = Instantiate(dexButton, new Vector3(0, 0, 0), Quaternion.identity);
        button.transform.SetParent(gridLayout.transform);
        button.transform.name = dexName + " Button";

        PokeDexButtonScript script = button.GetComponent<PokeDexButtonScript>();
        script.generation = generation;

        Text text = button.GetComponentInChildren<Text>();
        text.gameObject.transform.name = dexName + " Text Box";

        text.text = dexName + " Pokédex";
    }

    void InstantiateBlankObjects()
    {
        GameObject blankIcon = Instantiate(blankObject, new Vector3(0, 0, 0), Quaternion.identity);
        blankIcon.transform.SetParent(gridLayout.transform);
    }
}
