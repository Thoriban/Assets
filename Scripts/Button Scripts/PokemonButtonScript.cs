using UnityEngine;
using UnityEngine.SceneManagement;

public class PokemonButtonScript : MonoBehaviour
{
    string sceneName;
    public string pokemonName;
    public int dexNumber;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    public void ButtonPress()
    {
        switch (sceneName)
        {
            case "Pokemon Collection Page":
                PokemonCollectionPage pokemonCollectionPageScript = GameObject.Find("Main Camera").GetComponent<PokemonCollectionPage>();

                pokemonCollectionPageScript.collection.AddPokemonToCollection(pokemonName, dexNumber);

                pokemonCollectionPageScript.UpdateCollection();

                pokemonCollectionPageScript.collection.Save();
                break;

            case "New Hunt Menu":
                NewHuntPage newHuntPageScript = GameObject.Find("Main Camera").GetComponent<NewHuntPage>();
                newHuntPageScript.newHuntPopupWindow.SetActive(true);
                newHuntPageScript.newHuntPopupWindow.UpdatePokemonInfo(pokemonName);
                newHuntPageScript.newHuntPopupWindow.UpdateRegularFilename(Application.dataPath + "/Art Assets/Pokemon/Buttons/Caught/Normal/" + pokemonName + ".png");
                newHuntPageScript.newHuntPopupWindow.UpdateShinyFilename(Application.dataPath + "/Art Assets/Pokemon/Buttons/Caught/Shiny/" + pokemonName + ".png");
                newHuntPageScript.newHuntPopupWindow.PopulateGames();
                break;
        }
    }
}
