using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Event[] allEvents;
    Stats stats;
    UIcontrol uiControl;
    GameControl gameControl;
    private void Start()
    {
        stats = GetComponent<Stats>();
        uiControl = GetComponent<UIcontrol>();
        gameControl = GetComponent<GameControl>();
    }

    public void AnswerYesToInvite(GameObject eventTitle)
    {
        string eventName = eventTitle.GetComponent<TMP_Text>().text;
        // If agreed to go to the event, add approval points
        foreach(Event e in allEvents)
        {
            if(e.name == eventName)
            {
                stats.AddPoints(e.approval);
                e.answered = true;
            }
        }
        gameControl.EndPhase("Yes:" + eventName);
        uiControl.CloseEventScreen();
    }

    public void AnswerNoToInvite(GameObject eventTitle)
    {
        string eventName = eventTitle.GetComponent<TMP_Text>().text;
        // If the player says no to an event, do [something] (or nothing idk)
        foreach (Event e in allEvents)
        {
            if (e.name == eventName)
            {
                e.answered = true;
            }
        }
        gameControl.EndPhase("No:" + eventName);
        uiControl.CloseEventScreen();
    }

    public Event GetEventByName(string name)
    {
        foreach(Event e in allEvents)
        {
            if (e.name == name)
            {
                return e;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Event
{
    public string name;
    public string description;
    public string imgFile;
    public int approval;
    public bool answered;
}
