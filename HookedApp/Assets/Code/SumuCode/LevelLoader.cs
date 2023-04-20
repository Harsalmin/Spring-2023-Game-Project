using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // private int finalPoints;
    private static LevelLoader loader;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (loader == null)
        {
            loader = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
