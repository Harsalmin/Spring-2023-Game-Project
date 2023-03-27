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
    public GameObject viewport, textObj, buttonObj, convoObj;
    private GameObject messageContainer;
    [SerializeField] private Color npcColor, playerColor;
    List<GameObject> buttons = new List<GameObject>();
    private int index = 0;
    UIcontrol uiControl;

    private const string CHARACTERINDICATOR = "Character_";
    // private const string CONVERSATIONINDICATOR = "Conversation_";

    private void Start()
    {
        uiControl = GetComponent<UIcontrol>();
    }

    // Sends a message to the chat window
    public void AddMessage(string text, Message.Sender sender)
    {
        index++;
        Message newMessage = new Message();
        newMessage.id = index;
        newMessage.text = text;
        newMessage.sender = sender;
        GameObject newText = Instantiate(textObj, messageContainer.transform);
        TMP_Text txt = newText.GetComponent<TMP_Text>();
        txt.text = newMessage.text;
        //txt.color = MsgColor(sender);
        newText.GetComponent<TextMeshProUGUI>().faceColor = MsgColor(newMessage.sender);

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
    }

    // Adds a button and a listener
    public void AddButton(string text, int number, Choices choiceScript)
    {
        GameObject newButton = Instantiate(buttonObj, messageContainer.transform);
        newButton.GetComponentInChildren<TMP_Text>().text = text;
        newButton.GetComponent<Button>().onClick.AddListener(delegate { ButtonClick(number, choiceScript); });
        buttons.Add(newButton);
    }

    public void ButtonClick(int number, Choices choiceScript)
    {
        choiceScript.ButtonClicked(number);

        // Destroys all buttons
        foreach(GameObject g in buttons)
        {
            Destroy(g);
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
        // create dictionary if it doesn't already exist
        if (conversationHistory == null)
        {
            conversationHistory = new Dictionary<string, List<Message>>();
        }

        // add current npc to the dictionary if it isn't there already
        if (!conversationHistory.ContainsKey(currentNpc))
        {
            conversationHistory.Add(currentNpc, new List<Message>());
        }

        foreach (Message msg in messages)
        {
            conversationHistory[currentNpc].Add(msg);
        }
    }

    // Changes which conversation is active currently
    public void ChangeConversation(string characterName)
    {
        print("conversation: " + characterName);
        if (currentNpc != null)
        {
            SaveMessages();
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
        return conversationHistory;
    }

    // Loads all previous conversation history
    public void SetConversationHistory(Dictionary<string, List<Message>> convoHistory)
    {
        if(convoHistory == null)
        {
            return;
        }

        conversationHistory = convoHistory;
        foreach(string s in conversationHistory.Keys.ToList())
        {
            uiControl.AddConvoButton(s);
            ChangeConversation(s);
            List<int> processedIDs = new List<int>();
            foreach (Message msg in conversationHistory[s])
            {
                // prevents duplicates
                if (processedIDs.Contains(msg.id))
                {
                    continue;
                }
                processedIDs.Add(msg.id);

                GameObject newText = Instantiate(textObj, messageContainer.transform);
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

    public enum Sender
    {
        npc,
        player
    }
}