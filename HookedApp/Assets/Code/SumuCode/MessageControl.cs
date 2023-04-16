using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MessageControl : MonoBehaviour
{
    List<Message> messages = new List<Message>();
    Dictionary<string, List<Message>> conversationHistory;
    private string currentNpc;
    public GameObject viewport, textObj, buttonObj, convoObj, lineObj;
    private GameObject messageContainer;
    [SerializeField] private Color npcColor, playerColor;
    List<GameObject> buttons = new List<GameObject>();
    Dictionary<string, List<GameObject>> answerButtons = new Dictionary<string, List<GameObject>>();
    private int index = 0;
    UIcontrol uiControl;
    public bool languageChanged = false;

    private List<string> phaseCharacters = new List<string>();

    private const string CHARACTERINDICATOR = "Character_";
    // private const string CONVERSATIONINDICATOR = "Conversation_";

    private void Start()
    {
        uiControl = GetComponent<UIcontrol>();
    }

    public void NewPhase()
    {
        phaseCharacters.Clear();
    }

    // Starts a new day and destroys all buttons
    public void NewDay()
    {
        List<GameObject> convos = new List<GameObject>();
        foreach (Transform t in viewport.transform)
        {
            if (t.name.StartsWith(CHARACTERINDICATOR))
            {
                convos.Add(t.gameObject);
                Instantiate(lineObj, t);
                string charName = t.name.Replace(CHARACTERINDICATOR, "");
                foreach (GameObject g in answerButtons[charName])
                {
                    Destroy(g);
                }
            }
        }
    }

    // Sends a message to the chat window
    public void AddMessage(string text, Message.Sender sender, string characterName)
    {
        index++;
        currentNpc = characterName;
        Message newMessage = new Message();
        newMessage.id = index;
        newMessage.text = text;
        newMessage.sender = sender;
        newMessage.character = characterName;
        GameObject newText = Instantiate(textObj, viewport.transform.Find(CHARACTERINDICATOR + characterName));
        TMP_Text txt = newText.GetComponent<TMP_Text>();
        txt.text = newMessage.text;
        newText.GetComponent<TextMeshProUGUI>().faceColor = MsgColor(newMessage.sender);

        if(!phaseCharacters.Contains(characterName))
        {
            phaseCharacters.Add(characterName);
        }

        // Changes the alignment depending on who send the message
        switch (sender)
        {
            case Message.Sender.npc:
                txt.alignment = TextAlignmentOptions.Left;
                break;
            case Message.Sender.player:
                txt.alignment = TextAlignmentOptions.Right;
                break;
        }

        messages.Add(newMessage);
        SaveMessages();
    }

    // Adds a button and a listener
    public void AddButton(string text, int number, Choices choiceScript, string characterName)
    {
        GameObject newButton = Instantiate(buttonObj, viewport.transform.Find(CHARACTERINDICATOR + characterName));
        newButton.GetComponentInChildren<TMP_Text>().text = text;
        newButton.GetComponent<Button>().onClick.AddListener(delegate { ButtonClick(number, choiceScript, choiceScript.actualName); });
        buttons.Add(newButton);
        if (!answerButtons.ContainsKey(characterName))
        {
            answerButtons.Add(characterName, new List<GameObject>());
        }
        answerButtons[characterName].Add(newButton);
    }

    // Destroys all answer buttons in current conversation
    public void ButtonClick(int number, Choices choiceScript, string characterName)
    {
        choiceScript.ButtonClicked(number);
        foreach(GameObject btn in answerButtons[characterName])
        {
            Destroy(btn);
        }
    }

    // Changes the color of the message depending on who the sender is
    private Color MsgColor (Message.Sender sender)
    {
        Color color = playerColor;
        switch(sender)
        {
            case Message.Sender.npc:
                color = npcColor;
                break;
            case Message.Sender.player:
                color = playerColor;
                break;
        }
        return color;
    }

    // Saves all messages that are currently stored
    public void SaveMessages()
    {
        if(conversationHistory == null)
        {
            conversationHistory = new Dictionary<string, List<Message>>();
        }

        foreach(Message msg in messages)
        {
            if(!conversationHistory.ContainsKey(msg.character))
            {
                Debug.Log(msg.character + " added to list");
                conversationHistory.Add(msg.character, new List<Message>());
            }

            if (!conversationHistory[msg.character].Contains(msg))
            {
                Debug.Log("Stored: " + msg.text + " to " + msg.character);
                conversationHistory[msg.character].Add(msg);
            }
        }
    }


    // Changes which conversation is active currently
    public void ChangeConversation(string characterName)
    {
        Debug.Log("conversation: " + characterName);

        // saves messages
        if (currentNpc != null && !languageChanged)
        {
            SaveMessages();
            languageChanged = false;
        }
        currentNpc = characterName;
        messages.Clear();
        messageContainer = viewport.transform.Find(CHARACTERINDICATOR + characterName).gameObject;
    }

    // Adds a new conversation
    public void AddNewConversation(string name)
    {
        GameObject newConversation = Instantiate(convoObj, viewport.transform);
        newConversation.name = CHARACTERINDICATOR + name;
    }

    // returns the dictionary for saving
    public Dictionary<string, List<Message>> GetConversationHistory()
    {
        foreach(string s in conversationHistory.Keys)
        {
            foreach(Message msg in conversationHistory[s])
            {
                Debug.Log(msg.text + " added to " + s);
            }
        }
        return conversationHistory;
    }

    // Loads all previous conversation history
    public void SetConversationHistory(Dictionary<string, List<Message>> convoHistory)
    {
        if(convoHistory == null)
        {
            return;
        }

        Debug.Log(convoHistory.Keys.ToList().Count);

        conversationHistory = convoHistory;
        foreach(string s in conversationHistory.Keys.ToList())
        {
            uiControl.AddConvoButton(s);
            ChangeConversation(s);
            List<int> processedIDs = new List<int>();
            Debug.Log(s);
            foreach (Message msg in conversationHistory[s])
            {
                Debug.Log(msg.id);
                // prevents duplicates
                if (processedIDs.Contains(msg.id))
                {
                    Debug.Log("Contains");
                    continue;
                }
                processedIDs.Add(msg.id);

                GameObject newText = Instantiate(textObj, messageContainer.transform);
                Debug.Log(msg.text + " added to " + s);
                TMP_Text txt = newText.GetComponent<TMP_Text>();
                txt.text = msg.text;
                // txt.color = MsgColor(msg.sender);
                newText.GetComponent<TextMeshProUGUI>().faceColor = MsgColor(msg.sender);

                // Changes the alignment depending on who send the message
                switch (msg.sender)
                {
                    case Message.Sender.npc:
                        txt.alignment = TextAlignmentOptions.Left;
                        break;
                    case Message.Sender.player:
                        txt.alignment = TextAlignmentOptions.Right;
                        break;
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