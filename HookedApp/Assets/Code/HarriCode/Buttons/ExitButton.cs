using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    /// <summary>
    /// Calls the QuitGame method from GameManager, when ExitButton is pressed.
    /// </summary>
    public void QuitGame()
    {
        GameManager.QuitGame();
    }
}
