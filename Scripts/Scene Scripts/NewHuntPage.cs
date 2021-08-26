/*########## TO DO ############
     *
     * ----HUNT WINDOW----
     * - Add a hunt window to the buttons that allows the user to select the game and then the method in which they hunt the pokemon
     * - The window should display the best odds for obtaining the pokemon along with the game that method can be performed in.
     *
     * ----NATIONAL VARIANT DEX----
     * - Add a national variant pokedex that includes all variant forms (original, alolan, galarian, east coast, etc)
     * 
     * ----Shiny Images-----
     * - Change images loaded to the shiny variants when the images are created
     * 
     * ############################
    */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NewHuntPage : MonoBehaviour
{
    PokeDex dex;

    ShinyHunt hunt;

    InputField searchBar;

    List<GameObject> pokemon;

    public GameObject pokemonButton;
    public GameObject blankObject;
    public GameObject gridLayout;

    public NewHuntPopupWindow newHuntPopupWindow;

    string searchText;
    
    void Start()
    {
        searchBar = GameObject.Find("Search Bar").GetComponent<InputField>();

        searchText = searchBar.text;
        pokemon = new List<GameObject>();
        string filePath = Application.dataPath + "/Resources/Poke Dex/National Dex.dat"; //ADKJ - Change to National Variant Dex when created
        dex = DataControl.LoadPokeDex(filePath);
        UpdateList();

        newHuntPopupWindow = GameObject.Find("Popup Window").GetComponent<NewHuntPopupWindow>();

        newHuntPopupWindow.SetActive(false);
    }

    void Update()
    {
        if (searchBar.text != searchText)
        {
            searchText = searchBar.text;
            UpdateList();
        }
    }

    public void CreateHuntWindow()
    {
        Debug.Log("Opening Hunt Window");
    }

    #region Generate Pokemon Images
    void UpdateList()
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
            if (searchText != "")
            {
                string filter = searchText.ToLower();
                string pokemonName = dex.pokemonNames[i].ToLower();

                if (pokemonName.Contains(filter) | pokemonName.StartsWith(filter) | pokemonName.EndsWith(filter))
                {
                    InstantiatePokemonImages(dex.pokemonNames[i], dex.dexNumbers[i]);
                }
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

    void InstantiatePokemonImages(string pokemonName, int dexNumber)
    {
        GameObject icon;

        icon = Instantiate(pokemonButton, new Vector3(0, 0, 0), Quaternion.identity);
        icon.transform.SetParent(gridLayout.transform);
        icon.transform.name = pokemonName + " Button";

        PokemonButtonScript script = icon.GetComponent<PokemonButtonScript>();
        script.dexNumber = dexNumber;
        script.pokemonName = pokemonName;

        LoadArt(icon, Application.dataPath + "/Art Assets/Pokemon/Buttons/Caught/Normal/" + pokemonName + ".png"); //ADKJ - Change to Shiny once images are made

        Text text = icon.GetComponentInChildren<Text>();
        text.gameObject.transform.name = pokemonName + " Text Box";
        pokemonName = pokemonName.Replace("(M)", "♂");
        pokemonName = pokemonName.Replace("Male", "♂");
        pokemonName = pokemonName.Replace("Female", "♀");
        pokemonName = pokemonName.Replace("(F)", "♀");

        string formattedDexNumber = string.Empty;

        if (dexNumber < 10)
        {
            formattedDexNumber = "00" + dexNumber;
        }
        else if (dexNumber < 100)
        {
            formattedDexNumber = "0" + dexNumber;
        }
        else
        {
            formattedDexNumber = "" + dexNumber;
        }

        text.text = "#" + formattedDexNumber + "\n" + pokemonName;

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
    #endregion
}
