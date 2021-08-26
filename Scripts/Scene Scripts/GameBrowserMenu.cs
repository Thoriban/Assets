using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameBrowserMenu : MonoBehaviour
{
    public GameObject buttonGridLayout;
    public GameObject buttonPrefab;

    Dictionary<string, Game> games;

    void Start()
    {
        GetGames();

        foreach (KeyValuePair<string, Game> game in games)
        {
            CreateGameButton(game);
        }
    }

    void GetGames()
    {
        games = new Dictionary<string, Game>();
        string[] gamePaths = Directory.GetFiles(Application.dataPath + "/Resources/Game/");

        foreach (string path in gamePaths)
        {
            if (!path.EndsWith(".meta"))
            {
                Game game = DataControl.LoadGame(path);

                string gameName = path.Replace(Application.dataPath + "/Resources/Game/", "");
                gameName = gameName.Replace(".dat", "");
                games.Add(gameName, game);
            }
        }
    }

    GameObject CreateGameButton(KeyValuePair<string, Game> game)
    {
        GameObject button = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        button.name = game.Key;
        button.transform.parent = buttonGridLayout.transform;

        GameButtonScript buttonScript = button.GetComponent<GameButtonScript>();
        buttonScript.gameName = game.Key;

        Text buttonText = button.GetComponentInChildren<Text>();
        buttonText.gameObject.transform.name = game.Key + " Text Box";

        buttonText.text = "Pokémon " +  game.Value.name;

        return button;
    }
}
