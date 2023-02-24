using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageControl : MonoBehaviour
{
    List<Message> messages = new List<Message>();
    public GameObject messageContainer, textObj, buttonObj;
    [SerializeField] private Color npcColor, playerColor;
    List<GameObject> buttons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddMessage("This is a message from an npc", Message.Sender.npc);
            AddMessage("This is a message from the player", Message.Sender.player);
        }
    }

    // Sends a message to the chat window
    public void AddMessage(string text, Message.Sender sender)
    {
        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textObj, messageContainer.transform);
        newMessage.textObj = newText.GetComponent<TMP_Text>();
        newMessage.textObj.text = newMessage.text;
        newMessage.textObj.color = MsgColor(sender);

        // Changes the alignment depending on who send the message
        switch(sender)
        {
            case Message.Sender.npc:
                newMessage.textObj.alignment = TextAlignmentOptions.Left;
                break;
            case Message.Sender.player:
                newMessage.textObj.alignment = TextAlignmentOptions.Right;
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
        }
        return color;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public TMP_Text textObj;
    public Sender sender;

    public enum Sender
    {
        npc,
        player
    }
}