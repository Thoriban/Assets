using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EditPokemonRouteDataPage : MonoBehaviour
{
    public AppController appController;

    Pokemon selectedPokemon;
    Game selectedGame;

    List<string> pokemonDropdownOptions = new List<string>();
    List<string> gameDropdownOptions = new List<string>();
    List<string> routeDropdownOptions = new List<string>();

    Dropdown pokemonSelector;
    Dropdown gameSelector;
    Dropdown routeSelector;
    Dropdown encounterTypeSelector;

    InputField percentChangeInput;

    Text infoTextBox;
    Text chancePlaceholderTextBox;

    void Start()
    {
        pokemonSelector = GameObject.Find("Pokemon Selector").GetComponent<Dropdown>();
        gameSelector = GameObject.Find("Game Selector").GetComponent<Dropdown>();
        routeSelector = GameObject.Find("Route Selector").GetComponent<Dropdown>();
        encounterTypeSelector = GameObject.Find("Encounter Type Selector").GetComponent<Dropdown>();

        infoTextBox = GameObject.Find("Info Text Box").GetComponent<Text>();
        chancePlaceholderTextBox = GameObject.Find("Chance Placeholder").GetComponent<Text>();

        percentChangeInput = GameObject.Find("Chance Input Field").GetComponent<InputField>();

        pokemonDropdownOptions.Add("<i>Select Pokemon</i>");

        foreach (Pokemon pokemon in appController.pokemon)
        {
            pokemonDropdownOptions.Add(pokemon.name);
        }

        pokemonSelector.ClearOptions();
        pokemonSelector.AddOptions(pokemonDropdownOptions);

        gameDropdownOptions.Add("Please Select Pokemon First");
        gameSelector.ClearOptions();
        gameSelector.AddOptions(gameDropdownOptions);
    }


    public void ChangePokemon()
    {
        if (pokemonSelector.value != 0)
        {
            selectedPokemon = DataControl.LoadPokemon(Application.dataPath + "/Resources/Pokemon/" + pokemonSelector.captionText.text + ".dat");

            if (selectedPokemon.availableGames.Length != 0)
            {
                gameDropdownOptions.Clear();
                gameDropdownOptions.Add("<i>Select Game</i>");


                foreach (string gameName in selectedPokemon.availableGames)
                {
                    gameDropdownOptions.Add(gameName);
                }

                gameSelector.ClearOptions();
                gameSelector.AddOptions(gameDropdownOptions);

                if (selectedPokemon.availableLocations.Length == 1 | selectedPokemon.availableLocations == null)
                {
                    CreateRouteListsForAvailableGames();
                }
            }
        }
        else
        {
            selectedPokemon = null;
            ResetData();
        }

        UpdateText();
    }

    void CreateRouteListsForAvailableGames()
    {
        List<RouteList> routeLists = new List<RouteList>();
        foreach (string gameName in selectedPokemon.availableGames)
        {
            RouteList routeList = new RouteList();
            routeList.gameName = gameName;
            routeLists.Add(routeList);
        }

        selectedPokemon.availableLocations = RouteList.ToArray(routeLists);
        selectedPokemon.Save();
    }

    public void ChangeGame()
    {
        if (gameSelector.value != 0)
        {
            string gameName = gameSelector.captionText.text.Replace("Pokemon ", "");

            selectedGame = DataControl.LoadGame(Application.dataPath + "/Resources/Game/" + gameName + ".dat");

            Debug.Log(selectedGame.locations.Length);

            if (selectedGame.locations.Length != 0)
            {
                routeSelector.value = 0;
                routeDropdownOptions.Clear();
                routeDropdownOptions.Add("<i>Select Route</i>");

                foreach (string route in selectedGame.locations)
                {
                    routeDropdownOptions.Add(route);
                }

                routeSelector.ClearOptions();
                routeSelector.AddOptions(routeDropdownOptions);
            }
        }
        else
        {
            ResetData();
        }
    }

    public void ChangeEncounterType()
    {
        switch (encounterTypeSelector.captionText.text)
        {
            case "Wild Encounter": chancePlaceholderTextBox.text = "Enter Chance in %"; break;
            case "Fixed Encounter": chancePlaceholderTextBox.text = "Leave Blank"; break;
            case "Starter Choice": chancePlaceholderTextBox.text = "Leave Blank"; break;
            case "Choice": chancePlaceholderTextBox.text = "Leave Blank"; break;
            case "Gift": chancePlaceholderTextBox.text = "Leave Blank"; break;
            case "Purchase": chancePlaceholderTextBox.text = "Enter Cost and Currency Here"; break;
            case "Trade": chancePlaceholderTextBox.text = "Enter Pokemon Required"; break;
            default: chancePlaceholderTextBox.text = "Select Encounter Type First"; break;
        }
    }

    void ResetData()
    {
        infoTextBox.text = "";

        gameSelector.value = 0;
        gameDropdownOptions.Clear();
        gameDropdownOptions.Add("Please Select Pokemon First");
        gameSelector.ClearOptions();
        gameSelector.AddOptions(gameDropdownOptions);

        routeSelector.value = 0;
        routeDropdownOptions.Clear();
        routeDropdownOptions.Add("<i>Please Select Pokemon & Game First</i>");
        routeSelector.ClearOptions();
        routeSelector.AddOptions(routeDropdownOptions);
    }

    public void AddRoute()
    {
        if (routeSelector.value != 0)
        {
            bool gameFound = false;

            int gameIndex = 0;

            RouteList routeList = new RouteList();

            if (selectedPokemon.availableLocations.Length != 0)
            {
                if (selectedPokemon.availableLocations[0] != null)
                {
                    foreach (RouteList list in selectedPokemon.availableLocations)
                    {
                        if (list.gameName == gameSelector.captionText.text)
                        {
                            routeList = list;
                            gameFound = true;
                            break;
                        }

                        gameIndex++;
                    }
                }
            }

            List<string> routeNames = routeList.routeNames.ToList();
            List<int> encounterTypes = routeList.encounterTypes.ToList();
            List<string> encounterDescriptions = routeList.encounterDescriptions.ToList();

            if (routeNames[0] == null | routeNames[0] == "")
            {
                routeNames.RemoveAt(0);
                encounterTypes.RemoveAt(0);
                encounterDescriptions.RemoveAt(0);
            }

            if (gameFound)
            {
                bool routeFound = false;
                int routeIndex = 0;

                foreach (string route in routeNames)
                {
                    if (route == routeSelector.captionText.text)
                    {
                        routeFound = true;
                        break;
                    }

                    routeIndex++;
                }

                if (routeFound)
                {
                    encounterTypes[routeIndex] = encounterTypeSelector.value;
                    encounterDescriptions[routeIndex] = percentChangeInput.text;
                }
                else
                {
                    routeNames.Add(routeSelector.captionText.text);
                    encounterTypes.Add(encounterTypeSelector.value);
                    encounterDescriptions.Add(percentChangeInput.text);
                }

                routeList.routeNames = routeNames.ToArray();
                routeList.encounterTypes = encounterTypes.ToArray();
                routeList.encounterDescriptions = encounterDescriptions.ToArray();

                selectedPokemon.availableLocations[gameIndex] = routeList;
            }
            else
            {
                List<RouteList> routeLists = new List<RouteList>();

                if (selectedPokemon.availableLocations != null)
                {
                    if (selectedPokemon.availableLocations[0] != null)
                    {
                        string gameName = selectedPokemon.availableLocations[0].gameName;

                        if (gameName != "")
                        {
                            routeLists = selectedPokemon.availableLocations.ToList();
                        }
                    }
                    else
                    {
                        routeLists = new List<RouteList>();
                    }
                }

                routeNames.Add(routeSelector.captionText.text);
                encounterTypes.Add(encounterTypeSelector.value);
                encounterDescriptions.Add(percentChangeInput.text);

                routeList.gameName = gameSelector.captionText.text;
                routeList.routeNames = routeNames.ToArray();
                routeList.encounterTypes = encounterTypes.ToArray();
                routeList.encounterDescriptions = encounterDescriptions.ToArray();

                routeLists.Add(routeList);

                Debug.Log(routeLists.Count);
                selectedPokemon.availableLocations = RouteList.ToArray(routeLists);
            }

            selectedPokemon.Save();
        }

        UpdateText();
    }

    public void RemoveRoute()
    {
        if (routeSelector.value != 0)
        {
            bool gameFound = false;

            int gameIndex = 0;

            RouteList routeList = new RouteList();

            if (selectedPokemon.availableLocations.Length != 0)
            {
                if (selectedPokemon.availableLocations[0] != null)
                {
                    foreach (RouteList list in selectedPokemon.availableLocations)
                    {
                        if (list.gameName == gameSelector.captionText.text)
                        {
                            routeList = list;
                            gameFound = true;
                            break;
                        }

                        gameIndex++;
                    }
                }
            }

            if (gameFound)
            {
                bool routeFound = false;
                int routeIndex = 0;

                List<string> routeNames = routeList.routeNames.ToList();
                List<int> encounterTypes = routeList.encounterTypes.ToList();
                List<string> encounterDescriptions = routeList.encounterDescriptions.ToList();

                foreach (string route in routeNames)
                {
                    if (route == routeSelector.captionText.text)
                    {
                        routeFound = true;
                        break;
                    }

                    routeIndex++;
                }

                if (routeFound)
                {
                    routeNames.RemoveAt(routeIndex);
                    encounterTypes.RemoveAt(routeIndex);
                    encounterDescriptions.RemoveAt(routeIndex);
                }
                

                routeList.routeNames = routeNames.ToArray();
                routeList.encounterTypes = encounterTypes.ToArray();
                routeList.encounterDescriptions = encounterDescriptions.ToArray();

                selectedPokemon.availableLocations[gameIndex] = routeList;
                
                selectedPokemon.Save();
            }
        }

        UpdateText();
    }

    void UpdateText()
    {
        infoTextBox.text = "";

        if (selectedPokemon != null)
        {
            if (selectedPokemon.availableLocations == null)
            {
                selectedPokemon.availableLocations = new RouteList[1];
            }

            foreach (RouteList routeList in selectedPokemon.availableLocations)
            {
                if (routeList != null)
                {
                    infoTextBox.text += "<b>" + routeList.gameName + "</b>\n";
                    for (int i = 0; i < routeList.routeNames.Length; i++)
                    {
                        infoTextBox.text += routeList.routeNames[i] + ": " + routeList.encounterTypes[i] + " - " + routeList.encounterDescriptions[i] + "\n";
                    }

                    infoTextBox.text += "\n";
                }
            }
        }

        infoTextBox.text += "\n\n";
    }
}

