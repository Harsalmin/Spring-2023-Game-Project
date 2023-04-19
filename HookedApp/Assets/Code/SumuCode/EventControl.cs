using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventControl : MonoBehaviour
{
    const string fileName = "events";
    const string logName = "logs";
    List<EventInfo> allEvents = new List<EventInfo>();
    List<EventInfo> unlockedEvents = new List<EventInfo>();
    List<LogEntry> allEntries = new List<LogEntry>();
    [SerializeField] private Transform eventWindow, eventApp;
    [SerializeField] private TMP_Text logTexts;
    List<GameObject> eventButtons = new List<GameObject>();
    int currentEvent;
    Control control;
    LanguageControl language;

    [SerializeField] private Image fade;
    [SerializeField] Animator popupAnimator;
    [SerializeField] Button yesButton, noButton;

    // Start is called before the first frame update
    void Awake()
    {
        control = GetComponent<Control>();
        language = GetComponent<LanguageControl>();
        LoadFromFile();
        LoadLogs();

        foreach (Transform child in eventApp)
        {
            eventButtons.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
        eventWindow.parent.gameObject.SetActive(false);
    }

    // Starts the next step in the game depending on the choice made
    public void CheckWhatsNext()
    {
        EventInfo e = GetEventById(currentEvent);

        switch(currentEvent)
        {
            case 1:
                control.StartConversation("Eila-mummo", 1);
                if (e.Went)
                {
                    control.StartConversation("Marko", 7);
                }
                else
                {
                    control.StartConversation("Marko", 13);
                }
                break;

            case 2:
                control.StartConversation("Eila-mummo", 1);
                break;

            case 3:
                if(e.Went)
                {
                    control.StartConversation("Marko", 18);
                }
                else
                {
                    control.StartConversation("Marko", 22);
                }
                break;

            case 4:
                if(e.Went)
                {
                    control.StartConversation("Joni", 14);
                }
                else
                {
                    control.StartConversation("Joni", 22);
                }
                break;

            case 5:
                break;

            case 6:
                if(e.Went)
                {
                    control.StartConversation("Mira", 20);
                }
                else
                {

                }
                break;

            case 7:
                break;

            case 8:
                if(e.Went)
                {
                    control.StartConversation("Mira", 30);
                }
                else
                {
                    control.StartConversation("Mira", 27);
                }
                break;

            case 9:
                break;

            case 10:
                break;

            case 11:
                if(e.Went)
                {
                    control.StartConversation("Raimo", 1);
                }
                else
                {
                    control.GameEnds("Huono loppu", "Arvosta mummoasi, julmuri");
                }
                break;
        }
    }

    // Agreed to go to an event
    public void YesToEvent()
    {
        foreach(LogEntry log in allEntries)
        {
            if(log.EventId == currentEvent && log.WentToEvent)
            {
                logTexts.text += log.LogText + "\n";
                EventInfo e = GetEventById(currentEvent);
                e.Answered = true;
                e.Went = true;

                // change the popup text depending on the language
                if (Stats.language == "fi")
                    language.ChangeEventPopupText("P‰‰tit menn‰ tapahtumaan: " + e.Name);
                else
                    language.ChangeEventPopupText("You decided to go to an event: " + e.Name);
            }
        }

        // make it unable to attend to any events on the same day
        EventInfo current = GetEventById(currentEvent);
        foreach(EventInfo e in allEvents)
        {
            if (e.Date == current.Date)
            {
                e.Answered = true;
            }
        }

        FadePopup(true);
    }

    // Did not go to an event
    public void NoToEvent()
    {
        foreach (LogEntry log in allEntries)
        {
            if (log.EventId == currentEvent && !log.WentToEvent)
            {
                logTexts.text += log.LogText + "\n";
                EventInfo e = GetEventById(currentEvent);
                e.Answered = true;

                // change the popup text depending on the language
                if (Stats.language == "fi")
                    language.ChangeEventPopupText("P‰‰tit j‰tt‰‰ menem‰tt‰ tapahtumaan: " + e.Name);
                else
                    language.ChangeEventPopupText("You decided to skip an event: " + e.Name);
            }
        }
        FadePopup(true);
    }

    // updates event info and opens the event screen
    public void ChangeEventInfo(int eventId)
    {
        currentEvent = eventId;
        control.ToggleApp("Events");
        eventWindow.parent.gameObject.SetActive(true);
        EventInfo eventInvite = GetEventById(eventId);
        eventWindow.Find("Title").GetComponent<TMP_Text>().text = eventInvite.Name;
        Sprite img = Resources.Load<Sprite>("Art/Events/" + eventInvite.ImageName);
        eventWindow.Find("Image").GetComponent<Image>().sprite = img;
        // eventWindow.Find("Date").GetComponent<TMP_Text>().text = eventInvite.Date;
        eventWindow.Find("Description").GetComponent<TMP_Text>().text = eventInvite.Description;

        // makes buttons non-interactable if the event has already been answered to 
        yesButton.interactable = !eventInvite.Answered;
        noButton.interactable = !eventInvite.Answered;
    }

    // closes the event screen
    public void CloseEventScreen()
    {
        control.ToggleApp("Events");
        eventWindow.parent.gameObject.SetActive(false);
    }

    // activates the event button
    public void InvitedToEvent(int id)
    {
        EventInfo eventInvite = GetEventById(id);
        eventInvite.Unlocked = true;
        unlockedEvents.Add(eventInvite);

        foreach (GameObject btn in eventButtons)
        {
            if (btn.name == "Event_" + id)
            {
                btn.SetActive(true);
            }
        }
    }

    // returns event by it's id
    EventInfo GetEventById(int id)
    {
        foreach(EventInfo e in allEvents)
        {
            if(e.Id == id)
            {
                return e;
            }
        }
        return null;
    }

    // loads all event info from a text file
    void LoadFromFile()
    {
        TextAsset textData = Resources.Load("Story/" + Stats.language + "/" + fileName.ToLower()) as TextAsset;
        string txt = textData.text;
        var lines = txt.Split("\n");
        foreach (var line in lines)
        {
            string[] parts = line.Split("\t");
            EventInfo e = new EventInfo();
            e.Id = int.Parse(parts[0]);
            e.Name = parts[1];
            e.ImageName = parts[2];
            e.Description = parts[3];
            e.Date = parts[4];
            e.Approval = int.Parse(parts[5]);
            e.Answered = false;
            e.Went = false;
            e.Unlocked = false;
            allEvents.Add(e);
        }
    }

    void LoadLogs()
    {
        TextAsset textData = Resources.Load("Story/" + Stats.language + "/" + logName.ToLower()) as TextAsset;
        string txt = textData.text;
        var lines = txt.Split("\n");
        foreach (var line in lines)
        {
            string[] parts = line.Split("\t");
            LogEntry log = new LogEntry();
            log.EventId = int.Parse(parts[0]);
            if (parts[1].StartsWith("Y"))
                log.WentToEvent = true;
            else
                log.WentToEvent = false;
            log.LogText = parts[2];
            allEntries.Add(log);
        }
    }

    public void FadePopup(bool fadeIn)
    {
        foreach (GameObject btn in eventButtons)
        {
            if (btn.name == "Event_" + currentEvent)
            {
                btn.GetComponent<Image>().color = Color.gray;
            }
        }

        if (fadeIn)
        {
            StartCoroutine(FadeInPopUp());
        }
        else
        {
            CheckWhatsNext();
            StartCoroutine(FadeOutPopUp());
        }
    }

    public IEnumerator FadeInPopUp()
    {
        float timer = 0;
        float duration = 1;
        while (timer <= duration)
        {
            timer += Time.deltaTime * 2;
            fade.color = new Color(0, 0, 0, timer);
            yield return null;
        }
        fade.color = new Color(0, 0, 0, 1);
        CloseEventScreen();
        eventWindow.parent.gameObject.SetActive(false);
        control.ToggleApp("Logs");
        popupAnimator.SetTrigger("show");
    }

    public IEnumerator FadeOutPopUp()
    {
        float timer = 0;
        float duration = 1;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            fade.color = new Color(0, 0, 0, 1 - timer);
            yield return null;
        }
        fade.color = new Color(0, 0, 0, 0);
    }

    public List<EventInfo> GetUnlockedEvents()
    {
        return unlockedEvents;
    }

    public void SetUnlockedEvents(List<EventInfo> events)
    {
        unlockedEvents = events;
        foreach(EventInfo e in unlockedEvents)
        {
            EventInfo eventEntry = GetEventById(e.Id);
            eventEntry.Went = e.Went;
            eventEntry.Answered = e.Answered;

            foreach (GameObject btn in eventButtons)
            {
                if (btn.name == "Event_" + e.Id)
                {
                    btn.SetActive(true);
                    if (e.Answered)
                        btn.GetComponent<Image>().color = Color.gray;
                        btn.GetComponent<Image>().color = Color.gray;
                }
            }
        }
    }

    public string GetLogs()
    {
        return logTexts.text;
    }

    public void SetLogs(string loadedLogs)
    {
        logTexts.text = loadedLogs;
    }
}

[System.Serializable]
public class EventInfo
{
    public int Id;
    public string Name;
    public string ImageName;
    public string Description;
    public string Date;
    public int Approval;
    [HideInInspector] public bool Answered;
    [HideInInspector] public bool Went;
    [HideInInspector] public bool Unlocked;
}

public class LogEntry
{
    public int EventId;
    public bool WentToEvent;
    public string LogText;
}