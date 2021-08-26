using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMethodDataController : MonoBehaviour
{
    public Dropdown gamesDropdown;

    public GameObject userInterfaceElements;
    public GameObject methodDescriptionPanel;

    public GameObject removeGameButton;

    public InputField methodNameInputField;
    public InputField descriptionInputField;

    public DataPage pageController;

    List<Game> games;
    List<Pokemon> availablePokemon;

    List<int> requirements;

    List<string> oddsValues;

    public string description;

    public void Start()
    {
        pageController = GetComponent<DataPage>();

        requirements = new List<int>();
        oddsValues = new List<string>();
        games = new List<Game>();
        availablePokemon = new List<Pokemon>();

        FindGUIElements();

        Setup();
    }

    void FindGUIElements()
    {
        userInterfaceElements = GameObject.Find("Shiny Hunting Method Data Elements");
        methodDescriptionPanel = GameObject.Find("Shiny Hunting Method - Description Panel");
        removeGameButton = GameObject.Find("SHM - Remove Game Button");

        gamesDropdown = GameObject.Find("SHM - Game Selection Dropdown").GetComponent<Dropdown>();

        methodNameInputField = GameObject.Find("SHM Name InputField").GetComponent<InputField>();
        descriptionInputField = GameObject.Find("Shiny Hunting Method - Description Input Field").GetComponent<InputField>();
    }

    public void Setup()
    {
        DeactivateElements();
        PopulateDropdowns();
    }

    public void PopulateDropdowns()
    {
        pageController.PopulateDropdown("Game", gamesDropdown);
    }

    public void AddGameToMethod()
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
    }

    public void RemoveGameFromMethod()
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
    }

    public void LoadDescription()
    {
        ActivateDescriptionElements();
        descriptionInputField.SetTextWithoutNotify(description);
    }

    public void SaveDescription()
    {
        description = descriptionInputField.textComponent.text;
        DeactivateDescriptionElements();
    }

    public void CancelChangesToDescription()
    {
        descriptionInputField.SetTextWithoutNotify("");
        DeactivateDescriptionElements();
    }

    public void ActivateUIElements()
    {
        userInterfaceElements.SetActive(true);

        if (games.Count != 0)
        {
            removeGameButton.SetActive(true);
        }
    }

    public void ActivateDescriptionElements()
    {
        methodDescriptionPanel.SetActive(true);
    }

    public void DeactivateElements()
    {
        userInterfaceElements.SetActive(false);
        methodDescriptionPanel.SetActive(false);

        if (games.Count == 0)
        {
            removeGameButton.SetActive(false);
        }
    }

    public void DeactivateUIElements()
    {
        userInterfaceElements.SetActive(false);
    }

    public void DeactivateDescriptionElements()
    {
        methodDescriptionPanel.SetActive(false);
    }

    public void CreateShinyHuntingMethodData()
    {
        ShinyMethod method = new ShinyMethod();

        method.methodName = methodNameInputField.textComponent.text;

        method.description = description;

        method.Save();

        foreach (Game game in games)
        {
            //if (!game.availableMethodsList.Contains(method))
            //{
            //    game.availableMethodsList.Add(method);
            //    game.Save();
            //}
        }

        //foreach (Pokemon pokemon in availablePokemon)
        //{
        //    if (!pokemon.availableMethods.Contains(method))
        //    {
        //        pokemon.availableMethods.Add(method);
        //        pokemon.Save();
        //    }
        //}
    }
}
