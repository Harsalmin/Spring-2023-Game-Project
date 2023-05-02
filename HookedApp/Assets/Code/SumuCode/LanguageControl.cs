using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageControl : MonoBehaviour
{
    [SerializeField] TMP_Text chat, events, logs, settings, logsTitle;
    [SerializeField] TMP_Text settingsTitle, settingsDesc, save, load, exit;
    [SerializeField] TMP_Text eventYes, eventNo, eventPopup;
    [SerializeField] TMP_Text[] eventTitles;
    [SerializeField] TMP_Text grandmaConvo, grandmaButton, doormanConvo, doormanButton, gymManConvo, gymManButton;

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
        settingsDesc.text = "Täällä voit tallentaa ja ladata pelin, tai palata päävalikkoon";
        save.text = "Tallenna peli";
        load.text = "Lataa peli";
        exit.text = "Takaisin päävalikkoon";
        eventYes.text = "Osallistun";
        eventNo.text = "En osallistu";

        eventTitles[0].text = "Baari-ilta";
        eventTitles[1].text = "Työmessut";
        eventTitles[2].text = "Klubikeikka";
        eventTitles[3].text = "DJ-keikka";
        eventTitles[4].text = "Rekrymessut";
        eventTitles[5].text = "DJ-keikka";
        eventTitles[6].text = "Työmessut";
        eventTitles[7].text = "Pelikahvila";
        eventTitles[8].text = "Pelibaari";
        eventTitles[9].text = "Kahvila";
        eventTitles[10].text = "Baari-ilta";
        eventTitles[11].text = "Kuntosali";
        eventTitles[12].text = "Sulkapallo";
        eventTitles[13].text = "Nuorisotalo";
        eventTitles[14].text = "Opiskelijatapahtuma";
        eventTitles[15].text = "Pelimessut";

        grandmaConvo.text = "Eila-mummo";
        grandmaButton.text = "Eila-mummo";
        doormanConvo.text = "Joni, portsari";
        doormanButton.text = "Joni, portsari";
        gymManButton.text = "Kari salilta";
        gymManConvo.text = "Kari salilta";
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

        eventTitles[0].text = "Bar night";
        eventTitles[1].text = "Work fair";
        eventTitles[2].text = "Club gig";
        eventTitles[3].text = "DJ gig";
        eventTitles[4].text = "Recruiting fair";
        eventTitles[5].text = "DJ gig";
        eventTitles[6].text = "Work fair";
        eventTitles[7].text = "Gaming café";
        eventTitles[8].text = "Game bar";
        eventTitles[9].text = "Café";
        eventTitles[10].text = "Bar night";
        eventTitles[11].text = "Gym";
        eventTitles[12].text = "Badminton";
        eventTitles[13].text = "Youth centre";
        eventTitles[14].text = "Student event";
        eventTitles[15].text = "Game fair";

        grandmaConvo.text = "Grandma Eila";
        grandmaButton.text = "Grandma Eila";
        doormanConvo.text = "Joni, doorman";
        doormanButton.text = "Joni, doorman";
        gymManButton.text = "Kari from the gym";
        gymManConvo.text = "Kari from the gym";
    }
}
