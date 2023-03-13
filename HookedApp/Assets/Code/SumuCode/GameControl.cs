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

    private void Start()
    {
        // TESTING
        GameState("Pekka convo 1");
    }

    private void Update()
    {

    }

    // Controls the game flow
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
                characterName = "Pekka";
                messageControl.ChangeConversation(characterName);
                characterName += " 2";
                getDialogueByName(characterName).SetStateName(state);
                StartCoroutine(getDialogueByName(characterName).SendDelay());
                break;

            case "Test event invite":
                uiControl.AddEventButton("Test event");
                Debug.Log("Testi invite");
                break;
        }
    }

    // When a conversation ends, it comes back here
    public void EndPhase(string stateName)
    {
        Debug.Log("Phase '" + stateName + "' has ended");

        switch(stateName)
        {
            case "Pekka convo 1":
                GameState("Pekka convo 2");
                break;

            case "Pekka convo 2":
                GameState("Test event invite");
                break;

            case "Yes:Test event":
                Debug.Log("Game ends, you said yes");
                break;

            case "No:Test event":
                Debug.Log("Game ends, you said no");
                break;
        }
    }

    // Fetches a dialogue tree by name
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
