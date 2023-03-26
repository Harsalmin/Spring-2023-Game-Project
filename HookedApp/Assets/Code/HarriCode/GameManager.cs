using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void LoadScene(string sceneName)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneName);
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
