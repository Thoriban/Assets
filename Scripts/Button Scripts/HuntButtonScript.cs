using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HuntButtonScript : MonoBehaviour
{
    AppController appController;

    public string huntName;
    public string sceneName;

    void Start()
    {
        appController = GameObject.Find("AppController").GetComponent<AppController>();
    }

    public void OpenHunt()
    {
        appController.huntId = huntName;

        SceneManager.LoadScene("Hunt Page");
    }
}
