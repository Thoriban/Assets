using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CurrentHuntsMenuPage : MonoBehaviour
{
    public GameObject buttonGridLayout;
    public GameObject buttonPrefab;

    Dictionary<string, ShinyHunt> activeHunts;

    void Start()
    {
        GetHunts();

        foreach (KeyValuePair<string, ShinyHunt> hunt in activeHunts)
        {
            CreateHuntButton(hunt);
        }
    }

    void GetHunts()
    {
        activeHunts = new Dictionary<string, ShinyHunt>();
        string[] activeHuntPaths = Directory.GetFiles(Application.dataPath + "/Resources/Shiny Hunts/");

        foreach (string path in activeHuntPaths)
        {
            if (!path.EndsWith(".meta"))
            {
                ShinyHunt hunt = DataControl.LoadShinyHunt(path);

                string huntName = path.Replace(Application.dataPath + "/Resources/Shiny Hunts/", "");
                huntName = huntName.Replace(".dat", "");
                activeHunts.Add(huntName, hunt);
            }
        }
    }

    GameObject CreateHuntButton(KeyValuePair<string, ShinyHunt> hunt)
    {
        GameObject button = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        button.name = hunt.Key;
        button.transform.parent = buttonGridLayout.transform;

        HuntButtonScript buttonScript = button.GetComponent<HuntButtonScript>();
        buttonScript.huntName = hunt.Key;

        Text buttonText = button.GetComponentInChildren<Text>();
        buttonText.gameObject.transform.name = hunt.Key + " Text Box";

        buttonText.text = hunt.Value.pokemon + " - " + hunt.Value.game + " : " + hunt.Value.method;

        return button;
    }
}
