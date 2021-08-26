using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShinyHuntMenuPage : MonoBehaviour
{
    public GameObject currentHuntsButton;
    public GameObject completedHuntsButton;

    void Start()
    {
        if (Directory.GetFiles(Application.dataPath + "/Resources/Shiny Hunts/").Length == 0)
        {
            currentHuntsButton.SetActive(false);
        }
        else
        {
            currentHuntsButton.SetActive(true);
        }


        if (Directory.GetFiles(Application.dataPath + "/Resources/Completed Shiny Hunts/").Length == 0)
        {
            completedHuntsButton.SetActive(false);
        }
        else
        {
            completedHuntsButton.SetActive(true);
        }
    }
}
