using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsLanguageControl : MonoBehaviour
{
    [SerializeField] private TMP_Text language, volume, backToMenu;
    [SerializeField] private TMP_Dropdown languageOptions;

    // Start is called before the first frame update
    void Start()
    {
        bool finnish = PlayerPrefs.GetInt("Language") == 0;
        ChangeLanguage(finnish);

        if (finnish)
            languageOptions.value = 0;
        else
            languageOptions.value = 1;
        languageOptions.onValueChanged.AddListener(delegate { LanguageChanged(languageOptions); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeLanguage(bool finnish)
    {
        if (finnish)
        {
            language.text = "Kieli";
            volume.text = "Äänenvoimakkuus";
            backToMenu.text = "Takaisin päävalikkoon";
        }
        else
        {
            language.text = "Language";
            volume.text = "Volume";
            backToMenu.text = "Back to main menu";
        }
    }

    void LanguageChanged(TMP_Dropdown dropdown)
    {
        PlayerPrefs.SetInt("Language", dropdown.value);
        if (dropdown.value == 0)
            ChangeLanguage(true);
        else
            ChangeLanguage(false);
    }
}
