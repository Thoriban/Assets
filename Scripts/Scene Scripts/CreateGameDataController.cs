using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateGameDataController : MonoBehaviour
{
    public DataPage pageController;

    public GameObject userInterfaceElements;
    public GameObject consoleInterfaceElements;
    public GameObject locationInterfaceElements;
    public GameObject pokemonInterfaceElements;
    public GameObject methodInterfaceElements;

    public GameObject consoleNameGridController;
    public GameObject consoleRemoverGridController;

    public GameObject namedLocationGridController;
    public GameObject namedLocationRemoverGridController;

    public GameObject pokemonNameGridController;
    public GameObject pokemonRemoverGridController;

    public GameObject methodNameGridController;
    public GameObject methodRemoverGridController;

    public GameObject item;
    public GameObject removeButton;

    public Dropdown consoleDropdown;
    public Dropdown pokemonDropdown;
    public Dropdown methodDropdown;

    public InputField nameInputField;
    public InputField lowRouteNumberInputField;
    public InputField highRouteNumberInputField;
    public InputField namedLocationsInputField;

    public Toggle shinyCharmToggle;

    List<Pokemon> availablePokemon;
    List<ShinyMethod> availableMethods;

    List<string> consoles;
    List<string> routeNames;
    List<string> namedLocations;

    public void Start()
    {
        pageController = GetComponent<DataPage>();

        availablePokemon = new List<Pokemon>();
        availableMethods = new List<ShinyMethod>();

        consoles = new List<string>();
        routeNames = new List<string>();
        namedLocations = new List<string>();

        FindGUIElements();

        Setup();
    }

    void FindGUIElements()
    {
        userInterfaceElements = GameObject.Find("Game - Data Elements");
        consoleInterfaceElements = GameObject.Find("Game - Manage Console Interface");
        locationInterfaceElements = GameObject.Find("Game - Manage Location Interface");
        pokemonInterfaceElements = GameObject.Find("Game - Manage Pokemon Interface");
        methodInterfaceElements = GameObject.Find("Game - Manage Shiny Hunting Methods Interface");

        consoleNameGridController = GameObject.Find("Game - Console Name Grid Controller");
        consoleRemoverGridController = GameObject.Find("Game - Console Remove Grid Controller");

        namedLocationGridController = GameObject.Find("Game - Location Name Grid Controller");
        namedLocationRemoverGridController = GameObject.Find("Game - Location Remove Grid Controller");

        pokemonNameGridController = GameObject.Find("Game - Pokemon Name Grid Controller");
        pokemonRemoverGridController = GameObject.Find("Game - Pokemon Remove Grid Controller");

        methodNameGridController = GameObject.Find("Game - Method Name Grid Controller");
        methodRemoverGridController = GameObject.Find("Game - Method Remove Grid Controller");

        consoleDropdown = GameObject.Find("Game - Console selection Dropdown").GetComponent<Dropdown>();
        pokemonDropdown = GameObject.Find("Game - Pokemon selection Dropdown").GetComponent<Dropdown>();
        methodDropdown = GameObject.Find("Game - Method selection Dropdown").GetComponent<Dropdown>();

        nameInputField = GameObject.Find("Game - Name InputField").GetComponent<InputField>();
        lowRouteNumberInputField = GameObject.Find("Game - Lowest Route Number InputField").GetComponent<InputField>();
        highRouteNumberInputField = GameObject.Find("Game - Highest Route Number InputField").GetComponent<InputField>();
        namedLocationsInputField = GameObject.Find("Game - Named Locations Input Field").GetComponent<InputField>();

        shinyCharmToggle = GameObject.Find("Game - Shiny Charm Available Toggle").GetComponent<Toggle>();
    }

    public void Setup()
    {
        DeactivateElements();
        PopulateDropdowns();
    }

    public void PopulateDropdowns()
    {
        pageController.PopulateDropdown("Pokemon", pokemonDropdown);
        pageController.PopulateDropdown("Method", methodDropdown);
    }

    public void ActivateElements()
    {
        userInterfaceElements.SetActive(true);
    }

    public void DeactivateElements()
    {
        userInterfaceElements.SetActive(false);
        consoleInterfaceElements.SetActive(false);
        locationInterfaceElements.SetActive(false);
        pokemonInterfaceElements.SetActive(false);
        methodInterfaceElements.SetActive(false);
    }

    #region console interface
    public void ActivateConsoleInterfaceElements()
    {
        consoleInterfaceElements.SetActive(true);
    }

    public void DeactivateConsoleInterfaceElements()
    {
        consoleInterfaceElements.SetActive(false);
    }

    public void AddConsoleToGame()
    {
        if (consoleDropdown.value != 0)
        {
            if (!consoles.Contains(consoleDropdown.captionText.text))
            {
                consoles.Add(consoleDropdown.captionText.text);

                //instantiate a new text box and button pair that displays the console in the panel with the button removing it from the list if pressed
                GameObject console = Instantiate(item, consoleNameGridController.transform);
                Text consoleName = console.GetComponentInChildren<Text>();
                consoleName.text = consoleDropdown.captionText.text;

                GameObject buttonGameObject = Instantiate(removeButton, consoleRemoverGridController.transform);

                RemoveScript buttonScript = buttonGameObject.GetComponent<RemoveScript>();
                buttonScript.itemName = consoleDropdown.captionText.text;
                buttonScript.SetItemToRemove(console);

                Button button = buttonGameObject.GetComponent<Button>();
                button.onClick.AddListener(delegate () { buttonScript.DeleteElementFromList(RemoveConsoleFromGame); });
            }
        }
    }

    public void RemoveConsoleFromGame(string item)
    {
        for (int i = 0; i < consoles.Count; i++)
        {
            if (consoles[i] == item)
            {
                consoles.RemoveAt(i);
                break;
            }
        }
    }
    #endregion

    #region Pokemon Interface
    public void ActivatePokemonInterfaceElements()
    {
        pokemonInterfaceElements.SetActive(true);
    }

    public void DeactivatePokemonInterfaceElements()
    {
        pokemonInterfaceElements.SetActive(false);
    }

    public void AddPokemonToGame()
    {
        if (pokemonDropdown.value != 0)
        {
            string pokemonName = pokemonDropdown.captionText.text;

            foreach (Pokemon pokemon in pageController.appController.pokemon)
            {
                if (pokemon.name == pokemonName)
                {
                    if (!availablePokemon.Contains(pokemon))
                    {
                        availablePokemon.Add(pokemon);

                        //instantiate a new text box and button pair that displays the console in the panel with the button removing it from the list if pressed
                        GameObject pokemonobject = Instantiate(item, pokemonNameGridController.transform);
                        Text pokemonNameTextBox = pokemonobject.GetComponentInChildren<Text>();
                        pokemonNameTextBox.text = pokemonDropdown.captionText.text;

                        GameObject buttonGameObject = Instantiate(removeButton, pokemonRemoverGridController.transform);

                        RemoveScript buttonScript = buttonGameObject.GetComponent<RemoveScript>();
                        buttonScript.itemName = pokemonDropdown.captionText.text;
                        buttonScript.SetItemToRemove(pokemonobject);

                        Button button = buttonGameObject.GetComponent<Button>();
                        button.onClick.AddListener(delegate () { buttonScript.DeleteElementFromList(RemovePokemonFromGame); });
                    }

                    break;
                }
            }
        }
    }

    public void RemovePokemonFromGame(string item)
    {
        for (int i = 0; i < availablePokemon.Count; i++)
        {
            if (availablePokemon[i].name == item)
            {
                availablePokemon.RemoveAt(i);
                break;
            }
        }
    }
    #endregion

    #region Shiny Hunting Method Interface
    public void ActivateMethodInterfaceElements()
    {
        methodInterfaceElements.SetActive(true);
    }

    public void DeactivateMethodInterfaceElements()
    {
        methodInterfaceElements.SetActive(false);
    }

    public void AddShinyHuntingMethodToGame()
    {
        if (methodDropdown.value != 0)
        {
            string methodName = methodDropdown.captionText.text;

            foreach (ShinyMethod method in pageController.appController.methods)
            {
                if (method.methodName == methodName)
                {
                    if (!availableMethods.Contains(method))
                    {
                        availableMethods.Add(method);

                        //instantiate a new text box and button pair that displays the console in the panel with the button removing it from the list if pressed
                        GameObject methodObject = Instantiate(item, methodNameGridController.transform);
                        Text methodNameTextBox = methodObject.GetComponentInChildren<Text>();
                        methodNameTextBox.text = methodDropdown.captionText.text;

                        GameObject buttonGameObject = Instantiate(removeButton, methodRemoverGridController.transform);

                        RemoveScript buttonScript = buttonGameObject.GetComponent<RemoveScript>();
                        buttonScript.itemName = methodDropdown.captionText.text;
                        buttonScript.SetItemToRemove(methodObject);

                        Button button = buttonGameObject.GetComponent<Button>();
                        button.onClick.AddListener(delegate () { buttonScript.DeleteElementFromList(RemoveShinyHuntingMethodFromGame); });
                    }

                    break;
                }
            }
        }
    }

    public void RemoveShinyHuntingMethodFromGame(string item)
    {
        for (int i = 0; i < availableMethods.Count; i++)
        {
            if (availableMethods[i].methodName == item)
            {
                availableMethods.RemoveAt(i);
                break;
            }
        }
    }
    #endregion

    #region location interface
    public void ActivateLocationInterfaceElements()
    {
        locationInterfaceElements.SetActive(true);
    }

    public void DeactivateLocationInterfaceElements()
    {
        locationInterfaceElements.SetActive(false);
    }

    public void AddNamedLocationToGame()
    {
        string locationName = namedLocationsInputField.textComponent.text;

        if (!namedLocations.Contains(locationName))
        {
            namedLocations.Add(locationName);

            //instantiate a new text box and button pair that displays the location in the panel with the button removing it from the list if pressed
            GameObject location = Instantiate(item, namedLocationGridController.transform);
            Text locationText = location.GetComponentInChildren<Text>();
            locationText.text = locationName;

            GameObject buttonGameObject = Instantiate(removeButton, namedLocationRemoverGridController.transform);

            RemoveScript buttonScript = buttonGameObject.GetComponent<RemoveScript>();
            buttonScript.itemName = locationName;
            buttonScript.SetItemToRemove(location);

            Button button = buttonGameObject.GetComponent<Button>();
            button.onClick.AddListener(delegate () { buttonScript.DeleteElementFromList(RemoveNamedLocationFromGame); });
        }
    }

    public void RemoveNamedLocationFromGame(string item)
    {
        for (int i = 0; i < namedLocations.Count; i++)
        {
            if (namedLocations[i] == item)
            {
                namedLocations.RemoveAt(i);
                break;
            }
        }
    }

    public void CreateRouteNames()
    {
        int lowestRouteNumber = 0;
        int highestRouteNumber = 0;

        if (lowRouteNumberInputField.textComponent.text != string.Empty)
        {
            lowestRouteNumber = int.Parse(lowRouteNumberInputField.textComponent.text);
        }

        if (highRouteNumberInputField.textComponent.text != string.Empty)
        {
            highestRouteNumber = int.Parse(highRouteNumberInputField.textComponent.text);
        }

        if (lowestRouteNumber <= 0 | highestRouteNumber <= 0)
        {
            Debug.LogError("ERROR: CANNOT HAVE A ROUTE NUMBER WITH A NEGATIVE VALUE OR A VALUE OF 0.");
        }
        else
        {
            if (lowestRouteNumber >= highestRouteNumber)
            {
                int switchNumber = highestRouteNumber;

                lowestRouteNumber = highestRouteNumber;
                highestRouteNumber = switchNumber;
            }

            for (int i = lowestRouteNumber; i <= highestRouteNumber; i++)
            {
                routeNames.Add("Route " + i);
            }
        }
    }
    #endregion

    public void CreateGameData()
    {
        Game game = new Game();

        Debug.Log(nameInputField.textComponent.text);
        game.name = nameInputField.textComponent.text;

        List<string> locations = new List<string>();

        CreateRouteNames();
        locations.AddRange(routeNames);
        locations.AddRange(namedLocations);

        //game.playableConsolesList = consoles;

        //if (game.playableConsolesList.Contains("Gameboy Advanced") | game.playableConsolesList.Contains("Nintendo DS") | game.playableConsolesList.Contains("Virtual Console (3DS)"))
        //{
        //    game.baseShinyOdds = 1f / 8192f;
        //}
        //else
        //{
        //    game.baseShinyOdds = 1f / 4096f;
        //}

        //game.locations = locations;

        //game.shinyCharmAvailable = shinyCharmToggle.isOn;

        //game.availablePokemonList = availablePokemon;

        //game.availableMethodsList = availableMethods;

        game.Save();

        //foreach (Pokemon pokemon in availablePokemon)
        //{
        //    if (!pokemon.availableGames.Contains(game))
        //    {
        //        pokemon.availableGames.Add(game);
        //        pokemon.Save();
        //    }
        //}

        foreach (ShinyMethod method in availableMethods)
        {

            method.Save();

        }

        pageController.appController.UpdateData("All");
    }
}
