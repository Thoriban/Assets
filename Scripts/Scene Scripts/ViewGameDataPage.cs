using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ViewGameDataPage : MonoBehaviour
{
    public AppController appController;

    public GameObject[] buttons;
    public GameObject[] labels;

    Game game;

    Text titleTextBox;
    Text gameInformationTextBox;

    void Start()
    {
        buttons = new GameObject[]
        {
            GameObject.Find("Game Data Button"),
            GameObject.Find("Methods Button"),
            GameObject.Find("Pokemon Button")
        };

        labels = new GameObject[]
        {
            GameObject.Find("Game Data Label"),
            GameObject.Find("Method Label"),
            GameObject.Find("Pokemon Label")
        };


        appController = GameObject.Find("AppController").GetComponent<AppController>();
        titleTextBox = GameObject.Find("Game Title Text Box").GetComponent<Text>();
        gameInformationTextBox = GameObject.Find("Game Information Text Box").GetComponent<Text>();

        game = DataControl.LoadGame(Application.dataPath + "/Resources/Game/" + appController.gameName + ".dat");

        titleTextBox.text = "<b>Pokémon " + game.name + "</b>";

        GameInformationButton();
    }

    public void GameInformationButton()
    {
        ShinyMethod fullOdds = DataControl.LoadShinyHuntingMethod(Application.dataPath + "/Resources/Shiny Hunting Method/Pokemon " + game.name + "/Full Odds.dat");

        buttons[0].SetActive(false);
        buttons[1].SetActive(true);
        buttons[2].SetActive(true);

        labels[0].SetActive(true);
        labels[1].SetActive(false);
        labels[2].SetActive(false);

        string text = string.Empty;

        text += "<b>Playable on the following consoles</b>\n";

        for (int i = 0; i < game.playableConsoles.Length; i++)
        {
            if (game.playableConsoles[i] != "")
            {
                text += "   • " + game.playableConsoles[i] + "\n";
            }
        }

        text += "\n";

        text += "<b>Region: </b>" + game.regionName + "\n";

        text += "\n";

        text += "<b>Base Shiny Odds: </b>" + fullOdds.ConvertDecimalToFraction(fullOdds.baseOddsValues[0]) + "\n";

        text += "\n";

        text += "<b>Shiny Charm Available:</b>";
        if (game.shinyCharmAvailable)
        {
            text += " Yes\n";
        }
        else
        {
            text += " No\n";
        }


        gameInformationTextBox.text = text;
    }

    public void MethodButton()
    {
        buttons[0].SetActive(true);
        buttons[1].SetActive(false);
        buttons[2].SetActive(true);

        labels[0].SetActive(false);
        labels[1].SetActive(true);
        labels[2].SetActive(false);

        string text = string.Empty;

        text += "<b>Available Methods</b>\n";

        for (int i = 0; i < game.availableMethods.Length; i++)
        {
            ShinyMethod method = DataControl.LoadShinyHuntingMethod(Application.dataPath + "/Resources/Shiny Hunting Method/Pokemon " + game.name + "/" + game.availableMethods[i] + ".dat");
            
            if (game.availableMethods[i] != "")
            {
                text += "<b>" + method.methodName + "</b>";

                if (method.baseOddsKeys != null)
                {
                    text += "\n";
                    text += "Base Odds\n";

                    for (int j = 0; j < method.baseOddsKeys.Length; j++)
                    {
                        text += "" + method.baseOddsKeys[j] + " " + method.methodType + " = " + method.ConvertDecimalToFraction(method.baseOddsValues[j]) + "\n";
                    } 
                }

                if (game.shinyCharmAvailable)
                {
                    if (method.shinyCharmOddsKeys != null)
                    {
                        text += "\n";
                        text += "Shiny Charm Odds\n";

                        for (int j = 0; j < method.shinyCharmOddsKeys.Length; j++)
                        {
                            text += "" + method.shinyCharmOddsKeys[j] + " " + method.methodType + " = " + method.ConvertDecimalToFraction(method.shinyCharmOddsValues[j]) + "\n";
                        }
                    } 
                }
            }

            text += "\n";
        }

        gameInformationTextBox.text = text;
    }

    public void PokemonButton()
    {
        buttons[0].SetActive(true);
        buttons[1].SetActive(true);
        buttons[2].SetActive(false);

        labels[0].SetActive(false);
        labels[1].SetActive(false);
        labels[2].SetActive(true);

        string text = string.Empty;
        text += "<b>Pokemon Available To Catch:</b>\n";
        Dictionary<string, int> unsortedCollection = new Dictionary<string, int>();

        foreach (string poke in game.availablePokemon)
        {
            unsortedCollection.Add(poke, DataControl.LoadPokemon(Application.dataPath + "/Resources/Pokemon/" + poke + ".dat").dexNumber);
        }

        var sortedCollection = unsortedCollection.ToList();
        sortedCollection.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

        foreach (KeyValuePair<string, int> pair in sortedCollection)
        {
            if (pair.Key != "")
            {
                string dexNumber = string.Empty;

                if (pair.Value < 10)
                {
                    dexNumber += "00" + pair.Value;
                }
                else if (pair.Value < 100)
                {
                    dexNumber += "0" + pair.Value;
                }
                else
                {
                    dexNumber += "" + pair.Value;
                }

                string pokemonName = pair.Key;
                pokemonName = pokemonName.Replace("(F)", "♀");
                pokemonName = pokemonName.Replace("(M)", "♂");

                text += "   • #" + dexNumber + " " + pokemonName + "\n";
            }
        }

        gameInformationTextBox.text = text;
    }
}
