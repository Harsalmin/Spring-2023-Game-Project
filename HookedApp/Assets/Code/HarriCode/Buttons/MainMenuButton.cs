using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    /// <summary>
    /// Loads main manu, when a button is pressed, that has this script attached.
    /// </summary>
    public void LoadMainMenu()
    {
        GameManager.LoadScene("StartMenu");
    }
}
