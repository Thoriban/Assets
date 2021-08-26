using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ViewMethodDataPage : MonoBehaviour
{
    public AppController appController;

    List<string> gameDropdownOptions = new List<string>();
    List<string> methodDropdownOptions = new List<string>();

    Dropdown gameSelector;
    Dropdown methodSelector;

    Text dataTextBox;

    void Start()
    {
        gameSelector = GameObject.Find("Game Selector").GetComponent<Dropdown>();
        methodSelector = GameObject.Find("Method Selector").GetComponent<Dropdown>();
        dataTextBox = GameObject.Find("Method Info Text Box").GetComponent<Text>();

        gameDropdownOptions.Add("Select Game");

        foreach (Game game in appController.games)
        {
            gameDropdownOptions.Add(game.name);
        }

        gameSelector.ClearOptions();
        gameSelector.AddOptions(gameDropdownOptions);
    }

    public void ChangeGame()
    {
        ResetData();

        if (gameSelector.value != 0)
        {
            UpdateMethodSelectorOptions(gameSelector.captionText.text);
        }
        else
        {
            methodDropdownOptions.Clear();
            methodDropdownOptions.Add("Select Shiny Hunting Method");
            methodSelector.ClearOptions();
            methodSelector.AddOptions(methodDropdownOptions);
        }

        methodSelector.value = 0;
    }

    public void ChangeMethod()
    {
        ResetData();

        if (methodSelector.value != 0)
        {
            ShinyMethod method = DataControl.LoadShinyHuntingMethod(Application.dataPath + "/Resources/Shiny Hunting Method/Pokemon " + gameSelector.captionText.text + "/" + methodSelector.captionText.text + ".dat");

            string dataText = "";

            dataText += AddCategoryToDataText("Method Name", method.methodName);
            dataText += AddCategoryToDataText("Game", method.game);
            dataText += AddCategoryToDataText("Encounter Type", method.methodType);
            dataText += AddCategoryToDataText("Method Description", method.description);
            dataText += AddCategoryToDataText("Base Odds", "");
            dataText += AddTableEntryToDataText("Encounters", "Odds (%)", "Odds (1/x)");

            if (method.baseOddsKeys.Length != 0)
            {
                for (int i = 0; i < method.baseOddsKeys.Length; i++)
                {
                    float odds = method.baseOddsValues[i];
                    dataText += AddTableEntryToDataText("" + method.baseOddsKeys[i], method.ConvertDecimalToPercentage(odds), method.ConvertDecimalToFraction(odds));
                }

                dataText += "\n";
            }

            if (method.effectedByShinyCharm)
            {
                dataText += AddCategoryToDataText("Shiny Charm Odds", "");
                dataText += AddTableEntryToDataText("Encounters", "Odds (%)", "Odds (1/x)");

                if (method.shinyCharmOddsKeys.Length != 0)
                {
                    for (int i = 0; i < method.shinyCharmOddsKeys.Length; i++)
                    {
                        float odds = method.shinyCharmOddsValues[i];
                        dataText += AddTableEntryToDataText("" + method.shinyCharmOddsKeys[i], method.ConvertDecimalToPercentage(odds), method.ConvertDecimalToFraction(odds));
                    }
                }

                dataText += "\n";
            }

            dataTextBox.text = dataText;
        }
        else
        {
            methodDropdownOptions.Clear();
            methodDropdownOptions.Add("Select Shiny Hunting Method");
            methodSelector.ClearOptions();
            methodSelector.AddOptions(methodDropdownOptions);
        }
    }

    string AddCategoryToDataText(string title, string data)
    {
        return "<b>" + title + ":</b> " + data + "\n\n";
    }

    string AddTableEntryToDataText(string key, string value)
    {
        return "" + key + " | " + value + "\n";
    }

    string AddTableEntryToDataText(string key, string value, string modValue)
    {
        return "" + key + " | " + value + " | " + modValue + "\n";
    }

    void UpdateMethodSelectorOptions(string gameName)
    {
        string[] methods = Directory.GetFiles(Application.dataPath + "/Resources/Shiny Hunting Method/Pokemon " + gameName + "/", "*.dat", SearchOption.AllDirectories);

        for (int i = 0; i < methods.Length; i++)
        {
            methods[i] = methods[i].Replace(Application.dataPath + "/Resources/Shiny Hunting Method/Pokemon " + gameName + "/", string.Empty);
            methods[i] = methods[i].Replace(".dat", string.Empty);
        }

        methodDropdownOptions.Clear();
        methodDropdownOptions.Add("Select Shiny Hunting Method");

        foreach (string method in methods)
        {
            if (!method.Contains(".meta"))
            {
                methodDropdownOptions.Add(method);
            }
        }

        methodSelector.ClearOptions();
        methodSelector.AddOptions(methodDropdownOptions);
    }

    void ResetData()
    {
        dataTextBox.text = "";
    }
}
