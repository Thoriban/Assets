using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewPokemonDataPage : MonoBehaviour
{
    public AppController appController;

    List<string> pokemonDropdownOptions = new List<string>();

    Dropdown selector;

    Text dexNumberTextBox;
    Text infoTextBox;

    void Start()
    {

        appController = GameObject.Find("AppController").GetComponent<AppController>();
        selector = GameObject.Find("Pokemon Selector").GetComponent<Dropdown>();
        dexNumberTextBox = GameObject.Find("Dex Number Text Box").GetComponent<Text>();
        infoTextBox = GameObject.Find("Info Text Box").GetComponent<Text>();

        pokemonDropdownOptions.Add("Select Pokemon");

        foreach (Pokemon pokemon in appController.pokemon)
        {
            pokemonDropdownOptions.Add(pokemon.name);
        }

        selector.ClearOptions();
        selector.AddOptions(pokemonDropdownOptions);
    }


    public void ChangePokemon()
    {
        if (selector.value != 0)
        {
            Pokemon pokemon = DataControl.LoadPokemon(Application.dataPath + "/Resources/Pokemon/" + selector.captionText.text + ".dat");
            string dexNumber = "#";
            string textBoxText = "";

            if (pokemon.dexNumber < 100)
            {
                dexNumber += "0";
                if (pokemon.dexNumber < 10)
                {
                    dexNumber += "0";
                }
            }

            dexNumberTextBox.text = dexNumber + pokemon.dexNumber;

            if (pokemon.availableGames.Length != 0)
            {
                textBoxText += "<b>Available Games:</b>\n";
                foreach (string gameName in pokemon.availableGames)
                {
                    textBoxText += "• " + gameName + "\n";
                }
            }

            textBoxText += "\n<b>Evolution:</b>";

            //if (pokemon.evolutionTree.evolutionsFromNode.Length != 0)
            //{
            //    foreach (EvolutionTreeNode evolution in pokemon.evolutionTree.evolutionsFromNode)
            //    {
            //        Debug.Log(pokemon.evolutionTree.pokemon);
            //        textBoxText += pokemon.evolutionTree.pokemon + " evolves by " + evolution.methodToReachNode + " into " + evolution.pokemon;
                    
            //        if (evolution.evolutionsFromNode.Length != 0)
            //        {
            //            for (int i = 0; i < evolution.evolutionsFromNode.Length; i++)
            //            {
            //                textBoxText += " evolves by " + evolution.evolutionsFromNode[i].methodToReachNode + " into " + evolution.evolutionsFromNode[i].pokemon;

            //                if (i != evolution.evolutionsFromNode.Length - 1)
            //                {
            //                    textBoxText += " or it ";
            //                }
            //            }
            //        }
                    
            //        textBoxText += "\n";
            //    }
            //}
            //else
            //{
            //    textBoxText += pokemon.evolutionTree.pokemon + " is not know to evolve into any pokemon.";
            //}

            infoTextBox.text = textBoxText;
        }
        else
        {
            ResetData();
        }
    }

    void ResetData()
    {
        dexNumberTextBox.text = "";
        infoTextBox.text = "";
    }

    void Update()
    {

    }
}
