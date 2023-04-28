using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Start is called before the first frame update. Make the gameobject this script is attached to undestroyable between sceneloads, 
    /// to retain it's functionality.
    /// </summary>
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Loads a new scene by sceneName. After the new scene is loaded, unloads the previous scene
    /// </summary>
    /// <param name="sceneName">The name of the scene to be loaded</param>
    public static void LoadScene(string sceneName)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneName);
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }

    /// <summary>
    /// Quit the application by calling ready-made Application.Quit() method.
    /// </summary>
    public static void QuitGame()
    {
        Application.Quit();
    }
}
