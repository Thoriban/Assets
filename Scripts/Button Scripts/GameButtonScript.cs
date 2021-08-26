using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtonScript : MonoBehaviour
{
    AppController appController;

    public string gameName;

    void Start()
    {
        appController = GameObject.Find("AppController").GetComponent<AppController>();
    }

    public void OpenGame()
    {
        appController.gameName = gameName;

        SceneManager.LoadScene("View Game Page");
    }
}
