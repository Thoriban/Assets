using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuntPage : MonoBehaviour
{
    public AppController appController;
    public GameObject counterPrefab;
    public GameObject counterGridLayout;

    GameObject standardImage;
    GameObject shinyImage;

    Text pokemonNameTextBox;
    Text huntingMethodTextBox;
    Text averageTimeTextBox;

    public Dictionary<string, GameObject> counters;

    GameObject addCounterPopupWindow;

    public ShinyHunt hunt;

    void Start()
    {
        counters = new Dictionary<string, GameObject>();

        FindGameObjects();

        if (appController.huntId != "")
        {
            hunt = DataControl.LoadShinyHunt(Application.dataPath + "/Resources/Shiny Hunts/" + appController.huntId + ".dat");
        }
        else
        {
            hunt = new ShinyHunt(Application.dataPath);
        }

        PopulateHuntPage();

        addCounterPopupWindow.SetActive(false);
    }

    // Update is called once per frame
    void FindGameObjects()
    {
        appController = GameObject.Find("AppController").GetComponent<AppController>();
        standardImage = GameObject.Find("Pokemon Image");
        shinyImage = GameObject.Find("Pokemon Shiny Image");
        addCounterPopupWindow = GameObject.Find("Popup Window");

        pokemonNameTextBox = GameObject.Find("Pokemon Name Text Box").GetComponent<Text>();
        huntingMethodTextBox = GameObject.Find("Hunting Method Text Box").GetComponent<Text>();
        averageTimeTextBox = GameObject.Find("Timer Text Box").GetComponent<Text>();
    }

    void PopulateHuntPage()
    {
        if (hunt.pokemon != "")
        {
            pokemonNameTextBox.text = hunt.pokemon;
            SetRegularFilename(Application.dataPath + "/Art Assets/Pokemon/Caught/Normal/" + hunt.pokemon + ".png");
            SetShinyFilename(Application.dataPath + "/Art Assets/Pokemon/Caught/Shiny/" + hunt.pokemon + ".png");
        }
        else
        {
            pokemonNameTextBox.text = "ERROR 404: A Wild MissingNo Appeared!";
        }

        string methodText = string.Empty;
        methodText = hunt.game.Replace("Pokemon ", "");

        huntingMethodTextBox.text = methodText + " : " + hunt.method;

        DateTime averageTime = new DateTime();
        averageTime.AddSeconds(hunt.averageEncounterTime);
        averageTimeTextBox.text = "Average Encounter Time:\n";

        if (averageTime.Hour != 0)
        {
            averageTimeTextBox.text += averageTime.Hour + "h ";
        }

        if (averageTime.Minute != 0)
        {
            averageTimeTextBox.text += averageTime.Minute + "m ";
        }

        averageTimeTextBox.text += averageTime.Second + "s";

        if (hunt.counterTitles[0] != null)
        {
            if (hunt.counterTitles.Length != 0)
            {
                if (hunt.counterTitles[0] != "")
                {
                    for (int i = 0; i < hunt.counterTitles.Length; i++)
                    {
                        counters.Add(hunt.counterTitles[i], CreateCounter(hunt.counterTitles[i], hunt.counterValues[i], hunt.counterTimers[i]));
                    }
                }
            }
        }
        else
        {
            counters.Add("Encounters", CreateCounter("Encounters", 0, 0));
        }
    }

    public void SetRegularFilename(string filename)
    {
        bool success = SpriteController.LoadArt(standardImage, filename);

        if (!success)
        {
            Log.Write("FAILED TO CREATE ART FOR REGULAR FORM");
        }
    }

    public void SetShinyFilename(string filename)
    {
        bool success = SpriteController.LoadArt(shinyImage, filename);

        if (!success)
        {
            Log.Write("FAILED TO CREATE ART FOR SHINY FORM");
        }
    }

    public GameObject CreateCounter(string counterName, int counterValue, float counterTime)
    {
        GameObject counter = Instantiate(counterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        counter.name = counterName;
        counter.transform.parent = counterGridLayout.transform;

        Counter counterScript = counter.GetComponent<Counter>();
        counterScript.InitialiseTextBoxes();
        counterScript.hunt = hunt;
        Debug.Log(counterScript.SetCounterName(counterName));
        counterScript.SetCounterValue(counterValue);
        counterScript.SetCurrentTime(counterTime);

        return counter;
    }

    public void ShinyButton()
    {
        Debug.Log("Hunt Info: Filename will be " + hunt.pokemon + " - " + hunt.secretId + ".dat");
        //Load Shiny Caught Page/Window
        //Once Shiny is Confirmed, add Shiny to Collections and Delete Hunt file.
        //hunt = new ShinyHunt(Application.dataPath);
    }

    public void OpenPopupWindow()
    {
        if (addCounterPopupWindow != null)
        {
            addCounterPopupWindow.SetActive(true);
        }
    }

    public void ClosePopUpWindow()
    {
        if (addCounterPopupWindow != null)
        {
            addCounterPopupWindow.SetActive(false);
        }
    }

    public void AddCounterButton()
    {
        addCounterPopupWindow.SetActive(true);
    }
}
