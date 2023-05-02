using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    [SerializeField] GameObject conversationContainer, chatbuttonContainer, eventbuttonContainer;
    [SerializeField] TMP_Text score;
    MessageSender[] messageSenders;
    EventControl eventControl;
    [SerializeField] Color npcColor, playerColor;
    [SerializeField] Animator notificationAnimator, scoreAnimator;
    [SerializeField] Button loadButton;
    GameObject[] apps;
    bool animOn = false;

    private void Awake()
    {
        // Get the important stuff first
        messageSenders = GetComponents<MessageSender>();
        eventControl = GetComponent<EventControl>();

        // Set the gameobjects and colors to each sender script
        foreach(MessageSender ms in messageSenders)
        {
            ms.SetGameObjects(conversationContainer, chatbuttonContainer);
            ms.SetColors(npcColor, playerColor);
        }
        conversationContainer.SetActive(false);

        // Initially deactivate all apps
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
            foreach(MessageSender sender in messageSenders)
            {
                sender.LoadFromFile();
            }

            SaveLoad gameLoader = GetComponent<SaveLoad>();
            gameLoader.LoadGame();
        }

        // disables the load button if there is no save
        if(PlayerPrefs.GetInt("Save exists") != 1)
        {
            loadButton.interactable = false;
        }
        else
        {
            loadButton.interactable = true;
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

    // Starts a fresh new game
    void StartNewGame()
    {
        // resets the star points
        Stats.ResetPoints();

        foreach(MessageSender sender in messageSenders)
        {
            sender.LoadFromFile();
        }

        // hides everyone but the initial conversation buttons
        foreach (Transform t in chatbuttonContainer.transform)
        {
            if (t.name != "Character_Marko" && t.name != "Character_Joni")
            {
                t.gameObject.SetActive(false);
            }
        }

        // if this is a new game, start from the first phase which is marko 1
        foreach (MessageSender sender in messageSenders)
        {
            if(sender.GetName() == "Marko" || sender.GetName() == "Joni")
            {
                Debug.Log("Starting " + sender.GetName());
                sender.StartWrapper(1);
            }
        }
    }

    // Starts a new conversation from a specified index
    public void StartConversation(string characterName, int conversationIndex)
    {
        // If this is a first message from this person, activate their button too
        if (!chatbuttonContainer.transform.Find("Character_" + characterName).gameObject.activeInHierarchy)
            chatbuttonContainer.transform.Find("Character_" + characterName).gameObject.SetActive(true);

        // Start a conversation by telling the message sender to start from a specified index
        foreach (MessageSender sender in messageSenders)
        {
            if (sender.GetName() == characterName)
            {
                sender.StartWrapper(conversationIndex);
            }
        }
    }

    // Tells the animator to display the notification
    public void NewMessagesNotif()
    {
        if (notificationAnimator != null)
        {
            notificationAnimator.SetTrigger("newMessage");
        }
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
        Debug.Log(characterName);
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

    // Sets the conversation history for each conversation by cycling through every key-value pair that is saved
    public void SetConversationHistory(Dictionary<string, List<SentMessage>> history)
    {
        foreach (KeyValuePair<string, List<SentMessage>> pair in history)
        {
            foreach(MessageSender sender in messageSenders)
            {
                // Finds the sender that has the same name as the current key
                if (sender.GetName() == pair.Key)
                {
                    // Adds the message list to it
                    sender.SetHistory(pair.Value);

                    // Adds the texts to their windows
                    foreach(SentMessage sm in pair.Value)
                    {
                        sender.AddMessagesLoad(sm.Text, sm.SentBy);
                    }

                    // Check open choices only if there is something in the list
                    if (pair.Value.Count >= 1)
                    {
                        Debug.Log(pair.Key + pair.Value[pair.Value.Count - 1].Id);
                        // if there's an open choice, add the choice button(s) too
                        if (pair.Value[pair.Value.Count - 1].OpenChoice)
                        {
                            sender.UnreadMessages();
                            Dialogue d = sender.GetDialogueById(pair.Value[pair.Value.Count - 1].Id);
                            sender.SetCurrentDialogue(d);
                            sender.AddButton(d.AnswerOneText, 0);

                            // add the second choice button only if it is different from the first
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
            // Hides the buttons that don't even have their stories
            if (!history.ContainsKey(t.name.Replace("Character_", "")))
            {
                t.gameObject.SetActive(false);
            }
            else
            {
                // Hides the conversations that have no messages in them
                if(history[t.name.Replace("Character_", "")].Count <= 0)
                {
                    t.gameObject.SetActive(false);
                }
            }
        }
    }

    // Makes the load button interactable
    public void ActivateLoadButton()
    {
        loadButton.interactable = true;
    }

    // Tell event control that it needs to wait for Tero to think about things
    public void WaitForTero()
    {
        eventControl.WaitForTero();
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