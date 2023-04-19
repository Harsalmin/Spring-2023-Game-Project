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
    [SerializeField] private TMP_Text attending, notAttending, chatText, eventsText, logsText, logsTitle, settingsText;
    [SerializeField] private TMP_Text scoreText, eventPopupText;
    [SerializeField] private Animator starAnim, eventPopupAnim, notification;
    [SerializeField] private Image fade;
    [SerializeField] private ScrollRect scroll;
    private GameObject viewport;
    EventManager eventManager;
    GameControl control;
    LevelLoader levelLoader;
    SaveLoad saves;
    bool animOn = false;

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
        ChangeLanguage();
    }

    private void Start()
    {
        UpdatePoints();
    }

    private void Update()
    {
        if (scoreText.text != Stats.GetApproval().ToString() && !animOn)
        {
            starAnim.SetTrigger("getPoints");
            animOn = true;
        }
    }

    public void UpdatePoints()
    {
        scoreText.text = Stats.GetApproval().ToString();
        animOn = false;
    }

    public void FadePopup(bool fadeIn)
    {
        if(fadeIn)
        {
            StartCoroutine(FadeInPopUp());
        }
        else
        {
            StartCoroutine(FadeOutPopUp());
        }
    }

    public void ChangePopupText(string text)
    {
        eventPopupText.text = text;
    }

    private void LanguageChanged(TMP_Dropdown languageOptions)
    {
        ChangeLanguage();
        control.ChangeLanguage(languageOptions.value == 0);
    }

    public IEnumerator FadeInPopUp()
    {
        float timer = 0;
        float duration = 1;
        while(timer <= duration)
        {
            timer += Time.deltaTime * 2;
            fade.color = new Color(0, 0, 0, timer);
            yield return null;
        }
        fade.color = new Color(0, 0, 0, 1);
        CloseEventScreen();
        ToggleEventsApp();
        ToggleLogsApp();
        eventPopupAnim.SetTrigger("show");
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
        if (eventApp.activeInHierarchy)
        {
            ChangeLanguage();
        }
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
            if (Stats.language == "fi")
            {
                logsText.text = GameControl.logText;
            }
            else
            {
                logsText.text = GameControl.logTextEnglish;
            }
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
        notification.SetTrigger("newMessage");
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
        conversations.transform.Find("Name").GetComponent<TMP_Text>().text = name;
        Sprite img = Resources.Load<Sprite>("Art/Characters/" + name);
        conversations.transform.Find("Image").GetComponent<Image>().sprite = img;
        foreach(Transform convo in viewport.transform)
        {
            if(convo.gameObject.name == CHARACTERINDICATOR + name)
            {
                scroll.content = convo.gameObject.GetComponent<RectTransform>();
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
        notification.SetTrigger("newEvent");
        Debug.Log("Adding event button");
        // Adds a button for an event
        if (!eventApp.transform.Find(CONTAINERNAME).transform.Find(EVENTINDICATOR + eventName))
        {
            GameObject newEventBtn = Instantiate(button, eventApp.transform.Find(CONTAINERNAME).transform);
            newEventBtn.name = EVENTINDICATOR + eventName;
            newEventBtn.GetComponent<Button>().onClick.AddListener(delegate { OpenEvent(eventName); });
            newEventBtn.GetComponentInChildren<TMP_Text>().text = eventName;
            eventManager.EventUnlocked(eventManager.GetEventByName(eventName));
            Debug.Log("Event button added " + eventName);
        }
    }

    public void ChangeLanguage()
    {
        // Event names and descriptions
        foreach(Transform t in eventApp.transform.Find(CONTAINERNAME))
        {
            Debug.Log(t.name);
            if (t.name.StartsWith(EVENTINDICATOR))
            {
                string originalName = t.name.Replace(EVENTINDICATOR, "");
                Event e = eventManager.GetEventByName(originalName);
                if (Stats.language == "fi")
                {
                    Debug.Log(e.name);
                    t.GetComponentInChildren<TMP_Text>().text = e.name;
                }
                else
                {
                    t.GetComponentInChildren<TMP_Text>().text = e.englishName;
                }
            }
        }

        // Settings 
        foreach(Transform t in settingsApp.transform.Find(CONTAINERNAME))
        {
            if(Stats.language == "fi")
            {
                switch(t.name)
                {
                    case "Title":
                        t.GetComponent<TMP_Text>().text = "Asetukset";
                        break;
                    case "Description":
                        t.GetComponent<TMP_Text>().text = "T‰‰ll‰ on asetukset";
                        break;
                    case "Save":
                        t.GetComponentInChildren<TMP_Text>().text = "Tallenna peli";
                        break;
                    case "Load":
                        t.GetComponentInChildren<TMP_Text>().text = "Lataa peli";
                        break;
                    case "Exit":
                        t.GetComponentInChildren<TMP_Text>().text = "Poistu p‰‰valikkoon";
                        break;
                }
            }
            else
            {
                switch (t.name)
                {
                    case "Title":
                        t.GetComponent<TMP_Text>().text = "Settings";
                        break;
                    case "Description":
                        t.GetComponent<TMP_Text>().text = "Here be the settings";
                        break;
                    case "Save":
                        t.GetComponentInChildren<TMP_Text>().text = "Save";
                        break;
                    case "Load":
                        t.GetComponentInChildren<TMP_Text>().text = "Load";
                        break;
                    case "Exit":
                        t.GetComponentInChildren<TMP_Text>().text = "Exit to main menu";
                        break;
                }
            }
        }

        if(Stats.language == "fi")
        {
            attending.text = "Osallistun";
            notAttending.text = "En osallistu";
            chatText.text = "Keskustelut";
            eventsText.text = "Tapahtumat";
            logsText.text = "Loki";
            logsTitle.text = "Loki";
            settingsText.text = "Asetukset";
        }
        else
        {
            attending.text = "Attending";
            notAttending.text = "Not attending";
            chatText.text = "Chat";
            eventsText.text = "Events";
            logsText.text = "Logs";
            logsTitle.text = "Logs";
            settingsText.text = "Settings";
        }
    }

    private void OpenEvent(string eventName)
    {
        // Opens an event screen and changes its contents
        Event e = eventManager.GetEventByName(eventName);

        // changes texts to match the language
        if(Stats.language == "fi")
        {
            eventWindow.transform.Find(CONTAINERNAME).transform.Find("Title").GetComponent<TMP_Text>().text = e.name;
            eventWindow.transform.Find(CONTAINERNAME).transform.Find("Description").GetComponent<TMP_Text>().text = e.description;
        }
        else
        {
            eventWindow.transform.Find(CONTAINERNAME).transform.Find("Title").GetComponent<TMP_Text>().text = e.englishName;
            eventWindow.transform.Find(CONTAINERNAME).transform.Find("Description").GetComponent<TMP_Text>().text = e.englishDescription;
        }

        if (e.imgFile != "" || e.imgFile != null)
        {
            // if there is a photo of the event, load it from resources
            Sprite img = Resources.Load<Sprite>("Art/Events/" + e.imgFile);
            eventWindow.transform.Find(CONTAINERNAME).transform.Find("Image").GetComponent<Image>().sprite = img;
        }

        DisableEventButtons(e.answered);
        eventWindow.transform.Find(CONTAINERNAME).transform.Find("Date").GetComponent<TMP_Text>().text = e.date;
        eventWindow.SetActive(true);
    }

    public void DisableEventButtons(bool isAnswered)
    {
        // Disables (or activates) event buttons
        eventWindow.transform.Find(CONTAINERNAME).transform.Find("Yes").GetComponent<Button>().interactable = !isAnswered;
        eventWindow.transform.Find(CONTAINERNAME).transform.Find("No").GetComponent<Button>().interactable = !isAnswered;
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
