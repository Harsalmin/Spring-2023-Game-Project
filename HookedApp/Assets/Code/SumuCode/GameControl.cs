using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    UIcontrol uiControl;
    MessageControl messageControl;
    Choices[] choices;

    private void Awake()
    {
        uiControl = GetComponent<UIcontrol>();
        messageControl = GetComponent<MessageControl>();
        choices = GetComponents<Choices>();
    }

    private void Update()
    {
        // FOR TESTING ONLY
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameState("Pekka convo 1");
        }
    }

    public void GameState(string state)
    {
        switch(state)
        {
            case "Pekka convo 1":
                string characterName = "Pekka";
                uiControl.AddConvoButton(characterName);
                messageControl.ChangeConversation(characterName);
                getDialogueByName(characterName).SetStateName(state);
                StartCoroutine(getDialogueByName(characterName).SendDelay());
                break;

            case "Pekka convo 2":
                break;

            case "Test event invite":
                break;
        }
    }

    public void EndConversation(string stateName)
    {
        Debug.Log("Phase '" + stateName + "' has ended");
    }

    private Choices getDialogueByName(string name)
    {
        foreach(Choices ch in choices)
        {
            if(ch.characterName == name)
            {
                return ch;
            }
        }
        return null;
    }
}
