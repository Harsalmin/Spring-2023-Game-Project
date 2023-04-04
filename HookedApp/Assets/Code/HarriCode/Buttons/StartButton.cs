using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void StartGame()
    {
        if (PlayerPrefs.GetInt("Language") == 0)
            Stats.language = "fi";
        else
            Stats.language = "en";

        Stats.SetNewGame(true);
        GameManager.LoadScene("Game");
    }

    public void LoadGame()
    {
        Stats.SetNewGame(false);
        GameManager.LoadScene("Game");
    }
}
