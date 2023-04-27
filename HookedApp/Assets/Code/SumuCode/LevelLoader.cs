using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // private int finalPoints;
    private static LevelLoader loader;

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
