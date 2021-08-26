using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPage : MonoBehaviour
{
    public AppController appController;

    public void StartButton()
    {
        Log.Write("Start Button Pressed");

        appController.UpdateData("All");
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene("Main Page");
    }    
}
