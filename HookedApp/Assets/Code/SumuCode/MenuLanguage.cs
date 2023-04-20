using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuLanguage : MonoBehaviour
{
    [SerializeField] private TMP_Text start, load, options, quit;
    [SerializeField] private Button loadButton;

    // Start is called before the first frame update
    void Start()
    {
        // Checks the language
        bool finnish = (PlayerPrefs.GetInt("Language") == 0);
        ChangeLanguage(finnish);

        // Makes the load button un-interactable if there is no save to load
        if(PlayerPrefs.GetInt("Save exists") != 1)
        {
            loadButton.interactable = false;
        }
        else
        {
            loadButton.interactable = true;
        }
    }

    void ChangeLanguage(bool finnish)
    {
        if (finnish)
        {
            // Changes all menu texts to finnish
            start.text = "Aloita peli";
            load.text = "Lataa peli";
            options.text = "Asetukset";
            quit.text = "Lopeta peli";
        }
        else
        {
            // Changes all menu texts to english
            start.text = "Start game";
            load.text = "Load game";
            options.text = "Options";
            quit.text = "Exit game";
        }
    }
}
