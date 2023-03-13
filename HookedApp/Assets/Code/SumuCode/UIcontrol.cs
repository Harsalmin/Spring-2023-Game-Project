using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIcontrol : MonoBehaviour
{
    [SerializeField] private GameObject hub, chatApp, eventApp, button, convoWindow, conversations, eventWindow;
    private GameObject viewport;
    EventManager eventManager;

    void Awake()
    {
        eventManager = GetComponent<EventManager>();
        chatApp.SetActive(false);
        eventApp.SetActive(false);
        conversations.SetActive(false);
        eventWindow.SetActive(false);
        viewport = conversations.transform.Find("Scroll View/Viewport").gameObject;
    }

    public void ToggleChat()
    {
        // Opens and closes chat hub
        chatApp.SetActive(!chatApp.activeInHierarchy);
        hub.SetActive(!hub.activeInHierarchy);
    }

    public void ToggleEvents()
    {
        // Opens and closes event hub
        eventApp.SetActive(!eventApp.activeInHierarchy);
        hub.SetActive(!hub.activeInHierarchy);
    }

    public void AddConvoButton(string name)
    {
        // Adds a button and a conversation window for a character
        GameObject newConvoBtn = Instantiate(button, chatApp.transform.Find("Container").transform);
        newConvoBtn.GetComponent<Button>().onClick.AddListener(delegate { OpenConversation(name); });
        newConvoBtn.GetComponentInChildren<TMP_Text>().text = name;
        GameObject newConvo = Instantiate(convoWindow, viewport.transform);
        newConvo.name = "Character_" + name;
    }

    public void OpenConversation(string name)
    {
        // Opens a conversation by name
        ToggleChat();
        conversations.SetActive(true);
        foreach(Transform convo in viewport.transform)
        {
            if(convo.gameObject.name == "Character_" + name)
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
        ToggleChat();
    }

    public void AddEventButton(string eventName)
    {
        // Adds a button for an event
        GameObject newEventBtn = Instantiate(button, eventApp.transform.Find("Container").transform);
        newEventBtn.GetComponent<Button>().onClick.AddListener(delegate { OpenEvent(eventName); });
        newEventBtn.GetComponentInChildren<TMP_Text>().text = eventName;
    }

    private void OpenEvent(string eventName)
    {
        // Opens an event screen and changes its contents
        Event e = eventManager.GetEventByName(eventName);
        eventWindow.transform.Find("Container").transform.Find("Title").GetComponent<TMP_Text>().text = e.name;
        if (e.imgFile != "" || e.imgFile != null)
        {
            Sprite img = Resources.Load<Sprite>(e.imgFile);
            eventWindow.transform.Find("Container").transform.Find("Image").GetComponent<Image>().sprite = img;
        }

        if(e.answered)
        {
            eventWindow.transform.Find("Container").transform.Find("Yes").GetComponent<Button>().enabled = false;
            eventWindow.transform.Find("Container").transform.Find("No").GetComponent<Button>().enabled = false;
        }
        else
        {
            eventWindow.transform.Find("Container").transform.Find("Yes").GetComponent<Button>().enabled = true;
            eventWindow.transform.Find("Container").transform.Find("No").GetComponent<Button>().enabled = true;
        }

        eventWindow.transform.Find("Container").transform.Find("Description").GetComponent<TMP_Text>().text = e.description;
        eventWindow.SetActive(true);
    }

    public void CloseEventScreen()
    {
        // Closes event screen and returns to event list
        eventWindow.SetActive(false);
    }
}
