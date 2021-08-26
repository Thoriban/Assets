using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPage : MonoBehaviour
{
    public void ButtonPress(string sceneName)
    {
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName); 
        }
    }
}
