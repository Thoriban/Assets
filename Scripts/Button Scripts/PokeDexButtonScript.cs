using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PokeDexButtonScript : MonoBehaviour
{
    AppController appController;
    public int generation;

    void Start()
    {
        appController = GameObject.Find("AppController").GetComponent<AppController>();
    }

    public void ButtonPress()
    {
        appController.generation = generation;
        SceneManager.LoadScene("Pokemon Collection Menu");
    }
}
