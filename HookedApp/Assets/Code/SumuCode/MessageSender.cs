using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageSender : MonoBehaviour
{
    Control control;
    [SerializeField] private string characterName;
    GameObject conversationParent, conversationWindow, appButton;
    Color npcColor, playerColor;
    Image newMessagesImg;
    List<SentMessage> savedTexts = new List<SentMessage>();
    List<Dialogue> fullDialogue = new List<Dialogue>();
    Dialogue currDialogue;
    List<GameObject> buttons = new List<GameObject>();

    const string EVENT = "[E]";
    const string NEWCONVO = "[M]";
    const string CONTINUE = "[cont]";
    const string END = "[X]";
    const string WAIT = "[WAIT]";

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<Control>();
        LoadFromFile();
    }

    private void Update()
    {
        // For testing only
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StartMessages(1));
        }
    }

    // Change the sprite to unopened letter if there are unread messages
    public void UnreadMessages()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Art/icons");
        newMessagesImg.sprite = sprites[0];
    }

    // Starts the message sender
    public void StartWrapper(int index)
    {
        StartCoroutine(StartMessages(index));
    }

    // opens and closes the conversation
    public void ToggleConversation()
    {
        if (conversationParent.activeInHierarchy)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Art/icons");
            newMessagesImg.sprite = sprites[1];
        }

        conversationParent.SetActive(!conversationParent.activeInHierarchy);
    }

    // Starts to send messages and adding a delay
    public IEnumerator StartMessages(int index)
    {

        // Displays the notification and changes the icon if this screen is not active currently
        if (!conversationParent.activeInHierarchy)
        {
            control.NewMessagesNotif();
            UnreadMessages();
        }

        currDialogue = GetDialogueById(index);

        // Checks if the game is supposed to end before sending a message
        if (currDialogue.Next == NextAction.Ending)
        {
            control.GameEnds(currDialogue.EndingTitle, currDialogue.Text);
        }

        // Posts the first message with a randomized delay
        float delay = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(delay);
        AddMessage(currDialogue.Text, SentMessage.Sender.NPC);

        // Keeps sending new messages as long as it's supposed to go on
        while(currDialogue.Next == NextAction.Continue)
        {
            currDialogue = GetDialogueById(currDialogue.AnswerOneId);
            delay = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(delay);
            AddMessage(currDialogue.Text, SentMessage.Sender.NPC);
        }

        if (currDialogue.Next == NextAction.Option)
        {
            // Adds a choice button 
            savedTexts[savedTexts.Count - 1].OpenChoice = true;
            AddButton(currDialogue.AnswerOneText, 0);

            // Adds another too if the options are different
            if (currDialogue.AnswerTwoText != currDialogue.AnswerOneText)
                AddButton(currDialogue.AnswerTwoText, 1);
        }
        else if (currDialogue.Next == NextAction.EventInvite)
        {
            // Gets an event invite or two
            control.EventUnlocked(currDialogue.EventId);
            if(currDialogue.EventIdTwo != 0)
            {
                control.EventUnlocked(currDialogue.EventIdTwo);
            }
        }
        else if(currDialogue.Next == NextAction.NewConversation)
        {
            // Starts a new conversation with someone else
            string[] nextConvo = currDialogue.NewConversationName.Split(":");
            control.StartConversation(nextConvo[0], int.Parse(nextConvo[1]));

            // starts another conversation if there is one
            if(currDialogue.NewConversationNameTwo != null)
            {
                string[] nextConvoTwo = currDialogue.NewConversationNameTwo.Split(":");
                control.StartConversation(nextConvoTwo[0], int.Parse(nextConvoTwo[1]));
            }
        }
        else if (currDialogue.Next == NextAction.Ending)
        {
            // Ends the game
            control.GameEnds(currDialogue.EndingTitle, currDialogue.Text);
        }
        else if (currDialogue.Next == NextAction.Wait)
        {
            // Waits until some time has passed before this conversation goes on
            Debug.Log("Wait");
            if(characterName == "Leevi")
            {
                if(Stats.GetApproval() > 100)
                {
                    control.StartConversation("Leevi", 4);
                }
                else
                {
                    control.StartConversation("Leevi", 3);
                }
            }
            else if(characterName == "Tero")
            {

            }
        }
        else
        {
            // The conversation just ends here and nothing happens after it
            Debug.Log("End of conversation");
        }
    }

    // returns dialogue by id
    public Dialogue GetDialogueById(int id)
    {
        foreach(Dialogue d in fullDialogue)
        {
            if(d.Id == id)
            {
                return d;
            }
        }
        return null;
    }

    // Adds messages to the conversation without saving them
    // This is called only after loading the game
    public void AddMessagesLoad(string text, SentMessage.Sender sender)
    {
        GameObject newText = Instantiate(Resources.Load("Message") as GameObject, conversationWindow.transform);
        newText.GetComponentInChildren<TMP_Text>().text = text;
        switch (sender)
        {
            case SentMessage.Sender.NPC:
                newText.GetComponentInChildren<TMP_Text>().alignment = TextAlignmentOptions.Left;
                newText.GetComponentInChildren<TMP_Text>().color = npcColor;
                break;
            case SentMessage.Sender.Player:
                newText.GetComponentInChildren<TMP_Text>().alignment = TextAlignmentOptions.Right;
                newText.GetComponentInChildren<TMP_Text>().color = playerColor;
                break;
        }
    }

    // sends messages to the container
    public void AddMessage(string text, SentMessage.Sender sender)
    {
        // Saves the message to a list for asy saving
        SentMessage newMessage = new SentMessage();
        newMessage.Text = text;
        newMessage.SentBy = sender;
        newMessage.OpenChoice = false;
        newMessage.Id = currDialogue.Id;
        savedTexts.Add(newMessage);
        
        // Sends the message
        GameObject newText = Instantiate(Resources.Load("Message") as GameObject, conversationWindow.transform);
        newText.GetComponentInChildren<TMP_Text>().text = text;
        switch(sender)
        {
            // The sender is NPC
            case SentMessage.Sender.NPC:
                newText.GetComponentInChildren<TMP_Text>().alignment = TextAlignmentOptions.Left;
                newText.GetComponentInChildren<TMP_Text>().color = npcColor;
                break;
                // The sender is the player
            case SentMessage.Sender.Player:
                newText.GetComponentInChildren<TMP_Text>().alignment = TextAlignmentOptions.Right;
                newText.GetComponentInChildren<TMP_Text>().color = playerColor;
                break;
        }
    }

    // adds answer buttons to the container
    public void AddButton(string text, int number)
    {
        GameObject newButton = Instantiate(Resources.Load("AnswerOption") as GameObject, conversationWindow.transform);
        newButton.GetComponentInChildren<TMP_Text>().text = text;
        newButton.GetComponent<Button>().onClick.AddListener(delegate { ButtonClick(number); });
        buttons.Add(newButton);
    }

    // button clicks
    void ButtonClick(int number)
    {
        savedTexts[savedTexts.Count - 1].OpenChoice = false;

        if (number == 0)
        {
            // Chose the first option
            Stats.AddPoints(currDialogue.AnswerOneApproval);
            AddMessage(currDialogue.AnswerOneText, SentMessage.Sender.Player);
            StartCoroutine(StartMessages(currDialogue.AnswerOneId));
        }
        else
        {
            // Chose the second option
            Stats.AddPoints(currDialogue.AnswerTwoApproval);
            AddMessage(currDialogue.AnswerTwoText, SentMessage.Sender.Player);
            StartCoroutine(StartMessages(currDialogue.AnswerTwoId));
        }

        // Destroys all buttons when one of them is clicked
        foreach(GameObject btn in buttons)
        {
            Destroy(btn);
        }
    }

    // initializes the game objects
    public void SetGameObjects(GameObject conversations, GameObject chatButtons)
    {
        conversationWindow = conversations.transform.Find("Conversation_" + characterName).Find("Scroll View").Find("Viewport").Find("Conversation").gameObject;
        conversationParent = conversations.transform.Find("Conversation_" + characterName).gameObject;
        conversationParent.SetActive(false);
        newMessagesImg = chatButtons.transform.Find("Character_" + characterName).Find("Image").GetComponent<Image>();
    }

    // sets the message colors
    public void SetColors(Color npc, Color player)
    {
        npcColor = npc;
        playerColor = player;
    }

    // loads from text file
    void LoadFromFile()
    {
        TextAsset textData = Resources.Load("Story/" + Stats.language + "/" + characterName.ToLower()) as TextAsset;
        string txt = textData.text;
        var lines = txt.Split("\n");

        foreach(var line in lines)
        {
            string[] parts = line.Split("\t");
            Dialogue d = new Dialogue();

            // Id and text is always there
            d.Id = int.Parse(parts[0]);
            d.Text = parts[1];

            if (parts[2].StartsWith(EVENT))
            {
                // There is an indicator telling that this line leads to an event invite
                d.Next = NextAction.EventInvite;
                string eventString = parts[2].Replace(EVENT, "");

                if(eventString.Contains("+"))
                {
                    // If there's two event invites incoming
                    string[] events = eventString.Split("+");
                    d.EventId = int.Parse(events[0]);
                    d.EventIdTwo = int.Parse(events[1]);
                }
                else
                {
                    // There's only one event invite incoming
                    d.EventId = int.Parse(eventString);
                }
            }
            else if (parts[2].StartsWith(WAIT))
            {
                // This line makes the player wait for the conversation to continue
                d.Next = NextAction.Wait;
            }
            else if (parts[2].StartsWith(NEWCONVO))
            {
                // This line makes another character send messsages to the player
                d.Next = NextAction.NewConversation;

                if (parts[2].Contains("+"))
                {
                    // if there's two conversations incoming after this one
                    string[] convos = parts[2].Replace(NEWCONVO, "").Split("+");
                    d.NewConversationName = convos[0];
                    d.NewConversationNameTwo = convos[1];
                }
                else
                {
                    d.NewConversationName = parts[2].Replace(NEWCONVO, "");
                }
            }
            else if (parts[2].StartsWith(END))
            {
                // This line makes the game end
                d.Next = NextAction.Ending;
                string[] endingString = parts[1].Split(":");
                d.EndingTitle = endingString[0];
                d.Text = endingString[1];
            }
            else
            {
                if(parts.Length <= 3)
                {
                    // The conversation stops here
                    d.Next = NextAction.EndConversation;
                }
                else if (parts[3].StartsWith(CONTINUE))
                {
                    // The conversation automatically continues
                    d.Next = NextAction.Continue;
                    d.AnswerOneId = int.Parse(parts[2]);
                }
                else
                {
                    // There is an option to make
                    d.Next = NextAction.Option;
                    d.AnswerOneId = int.Parse(parts[2]);
                    d.AnswerOneText = parts[3];
                    d.AnswerOneApproval = int.Parse(parts[4]);
                    d.AnswerTwoId = int.Parse(parts[5]);
                    d.AnswerTwoText = parts[6];
                    d.AnswerTwoApproval = int.Parse(parts[7]);
                }

            }
            fullDialogue.Add(d);
        }
    }

    // Returns the name of this character
    public string GetName()
    {
        return characterName;
    }

    // Gets this characters conversation history for easy saving
    public List<SentMessage> GetHistory()
    {
        return savedTexts;
    }

    // Sets the history for this character
    public void SetHistory(List<SentMessage> history)
    {
        savedTexts = history;
    }

    // Sets the current dialogue so the conversation won't start from the beginning
    public void SetCurrentDialogue(Dialogue d)
    {
        currDialogue = d;
    }
}

[System.Serializable]
public class Dialogue
{
    public int Id;
    public string Text;
    public NextAction Next;
    public int AnswerOneId;
    public int AnswerTwoId;
    public string AnswerOneText;
    public string AnswerTwoText;
    public int AnswerOneApproval;
    public int AnswerTwoApproval;
    public int EventId;
    public int EventIdTwo;
    public string NewConversationName;
    public string NewConversationNameTwo;
    public string EndingTitle;
}

[System.Serializable]
public enum NextAction
{
    Continue,
    Option,
    NewConversation,
    EventInvite,
    EndConversation,
    Ending,
    Wait
}

[System.Serializable]
public class SentMessage
{
    public string Text;
    public int Id;
    public Sender SentBy;
    public bool OpenChoice;
    
    public enum Sender
    {
        Player,
        NPC
    }
}