using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Event[] allEvents;
    private List<Event> unlockedEvents;
    // Stats stats;
    UIcontrol uiControl;
    GameControl gameControl;
    MessageControl msgControl;

    private void Start()
    {
        uiControl = GetComponent<UIcontrol>();
        gameControl = GetComponent<GameControl>();
        msgControl = GetComponent<MessageControl>();
    }

    public void AnswerYesToInvite(GameObject eventTitle)
    {
        msgControl.NewDay();
        string eventName = eventTitle.GetComponent<TMP_Text>().text;
        // If agreed to go to the event, add approval points
        Event e = GetEventByName(eventName);
        eventName = e.name;
        Stats.AddPoints(e.approval);
        e.answered = true;

        DateFull(e.date);

        // tell that this phase has ended with the player agreeing
        gameControl.EndPhase("Yes:" + eventName);
        uiControl.FadePopup(true);

        // change text depending on the language
        if (Stats.language == "fi")
            uiControl.ChangePopupText("P‰‰tit menn‰ tapahtumaan: " + eventName);
        else
            uiControl.ChangePopupText("You went to an event: " + e.englishName);

        uiControl.DisableEventButtons(true);
    }

    public void AnswerNoToInvite(GameObject eventTitle)
    {
        string eventName = eventTitle.GetComponent<TMP_Text>().text;
        Event e = GetEventByName(eventName);
        eventName = e.name;
        e.answered = true;

        // if the player says no to an event, tell that this phase has ended as such
        gameControl.EndPhase("No:" + eventName);
        uiControl.DisableEventButtons(true);
    }

    // add to list of unlocked events
    public void EventUnlocked(Event e)
    {
        if(unlockedEvents == null)
        {
            unlockedEvents = new List<Event>();
        }

        if(!unlockedEvents.Contains(e))
        {
            unlockedEvents.Add(e);
        }
    }

    // marks all events with the same date as answered
    public void DateFull(string eventDate)
    {
        foreach (Event e in unlockedEvents)
        {
            if (e.date == eventDate)
            {
                e.answered = true;
            }
        }
    }

    // returns an event by name
    public Event GetEventByName(string name)
    {
        foreach(Event e in allEvents)
        {
            if (e.name == name || e.englishName == name)
            {
                return e;
            }
        }
        return null;
    }

    // returns unlocked events for saving purposes
    public List<Event> GetUnlockedEvents()
    {
        if(unlockedEvents == null)
        {
            return null;
        }

        foreach (Event e in unlockedEvents)
            Debug.Log(e.name + " got");

        return unlockedEvents;
    }

    // loads all previously unlocked events
    public void LoadUnlockedEvents(List<Event> eventHistory)
    {
        if(eventHistory != null)
        {
            unlockedEvents = new List<Event>(eventHistory);
            foreach(Event e in eventHistory)
            {
                Debug.Log(e.name);
                uiControl.AddEventButton(e.name);
            }
        }
    }

    public void ResetEvents()
    {
        if(unlockedEvents != null)
            unlockedEvents = null;
    }
}

[System.Serializable]
public class Event
{
    public string name, englishName;
    public string description, englishDescription;
    public string imgFile;
    public string date;
    public int approval;
    [HideInInspector] public bool answered;
}
