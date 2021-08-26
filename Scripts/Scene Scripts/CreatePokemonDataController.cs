using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreatePokemonDataController : MonoBehaviour
{
    public DataPage pageController;

    public GameObject userInterfaceElements;
    public GameObject removeGameButton;
    public GameObject removeMethodButton;
    public GameObject removeAlternateFormButton;

    public Dropdown gamesDropdown;
    public Dropdown methodDropdown;

    public InputField nameInputField;
    public InputField pokedexNumberInputField;
    public InputField alternateFormsInputField;

    public Text alternateFormsTextBox;

    List<Game> games;
    List<ShinyMethod> availableMethods;

    List<string> availableLocations;
    List<string> alternateForms;

    public void Start()
    {
        pageController = GetComponent<DataPage>();

        games = new List<Game>();
        availableMethods = new List<ShinyMethod>();

        availableLocations = new List<string>();
        alternateForms = new List<string>();

        FindGUIElements();

        Setup();
    }

    void FindGUIElements()
    {
        userInterfaceElements = GameObject.Find("Pokemon Data Elements");
        removeGameButton = GameObject.Find("Pokemon - Remove Game Button");
        removeMethodButton = GameObject.Find("Pokemon - Remove Method Button");
        removeAlternateFormButton = GameObject.Find("Pokmeon - Remove Alternate Form Button");

        gamesDropdown = GameObject.Find("Pokemon - Game selection Dropdown").GetComponent<Dropdown>();
        methodDropdown = GameObject.Find("Pokemon - Method selection Dropdown").GetComponent<Dropdown>();

        nameInputField = GameObject.Find("Pokemon - Name Input Field").GetComponent<InputField>();
        pokedexNumberInputField = GameObject.Find("Pokemon - Pokedex Number Input Field").GetComponent<InputField>();
        alternateFormsInputField = GameObject.Find("Pokemon - Alternate Forms Input Field").GetComponent<InputField>();

        alternateFormsTextBox = GameObject.Find("Pokemon - Alternate Forms Text Box").GetComponent<Text>();
    }

    public void Setup()
    {
        DeactivateElements();
        PopulateDropdowns();
    }

    public void PopulateDropdowns()
    {
        pageController.PopulateDropdown("Game", gamesDropdown);
        pageController.PopulateDropdown("Method", methodDropdown);
    }

    public void ActivateElements()
    {
        userInterfaceElements.SetActive(true);
    }

    public void DeactivateElements()
    {
        userInterfaceElements.SetActive(false);

        if (games.Count == 0)
        {
            removeGameButton.SetActive(false);
        }

        if (availableMethods.Count == 0)
        {
            removeMethodButton.SetActive(false);
        }

        if (alternateForms.Count == 0)
        {
            removeAlternateFormButton.SetActive(false);
        }
    }

    public void AddGameToPokemon()
    {
        if (gamesDropdown.value != 0)
        {
            string gameName = gamesDropdown.captionText.text;

            foreach (Game game in pageController.appController.games)
            {
                if (game.name == gameName)
                {
                    if (!games.Contains(game))
                    {
                        games.Add(game);
                    }

                    break;
                }
            }

            removeGameButton.SetActive(true);
        }

        gamesDropdown.value = 0;
    }

    public void RemoveGameFromPokemon()
    {
        if (gamesDropdown.value != 0)
        {
            if (games.Count != 0)
            {
                string gameName = gamesDropdown.captionText.text;

                for (int i = 0; i < games.Count; i++)
                {
                    if (games[i].name == gameName)
                    {
                        games.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        if (games.Count == 0)
        {
            removeGameButton.SetActive(false);
        }

        gamesDropdown.value = 0;
    }

    public void AddMethodToPokemon()
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
                    }

                    break;
                }
            }

            removeMethodButton.SetActive(true);
        }
        methodDropdown.value = 0;
    }

    public void RemoveMethodFromPokemon()
    {
        if (methodDropdown.value != 0)
        {
            if (availableMethods.Count != 0)
            {
                string methodName = methodDropdown.captionText.text;

                for (int i = 0; i < availableMethods.Count; i++)
                {
                    if (availableMethods[i].methodName == methodName)
                    {
                        availableMethods.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        if (availableMethods.Count == 0)
        {
            removeMethodButton.SetActive(false);
        }

        methodDropdown.value = 0;
    }

    public void AddAlternateFormToPokemon()
    {
        string alternateFormName = alternateFormsInputField.textComponent.text;

        if (!alternateForms.Contains(alternateFormName))
        {
            alternateForms.Add(alternateFormName);
        }

        removeMethodButton.SetActive(true);

        alternateFormsInputField.SetTextWithoutNotify(string.Empty);
    }

    public void RemoveAlternateFormFromPokemon()
    {
        string alternateFormName = alternateFormsInputField.textComponent.text;

        for (int i = 0; i < alternateForms.Count; i++)
        {
            if (alternateForms[i] == alternateFormName)
            {
                alternateForms.RemoveAt(i);
            }
        }

        if (alternateForms.Count == 0)
        {
            removeMethodButton.SetActive(false);
        }

        alternateFormsInputField.SetTextWithoutNotify(string.Empty);
    }

    public void CreatePokemonData()
    {
        Pokemon pokemon = new Pokemon();

        pokemon.name = nameInputField.textComponent.text;

        pokemon.dexNumber = int.Parse(pokedexNumberInputField.textComponent.text);

        //pokemon.availableLocationsValues = availableLocations.ToArray();

        List<string> gameNames = new List<string>();

        foreach (Game game in games)
        {
            gameNames.Add(game.name);
        }

        pokemon.availableGames = gameNames.ToArray();

        List<string> methodNames = new List<string>();
        foreach (ShinyMethod method in availableMethods)
        {
            methodNames.Add(method.methodName);
        }

        pokemon.availableMethods = methodNames.ToArray();

        pokemon.alternateForms = alternateForms.ToArray();

        pokemon.Save();

        foreach (Game game in games)
        {
            //if (!game.availablePokemonList.Contains(pokemon))
            //{
            //    game.availablePokemonList.Add(pokemon);
            //    game.Save();
            //}
        }

        foreach (ShinyMethod method in availableMethods)
        {
            List<string> availablePokemon = method.availablePokemon.ToList();

            if (!availablePokemon.Contains(pokemon.name))
            {
                availablePokemon.Add(pokemon.name);
                method.availablePokemon = availablePokemon.ToArray();
                method.Save();
            }
        }

        pageController.appController.UpdateData("All");
    }
}