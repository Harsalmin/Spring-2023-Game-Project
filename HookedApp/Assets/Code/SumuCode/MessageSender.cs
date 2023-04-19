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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StartMessages(1));
        }
    }

    public void UnreadMessages()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Art/icons");
        newMessagesImg.sprite = sprites[0];
    }

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
        if (!conversationParent.activeInHierarchy)
        {
            control.NewMessagesNotif();
            Sprite[] sprites = Resources.LoadAll<Sprite>("Art/icons");
            newMessagesImg.sprite = sprites[0];
        }

        currDialogue = GetDialogueById(index);
        float delay = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(delay);
        AddMessage(currDialogue.Text, SentMessage.Sender.NPC);

        while(currDialogue.Next == NextAction.Continue)
        {
            currDialogue = GetDialogueById(currDialogue.AnswerOneId);
            delay = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(delay);
            AddMessage(currDialogue.Text, SentMessage.Sender.NPC);
        }

        if (currDialogue.Next == NextAction.Option)
        {
            savedTexts[savedTexts.Count - 1].OpenChoice = true;
            AddButton(currDialogue.AnswerOneText, 0);
            if (currDialogue.AnswerTwoText != currDialogue.AnswerOneText)
                AddButton(currDialogue.AnswerTwoText, 1);
        }
        else if (currDialogue.Next == NextAction.EventInvite)
        {
            control.EventUnlocked(currDialogue.EventId);
            if(currDialogue.EventIdTwo != 0)
            {
                control.EventUnlocked(currDialogue.EventIdTwo);
            }
        }
        else if(currDialogue.Next == NextAction.NewConversation)
        {
            Debug.Log("Next conversation");
            string[] nextConvo = currDialogue.NewConversationName.Split(":");
            control.StartConversation(nextConvo[0], int.Parse(nextConvo[1]));
        }
        else if (currDialogue.Next == NextAction.Ending)
        {
            Debug.Log("Ending");
            control.GameEnds(currDialogue.EndingTitle, currDialogue.Text);
        }
        else if (currDialogue.Next == NextAction.Wait)
        {
            Debug.Log("Wait");
        }
        else
        {
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
        SentMessage newMessage = new SentMessage();
        newMessage.Text = text;
        newMessage.SentBy = sender;
        newMessage.OpenChoice = false;
        newMessage.Id = currDialogue.Id;
        savedTexts.Add(newMessage);
        
        GameObject newText = Instantiate(Resources.Load("Message") as GameObject, conversationWindow.transform);
        newText.GetComponentInChildren<TMP_Text>().text = text;
        switch(sender)
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
            Stats.AddPoints(currDialogue.AnswerOneApproval);
            AddMessage(currDialogue.AnswerOneText, SentMessage.Sender.Player);
            StartCoroutine(StartMessages(currDialogue.AnswerOneId));
        }
        else
        {
            Stats.AddPoints(currDialogue.AnswerTwoApproval);
            AddMessage(currDialogue.AnswerTwoText, SentMessage.Sender.Player);
            StartCoroutine(StartMessages(currDialogue.AnswerTwoId));
        }

        foreach(GameObject btn in buttons)
        {
            Destroy(btn);
        }
    }

    // initializes the game objects
    public void SetGameObjects(GameObject conversations, GameObject chatButtons)
    {
        Debug.Log("Set containers");
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

            d.Id = int.Parse(parts[0]);
            d.Text = parts[1];

            if (parts[2].StartsWith(EVENT))
            {
                d.Next = NextAction.EventInvite;
                string eventString = parts[2].Replace(EVENT, "");
                if(eventString.Contains("+"))
                {
                    string[] events = eventString.Split("+");
                    d.EventId = int.Parse(events[0]);
                    d.EventIdTwo = int.Parse(events[1]);
                }
                else
                {
                    d.EventId = int.Parse(eventString);
                }
            }
            else if (parts[2].StartsWith(WAIT))
            {
                d.Next = NextAction.Wait;
            }
            else if (parts[2].StartsWith(NEWCONVO))
            {
                d.Next = NextAction.NewConversation;
                d.NewConversationName = parts[2].Replace(NEWCONVO, "");
            }
            else if (parts[2].StartsWith(END))
            {
                d.Next = NextAction.Ending;
                string[] endingString = parts[1].Split(":");
                d.EndingTitle = endingString[0];
                d.Text = endingString[1];
            }
            else
            {
                if(parts.Length <= 3)
                {
                    d.Next = NextAction.EndConversation;
                }
                else if (parts[3].StartsWith(CONTINUE))
                {
                    d.Next = NextAction.Continue;
                    d.AnswerOneId = int.Parse(parts[2]);
                }
                else
                {
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

    public string GetName()
    {
        return characterName;
    }

    public List<SentMessage> GetHistory()
    {
        return savedTexts;
    }

    public void SetHistory(List<SentMessage> history)
    {
        savedTexts = history;
    }

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