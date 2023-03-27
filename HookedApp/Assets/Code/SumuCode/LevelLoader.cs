using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private int finalPoints;
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

    // when the game is about to end
    // set the final score here
    public void SetFinalPoints(int points)
    {
        finalPoints = points;
    }

    // when the ending scene is loaded
    // use this to show the score
    public int GetFinalPoints()
    {
        return finalPoints;
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
