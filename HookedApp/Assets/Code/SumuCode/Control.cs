using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField] GameObject conversationContainer, chatbuttonContainer, eventbuttonContainer;
    [SerializeField] TMP_Text score;
    MessageSender[] messageSenders;
    EventControl eventControl;
    [SerializeField] Color npcColor, playerColor;
    [SerializeField] Animator notificationAnimator, scoreAnimator;
    GameObject[] apps;
    bool animOn = false;

    private void Awake()
    {
        messageSenders = GetComponents<MessageSender>();
        eventControl = GetComponent<EventControl>();
        foreach(MessageSender ms in messageSenders)
        {
            ms.SetGameObjects(conversationContainer, chatbuttonContainer);
            ms.SetColors(npcColor, playerColor);
        }

        conversationContainer.SetActive(false);
        apps = GameObject.FindGameObjectsWithTag("App");
        foreach(GameObject g in apps)
        {
            g.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // checks if this is supposed to be a new game
        if (Stats.IsNewGame())
        {
            StartNewGame();
        }
        else
        {
            // loads the data
            SaveLoad gameLoader = GetComponent<SaveLoad>();
            gameLoader.LoadGame();
        }
    }

    private void Update()
    {
        // starts the start point animation
        if (score.text != Stats.GetApproval().ToString() && !animOn)
        {
            scoreAnimator.SetTrigger("getPoints");
            animOn = true;
        }
    }

    // updates the points when the animation is done
    public void UpdatePoints()
    {
        score.text = Stats.GetApproval().ToString();
        animOn = false;
    }


    void StartNewGame()
    {
        // hides everyone but the initial conversations
        foreach (Transform t in chatbuttonContainer.transform)
        {
            if (t.name != "Character_Marko")
            {
                t.gameObject.SetActive(false);
            }
        }

        // if this is a new game, start from the first phase which is marko 1
        foreach (MessageSender sender in messageSenders)
        {
            if(sender.GetName() == "Marko")
            {
                sender.StartWrapper(1);
            }
        }
    }

    // Starts a new conversation from a specified index
    public void StartConversation(string characterName, int conversationIndex)
    {
        if (!chatbuttonContainer.transform.Find("Character_" + characterName).gameObject.activeInHierarchy)
            chatbuttonContainer.transform.Find("Character_" + characterName).gameObject.SetActive(true);

        foreach (MessageSender sender in messageSenders)
        {
            if (sender.GetName() == characterName)
            {
                sender.StartWrapper(conversationIndex);
            }
        }
    }

    // tells the animator to display the notification
    public void NewMessagesNotif()
    {
        notificationAnimator.SetTrigger("newMessage");
    }

    // Unlocks the event and displays the notification
    public void EventUnlocked(int eventId)
    {
        eventControl.InvitedToEvent(eventId);
        notificationAnimator.SetTrigger("newEvent");
    }

    // opens or closes an app
    public void ToggleApp(string appName)
    {
        foreach (GameObject g in apps)
        {
            if (g.name == "APP: " + appName)
            {
                g.SetActive(!g.activeInHierarchy);
            }
        }
    }

    // toggles conversation window on and off
    public void ToggleConversation(string characterName)
    {
        ToggleApp("Chat");
        foreach(MessageSender sender in messageSenders)
        {
            if(sender.GetName() == characterName)
            {
                sender.ToggleConversation();
            }
        }
        conversationContainer.SetActive(!conversationContainer.activeInHierarchy);
    }

    // Loads the ending level with ending texts
    public void GameEnds(string title, string desc)
    {
        Stats.ChangeEndCredits(title, desc);
        LevelLoader loader = FindObjectOfType<LevelLoader>();
        loader.LoadLevel("Ending");
    }

    // returns the conversation history for saving
    public Dictionary<string, List<SentMessage>> GetConversationHistory()
    {
        Dictionary<string, List<SentMessage>> history = new Dictionary<string, List<SentMessage>>();
        foreach(MessageSender sender in messageSenders)
        {
            history.Add(sender.GetName(), sender.GetHistory());
        }
        return history;
    }

    // Sets the conversation history for each conversation
    public void SetConversationHistory(Dictionary<string, List<SentMessage>> history)
    {
        foreach (KeyValuePair<string, List<SentMessage>> pair in history)
        {
            foreach(MessageSender sender in messageSenders)
            {
                if (sender.GetName() == pair.Key)
                {
                    sender.SetHistory(pair.Value);

                    // adds the texts
                    foreach(SentMessage sm in pair.Value)
                    {
                        sender.AddMessagesLoad(sm.Text, sm.SentBy);
                    }

                    if (pair.Value.Count >= 1)
                    {
                        // if there's an open choice, add the choice button(s) too
                        if (pair.Value[pair.Value.Count - 1].OpenChoice)
                        {
                            sender.UnreadMessages();
                            Dialogue d = sender.GetDialogueById(pair.Value[pair.Value.Count - 1].Id);
                            sender.SetCurrentDialogue(d);
                            sender.AddButton(d.AnswerOneText, 0);
                            if (d.AnswerOneText != d.AnswerTwoText)
                                sender.AddButton(d.AnswerTwoText, 1);
                        }
                    }
                }
            }
        }

        // hides the buttons that shouldn't be visible yet
        foreach (Transform t in chatbuttonContainer.transform)
        {
            if (!history.ContainsKey(t.name.Replace("Character_", "")))
            {
                t.gameObject.SetActive(false);
            }
            else
            {
                if(history[t.name.Replace("Character_", "")].Count <= 0)
                {
                    t.gameObject.SetActive(false);
                }
            }
        }
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Sender sender;
    public int id;
    public string character;

    public enum Sender
    {
        npc,
        player
    }
}