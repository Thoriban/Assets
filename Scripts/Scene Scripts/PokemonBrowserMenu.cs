using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonBrowserMenu : MonoBehaviour
{
    Dropdown generationSelector;

    List<string> generationOptions;

    void Start()
    {
        generationSelector = GameObject.Find("Generation Selection").GetComponent<Dropdown>();

        generationOptions = new List<string>();
        generationOptions.Add("Select Generation");
        generationOptions.Add("Gen 1 (#001 - #151)");
        generationOptions.Add("Gen 2 (#152 - #251)");
        generationOptions.Add("Gen 3 (#252 - #386)");
        generationOptions.Add("Gen 4 (#387 - #493)");
        generationOptions.Add("Gen 5 (#494 - #649)");
        generationOptions.Add("Gen 6 (#650 - #721)");
        generationOptions.Add("Gen 7 (#722 - #809)");
        generationOptions.Add("Gen 8 (#810 - #898)");
    }

    public void ChangeGeneration()
    {
        switch (generationSelector.value)
        {
            case 0:

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
