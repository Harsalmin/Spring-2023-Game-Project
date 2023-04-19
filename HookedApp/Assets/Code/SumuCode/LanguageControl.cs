using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageControl : MonoBehaviour
{
    [SerializeField] TMP_Text chat, events, logs, settings, logsTitle;
    [SerializeField] TMP_Text settingsTitle, settingsDesc, save, load, exit;
    [SerializeField] TMP_Text eventYes, eventNo, eventPopup;

    // Start is called before the first frame update
    void Start()
    {
        if(Stats.language == "fi")
        {
            Finnish();
        }
        else
        {
            English();
        }
    }

    public void ChangeEventPopupText(string text)
    {
        eventPopup.text = text;
    }

    public void Finnish()
    {
        chat.text = "Keskustelut";
        events.text = "Tapahtumat";
        logs.text = "Loki";
        logsTitle.text = "Loki";
        settings.text = "Asetukset";
        settingsTitle.text = "Asetukset";
        settingsDesc.text = "T‰‰ll‰ voit tallentaa ja ladata pelin, tai palata p‰‰valikkoon";
        save.text = "Tallenna peli";
        load.text = "Lataa peli";
        exit.text = "Takaisin p‰‰valikkoon";
        eventYes.text = "Osallistun";
        eventNo.text = "En osallistu";
    }

    public void English()
    {
        chat.text = "Chat";
        events.text = "Events";
        logs.text = "Logs";
        logsTitle.text = "Logs";
        settings.text = "Settings";
        settingsTitle.text = "Settings";
        settingsDesc.text = "Here you can save or load the game, or go back to main menu";
        save.text = "Save game";
        load.text = "Load game";
        exit.text = "Back to main menu";
        eventYes.text = "Attending";
        eventNo.text = "Not attending";
    }
}
