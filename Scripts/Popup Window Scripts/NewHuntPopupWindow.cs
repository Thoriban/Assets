using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewHuntPopupWindow : MonoBehaviour
{
    AppController appController;

    public GameObject nameTextBox;
    public Text pokemonNameTextBox;
    public Text bestOddsTextBox;
    public Text baseOddsTextBox;
    public GameObject normalForm;
    public GameObject shinyForm;
    public Dropdown gameDropdown;
    public Dropdown methodDropdown;

    public InputField newCounterTitle;

    string pokemonName;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    // Update is called once per frame
    public void UpdatePokemonInfo(string pokemonName)
    {
        this.pokemonName = pokemonName;
        pokemonNameTextBox = nameTextBox.GetComponent<Text>();
        pokemonNameTextBox.text = pokemonName;
    }

    public void UpdateRegularFilename(string filename)
    {
        bool success = SpriteController.LoadArt(normalForm, filename);

        if (!success)
        {
            Log.Write("FAILED TO CREATE ART FOR REGULAR FORM");
        }
    }

    public void UpdateShinyFilename(string filename)
    {
        bool success = SpriteController.LoadArt(shinyForm, filename);

        if (!success)
        {
            Log.Write("FAILED TO CREATE ART FOR SHINY FORM");
        }
    }

    public void PopulateGames()
    {
        List<string> options = new List<string>();
        Pokemon pokemon = DataControl.LoadPokemon(Application.dataPath + "/Resources/Pokemon/" + pokemonName + ".dat");

        options.Add("<i>Select Game</i>");
        foreach (string game in pokemon.availableGames)
        {
            options.Add(game);
        }

        bestOddsTextBox = GameObject.Find("Best Odds Text Box").GetComponent<Text>();
        baseOddsTextBox = GameObject.Find("Base Odds Text Box").GetComponent<Text>();
        gameDropdown = GameObject.Find("Game Selector").GetComponent<Dropdown>();
        methodDropdown = GameObject.Find("Method Selector").GetComponent<Dropdown>();
        gameDropdown.ClearOptions();
        gameDropdown.AddOptions(options);
    }

    public void ChangeGame()
    {
        if (gameDropdown.value != 0)
        {
            PopulateMethods(gameDropdown.captionText.text);
            methodDropdown.gameObject.SetActive(true);

        }
        else
        {
            methodDropdown.gameObject.SetActive(false);
            baseOddsTextBox.text = "";
            bestOddsTextBox.text = "";
        }
    }

    void PopulateMethods(string game)
    {
        List<string> options = new List<string>();
        Pokemon pokemon = DataControl.LoadPokemon(Application.dataPath + "/Resources/Pokemon/" + pokemonName + ".dat");

        options.Add("<i>Select Shiny Hunting Method</i>");
        if (game == "")
        {
            foreach (string method in pokemon.availableMethods)
            {
                options.Add(method);
            }

        }
        else
        {
            string[] methods = Directory.GetFiles(Application.dataPath + "/Resources/Shiny Hunting Method/" + game + "/", "*.dat", SearchOption.AllDirectories);

            for (int i = 0; i < methods.Length; i++)
            {
                methods[i] = methods[i].Replace(Application.dataPath + "/Resources/Shiny Hunting Method/" + game + "/", string.Empty);
                methods[i] = methods[i].Replace(".dat", string.Empty);
            }

            foreach (string method in methods)
            {
                if (!method.Contains(".meta"))
                {
                    options.Add(method);
                }
            }
        }

        methodDropdown.ClearOptions();
        methodDropdown.AddOptions(options);
    }

    public void ChangeMethod()
    {
        if (methodDropdown.value != 0)
        {
            ShinyMethod fullOddsMethod = DataControl.LoadShinyHuntingMethod(Application.dataPath + "/Resources/Shiny Hunting Method/" + gameDropdown.captionText.text + "/Full Odds.dat");

            string text = "Base Odds: " + fullOddsMethod.ConvertDecimalToFraction(fullOddsMethod.baseOddsValues[0]);
            baseOddsTextBox.text = text;

            //string gameName = gameDropdown.captionText.text.Replace("Pokemon ", "");
            //Game game = DataControl.LoadGame(Application.dataPath + "/Resources/Game/" + gameName + ".dat");
            //if (game.shinyCharmAvailable)
            //{

            ShinyMethod method = DataControl.LoadShinyHuntingMethod(Application.dataPath + "/Resources/Shiny Hunting Method/" + gameDropdown.captionText.text + "/" + methodDropdown.captionText.text + ".dat");
            bestOddsTextBox.text = "Best Odds: " + method.ConvertDecimalToFraction(method.shinyCharmOddsValues[method.shinyCharmOddsValues.Length - 1]);
            //}

        }
        else
        {
            baseOddsTextBox.text = "";
            bestOddsTextBox.text = "";
        }
    }

    public void BeginButton()
    {
        if (SceneManager.GetActiveScene().name == "New Hunt Menu")
        {
            if (gameDropdown.value != 0 && methodDropdown.value != 0)
            {
                ShinyHunt hunt = new ShinyHunt(Application.dataPath);
                hunt.pokemon = pokemonName;
                hunt.game = gameDropdown.captionText.text;
                hunt.method = methodDropdown.captionText.text;

                hunt.Save();

                appController = GameObject.Find("AppController").GetComponent<AppController>();
                appController.huntId = pokemonName + " - " + hunt.secretId;

                SceneManager.LoadScene("Hunt Page");
            }
        }
        else if (SceneManager.GetActiveScene().name == "Hunt Page")
        {
            newCounterTitle = GameObject.Find("Counter Title Input Field").GetComponent<InputField>();

            if (newCounterTitle.text != "")
            {
                HuntPage huntPage = GameObject.Find("Main Camera").GetComponent<HuntPage>();
                ShinyHunt hunt = huntPage.hunt;

                List<string> counterTitles = hunt.counterTitles.ToList();
                List<int> counterValues = hunt.counterValues.ToList();
                List<float> counterTimers = hunt.counterTimers.ToList();

                if (hunt.counterTitles[0] != null)
                {
                    Debug.Log("hunt.counterTitles[0] = "  + hunt.counterTitles[0]);
                    if (hunt.counterTitles[0] != "")
                    {
                        counterTitles.Add(newCounterTitle.text);
                        counterValues.Add(0);
                        counterTimers.Add(0);

                        //int index = 0;

                        //foreach (GameObject counter in huntPage.counters.Values)
                        //{
                        //    Counter counterScript = counter.GetComponent<Counter>();

                        //    hunt.counterTitles[index] = counterScript.counterName;
                        //    hunt.counterValues[index] = counterScript.counterValue;
                        //    hunt.counterTimers[index] = counterScript.totalTime;

                        //    index++;
                        //}

                        hunt.counterTitles = counterTitles.ToArray();
                        hunt.counterValues = counterValues.ToArray();
                        hunt.counterTimers = counterTimers.ToArray();
                    }
                    else
                    {
                        hunt.counterTitles[0] = newCounterTitle.text;
                        hunt.counterValues[0] = 0;
                        hunt.counterTimers[0] = 0f;
                    }
                }

                huntPage.counters.Add(newCounterTitle.text, huntPage.CreateCounter(newCounterTitle.text, 0, 0));

                hunt.Save();
                Debug.Log("# counters " + hunt.counterTitles.Length);

                huntPage.ClosePopUpWindow();
            }

        }
    }

    public void CancelButton()
    {
        SetActive(false);
    }
}
