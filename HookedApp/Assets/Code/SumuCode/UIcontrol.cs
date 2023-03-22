using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIcontrol : MonoBehaviour
{
    [SerializeField] private GameObject chatApp, eventApp, logsApp, settingsApp;
    [SerializeField] private GameObject hub, button, convoWindow, conversations, eventWindow;
    private GameObject viewport;
    EventManager eventManager;
    GameControl control;
    LevelLoader levelLoader;
    SaveLoad saves;

    private const string CONTAINERNAME = "Container";
    private const string CHARACTERINDICATOR = "Character_";
    private const string CONVERSATIONINDICATOR = "Conversation_";
    private const string EVENTINDICATOR = "Event_";

    void Awake()
    {
        eventManager = GetComponent<EventManager>();
        control = GetComponent<GameControl>();
        levelLoader = GameObject.Find("Loader").GetComponent<LevelLoader>();
        saves = GetComponent<SaveLoad>();
        chatApp.SetActive(false);
        eventApp.SetActive(false);
        logsApp.SetActive(false);
        settingsApp.SetActive(false);
        conversations.SetActive(false);
        eventWindow.SetActive(false);
        viewport = conversations.transform.Find("Scroll View/Viewport").gameObject;
    }

    public IEnumerator RefreshReferences()
    {
        // Refreshed the references to gameobjects
        // maybe test if this is even needed
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        GameObject canvas;
        foreach(GameObject go in rootObjects)
        {
            if (go.name == "Canvas")
            {
                canvas = go;
                hub = canvas.transform.Find("Hub").gameObject;
                chatApp = canvas.transform.Find("APP: Chat").gameObject;
                foreach(Transform t in chatApp.transform.Find(CONTAINERNAME).transform)
                {
                    print("children: " + t.name);
                }
                eventApp = canvas.transform.Find("APP: Events").gameObject;

            }
        }

        Debug.Log("Refreshed");
        yield return null;
    }

    public void ToggleChatApp()
    {
        // Opens and closes chat app
        chatApp.SetActive(!chatApp.activeInHierarchy);
        hub.SetActive(!hub.activeInHierarchy);
    }

    public void ToggleEventsApp()
    {
        // Opens and closes events app
        eventApp.SetActive(!eventApp.activeInHierarchy);
        hub.SetActive(!hub.activeInHierarchy);
    }

    public void ToggleLogsApp()
    {
        // Opens and closes logs app
        logsApp.SetActive(!logsApp.activeInHierarchy);
        hub.SetActive(!hub.activeInHierarchy);

        if(logsApp.activeInHierarchy)
        {
            GameObject logs = logsApp.transform.Find(CONTAINERNAME).transform.Find("Logs").gameObject;
            TMP_Text logsText = logs.GetComponent<TMP_Text>();
            logsText.text = GameControl.logText;
        }
    }

    public void ToggleSettings()
    {
        // Opens and closes the settings
        settingsApp.SetActive(!settingsApp.activeInHierarchy);
        hub.SetActive(!hub.activeInHierarchy);
    }

    public void AddConvoButton(string name)
    {
        // Adds a button and a conversation window for a character
        if (!chatApp.transform.Find(CONTAINERNAME).transform.Find(CONVERSATIONINDICATOR + name))
        {
            GameObject newConvoBtn = Instantiate(button, chatApp.transform.Find(CONTAINERNAME).transform);
            newConvoBtn.name = CONVERSATIONINDICATOR + name;
            newConvoBtn.GetComponent<Button>().onClick.AddListener(delegate { OpenConversation(name); });
            newConvoBtn.GetComponentInChildren<TMP_Text>().text = name;
            GameObject newConvo = Instantiate(convoWindow, viewport.transform);
            newConvo.name = CHARACTERINDICATOR + name;
        }
    }

    public void OpenConversation(string name)
    {
        // Opens a conversation by name
        ToggleChatApp();
        conversations.SetActive(true);
        foreach(Transform convo in viewport.transform)
        {
            if(convo.gameObject.name == CHARACTERINDICATOR + name)
            {
                convo.gameObject.SetActive(true);
            }
        }
    }

    public void CloseConversations()
    {
        // Closes all conversations that may be active
        foreach(Transform convo in viewport.transform)
        {
            if(convo.gameObject.activeInHierarchy)
            {
                convo.gameObject.SetActive(false);
            }
        }
        conversations.SetActive(false);
        ToggleChatApp();
    }

    public void AddEventButton(string eventName)
    {
        // Adds a button for an event
        if (!eventApp.transform.Find(CONTAINERNAME).transform.Find(EVENTINDICATOR + eventName))
        {
            GameObject newEventBtn = Instantiate(button, eventApp.transform.Find(CONTAINERNAME).transform);
            newEventBtn.name = EVENTINDICATOR + eventName;
            newEventBtn.GetComponent<Button>().onClick.AddListener(delegate { OpenEvent(eventName); });
            newEventBtn.GetComponentInChildren<TMP_Text>().text = eventName;
            eventManager.EventUnlocked(eventManager.GetEventByName(eventName));
        }
    }

    private void OpenEvent(string eventName)
    {
        // Opens an event screen and changes its contents
        Event e = eventManager.GetEventByName(eventName);
        eventWindow.transform.Find(CONTAINERNAME).transform.Find("Title").GetComponent<TMP_Text>().text = e.name;

        if (e.imgFile != "" || e.imgFile != null)
        {
            // if there is a photo of the event, load it from resources
            Sprite img = Resources.Load("Art/Events/" + e.imgFile) as Sprite;
            eventWindow.transform.Find(CONTAINERNAME).transform.Find("Image").GetComponent<Image>().sprite = img;
        }

        DisableEventButtons(e.answered);
        eventWindow.transform.Find(CONTAINERNAME).transform.Find("Date").GetComponent<TMP_Text>().text = e.date;
        eventWindow.transform.Find(CONTAINERNAME).transform.Find("Description").GetComponent<TMP_Text>().text = e.description;
        eventWindow.SetActive(true);
    }

    public void DisableEventButtons(bool isAnswered)
    {
        // Disables (or activates) event buttons
        eventWindow.transform.Find(CONTAINERNAME).transform.Find("Yes").GetComponent<Button>().enabled = !isAnswered;
        eventWindow.transform.Find(CONTAINERNAME).transform.Find("No").GetComponent<Button>().enabled = !isAnswered;
    }

    public void CloseEventScreen()
    {
        // Closes event screen and returns to event list
        eventWindow.SetActive(false);
    }

    public void PressSave()
    {
        saves.SaveGame();
    }

    public void PressLoad()
    {
        saves.LoadGame();
    }

    public void PressExit()
    {
        levelLoader.LoadLevel("StartMenu");
    }
}
