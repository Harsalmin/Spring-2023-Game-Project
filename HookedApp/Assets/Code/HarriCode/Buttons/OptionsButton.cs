using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    /// <summary>
    /// Loads OptionsMenu scene by calling the LoadScene method at GameManager.
    /// </summary>
    public void LoadOptions()
    {
        GameManager.LoadScene("OptionsMenu");
    }
}
