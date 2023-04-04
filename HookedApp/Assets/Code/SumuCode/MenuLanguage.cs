using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuLanguage : MonoBehaviour
{
    [SerializeField] private TMP_Text start, load, options, quit;

    // Start is called before the first frame update
    void Start()
    {
        bool finnish = (PlayerPrefs.GetInt("Language") == 0);
        ChangeLanguage(finnish);
    }

    void ChangeLanguage(bool finnish)
    {
        if (finnish)
        {
            start.text = "Aloita peli";
            load.text = "Lataa peli";
            options.text = "Asetukset";
            quit.text = "Lopeta peli";
        }
        else
        {
            start.text = "Start game";
            load.text = "Load game";
            options.text = "Options";
            quit.text = "Exit game";
        }
    }
}
