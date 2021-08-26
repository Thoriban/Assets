using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPage : MonoBehaviour
{
    public AppController appController;
    public Dropdown dataTypeDropdown;

    public CreateGameDataController gameElements;
    public CreatePokemonDataController pokemonElements;
    public CreateMethodDataController methodElements;
    public GameObject methodDescriptionPanel;

    void Start()
    {
        gameElements = GetComponent<CreateGameDataController>();
        pokemonElements = GetComponent<CreatePokemonDataController>();
        methodElements = GetComponent<CreateMethodDataController>();

        FindGUIElements();

        Setup();
    }

    void FindGUIElements()
    {
        dataTypeDropdown = GameObject.Find("Data Type Dropdown").GetComponent<Dropdown>();
    }

    void Setup()
    {
        dataTypeDropdown.value = 0;
    }

    public void PopulateDropdown(string dataType, Dropdown targetDropdown)
    {
        List<string> options = new List<string>();

        switch (dataType)
        {
            case "Game":
                options.Add("<i> - select game -</i>");

                foreach (Game game in appController.games)
                {
                    options.Add(game.name);
                }
                break;
            case "Pokemon":
                options.Add("<i> - select pokemon -</i>");

                foreach (Pokemon pokemon in appController.pokemon)
                {
                    options.Add(pokemon.name);
                }
                break;
            case "Method":
                options.Add("<i> - select shiny hunting method -</i>");

                foreach (ShinyMethod method in appController.methods)
                {
                    options.Add(method.methodName);
                }
                break;
        }

        targetDropdown.ClearOptions();
        targetDropdown.AddOptions(options);
    }

    public void ChangeDataType()
    {
        gameElements.DeactivateElements();
        pokemonElements.DeactivateElements();
        methodElements.DeactivateElements();

        switch (dataTypeDropdown.value)
        {
            case 1:
                gameElements.ActivateElements(); 
                gameElements.PopulateDropdowns();
                break;
            case 2: 
                pokemonElements.ActivateElements(); 
                pokemonElements.PopulateDropdowns();
                break;
            case 3: 
                methodElements.ActivateUIElements(); 
                methodElements.PopulateDropdowns();
                break;
            default: 
                break;
        }
    }

    public void OpenMethodDescription()
    {
        methodElements.LoadDescription();
    }

    public void SaveMethodDescription()
    {
        methodElements.SaveDescription();
    }

    public void CancelChangeToMethodDescrtiption()
    {
        methodElements.CancelChangesToDescription();
    }
}
