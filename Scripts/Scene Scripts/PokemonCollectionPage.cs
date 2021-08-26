using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PokemonCollectionPage : MonoBehaviour
{
    AppController appController;
    List<GameObject> pokemon;
    public GameObject pokemonButton;
    public GameObject pokemonIcon;
    public GameObject blankObject;
    public GameObject gridLayout;

    public int collectionType;
    public int generation;

    string pokeDexRegion;
    string dexType;
    string collectionVersion;

    public PokemonCollection collection;
    PokeDex dex;

    /*########## TO DO ############
     *
     * ----POKEMON POPUP WINDOW----
     * Add A popup window when a collection pokemon is tapped.
     * The window should include an image of the pokemon in both normal and shiny variants
     * The window should have toggles to show alternate forms (male/female, alolan, galarian, mega, gigantamax, etc)
     * The window should give information about the pokemon, games its available in, etc
     * The window should include a button to remove the pokemon from the collection.
     * 
     * ----ADD IMPORT----
     * Add the function to import the selected poke dex if it is not present in the data folder.
     * 
     * ############################
    */

    void Start()
    {
        appController = GameObject.Find("AppController").GetComponent<AppController>();

        generation = appController.generation;
        collectionType = appController.collectionType;

        pokemon = new List<GameObject>();

        collection = DataControl.LoadCollection(Application.dataPath + "/Resources/Collections/" + appController.collectionName + ".dat");

        switch (generation)
        {
            case 1:
                pokeDexRegion = "Kanto ";
                break;
            case 2:
                pokeDexRegion = "Johto ";
                break;
            case 3:
                pokeDexRegion = "Sinnoh ";
                break;
            case 4:
                pokeDexRegion = "Hoenn ";
                break;
            case 5:
                pokeDexRegion = "Unova ";
                break;
            case 6:
                pokeDexRegion = "Kalos ";
                break;
            case 7:
                pokeDexRegion = "Alola ";
                break;
            case 8:
                pokeDexRegion = "Galar ";
                break;

            default: //National Dex
                pokeDexRegion = "National ";
                break;
        }

        switch (collectionType)
        {
            case 1: dexType = "Form "; break;
            case 3: dexType = "Form "; break;
            default: break;
        }

        if (collectionType == 2 | collectionType == 3)
        {
            collectionVersion = "Shiny";
        }
        else
        {
            collectionVersion = "Normal";
        }

        string filePath = Application.dataPath + "/Resources/Poke Dex/" + pokeDexRegion + dexType + "Dex.dat";

        //ADKJ - Add if pokeDex Is missing, use import script to import the dex
        dex = DataControl.LoadPokeDex(filePath);

        UpdateCollection();
    }

    public void UpdateCollection()
    {
        if (pokemon.Count != 0)
        {
            int length = pokemon.Count;

            for (int i = 0; i < length; i++)
            {
                Destroy(pokemon[0]);
                pokemon.RemoveAt(0);
            }
        }

        for (int i = 0; i < dex.dexNumbers.Length; i++)
        {
            bool pokemonFound = false;

            foreach (string pokemonName in collection.pokemonNames)
            {
                if (pokemonName == dex.pokemonNames[i])
                {
                    pokemonFound = true;
                    break;
                }
            }

            if (pokemonFound)
            {
                InstantiatePokemonImages(dex.pokemonNames[i], dex.dexNumbers[i], true);
            }
            else
            {
                InstantiatePokemonImages(dex.pokemonNames[i], dex.dexNumbers[i]);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            InstantiateBlankObjects();
        }
    }
    
    void InstantiatePokemonImages(string pokemonName, int dexNumber, bool caught = false)
    {
        GameObject icon;

        if (caught)
        {
            icon = Instantiate(pokemonIcon, new Vector3(0, 0, 0), Quaternion.identity);
            icon.transform.SetParent(gridLayout.transform);
            icon.transform.name = pokemonName + " Icon";

            LoadArt(icon, Application.dataPath + "/Art Assets/Pokemon/Buttons/Caught/" + collectionVersion + "/" + pokemonName + ".png");
        }
        else
        {
            icon = Instantiate(pokemonButton, new Vector3(0, 0, 0), Quaternion.identity);
            icon.transform.SetParent(gridLayout.transform);
            icon.transform.name = pokemonName + " Button";

            PokemonButtonScript script = icon.GetComponent<PokemonButtonScript>();
            script.dexNumber = dexNumber;
            script.pokemonName = pokemonName;

            LoadArt(icon, Application.dataPath + "/Art Assets/Pokemon/Buttons/Missing/" + pokemonName + ".png");
        }

        Text text = icon.GetComponentInChildren<Text>();
        text.gameObject.transform.name = pokemonName + " Text Box";
        pokemonName = pokemonName.Replace("(M)", "♂");
        pokemonName = pokemonName.Replace("Male", "♂");
        pokemonName = pokemonName.Replace("Female", "♀");
        pokemonName = pokemonName.Replace("(F)", "♀");

        text.text = "#" + dexNumber + "\n" + pokemonName;

        pokemon.Add(icon);
    }

    void LoadArt(GameObject icon, string filepath)
    {
        Texture2D spriteTexture = LoadTexture(filepath);

        if (spriteTexture != null)
        {
            Sprite pokemonSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), 100, 0, SpriteMeshType.Tight);

            Image image = icon.GetComponent<Image>();
            image.sprite = pokemonSprite;
        }
    }

    Texture2D LoadTexture(string filePath)
    {
        Texture2D texture2D;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            texture2D = new Texture2D(1080, 1080);

            if (texture2D.LoadImage(fileData))
            {
                return texture2D;
            }
        }

        return null;
    }

    void InstantiateBlankObjects()
    {
        GameObject blankIcon = Instantiate(blankObject, new Vector3(0, 0, 0), Quaternion.identity);
        blankIcon.transform.SetParent(gridLayout.transform);
        pokemon.Add(blankIcon);
    }

    public void ButtonPress(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
