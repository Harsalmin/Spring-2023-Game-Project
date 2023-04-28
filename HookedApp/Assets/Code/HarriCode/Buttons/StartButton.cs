using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    /// <summary>
    /// Start a new game with preferred language. Set stats of game to defaults, and loads the Game scene.
    /// </summary>
    public void StartGame()
    {
        if (PlayerPrefs.GetInt("Language") == 0)
            Stats.language = "fi";
        else
            Stats.language = "en";

        Stats.SetNewGame(true);
        GameManager.LoadScene("Game");
    }

    /// <summary>
    /// Loads a previously saved game, with saved star points, contacts and events.
    /// </summary>
    public void LoadGame()
    {
        Stats.SetNewGame(false);
        GameManager.LoadScene("Game");
    }
}
