using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    UIcontrol uiControl;
    MessageControl messageControl;
    Choices[] choices;
    LevelLoader loader;
    Stats stats;
    private string gameState;
    private string logText;

    private void Awake()
    {
        uiControl = GetComponent<UIcontrol>();
        messageControl = GetComponent<MessageControl>();
        choices = GetComponents<Choices>();
        loader = GameObject.Find("Loader").GetComponent<LevelLoader>();
        stats = GetComponent<Stats>();
    }

    private void Start()
    {
        // TESTING
        GameState("Pekka convo");
    }

    private void Update()
    {

    }

    // Controls the game flow
    public void GameState(string state)
    {
        print("Game state: " + state);
        gameState = state;
        switch (state)
        {
            case "Pekka convo":
                string characterName = "Pekka";
                uiControl.AddConvoButton(characterName);
                messageControl.ChangeConversation(characterName);
                getDialogueByName(characterName).SetStateName(state);
                StartCoroutine(getDialogueByName(characterName).SendDelay());
                break;

            case "Timo convo":
                characterName = "Timo";
                uiControl.AddConvoButton(characterName);
                messageControl.ChangeConversation(characterName);
                getDialogueByName(characterName).SetStateName(state);
                StartCoroutine(getDialogueByName(characterName).SendDelay());
                break;

            case "Test event invite":
                uiControl.AddEventButton("Test event");
                uiControl.AddEventButton("Test event 2");
                Debug.Log("Testi invite");
                break;
        }
    }

    // When a conversation ends, it comes back here
    public void EndPhase(string stateName)
    {
        Debug.Log(stateName);
        stateName = stateName.Trim();

        switch (stateName)
        {
                // test
            case "Pekka convo: Testi1":
                logText += "You said yes to Pekka \n";
                GameState("Timo convo");
                break;

                // test
            case "Pekka convo: Testi2":
                logText += "You said no to Pekka \n";
                GameState("Timo convo");
                break;

                // test
            case "Timo convo: Testi1":
                logText += "You said yes to Timo \n";
                GameState("Test event invite");
                break;

            // test
            case "Timo convo: Testi2":
                logText += "You said no to Timo \n";
                GameState("Test event invite");
                break;

            case "Yes:Test event":
                Debug.Log("Game ends, you said yes to event 1");
                loader.SetFinalPoints(stats.GetApproval());
                loader.LoadLevel("Ending");
                break;

            case "No:Test event":
                Debug.Log("Game ends, you said no to event 1");
                loader.SetFinalPoints(stats.GetApproval());
                loader.LoadLevel("Ending");
                break;

            case "Yes:Test event 2":
                Debug.Log("Game ends, you said yes to event 2");
                loader.SetFinalPoints(stats.GetApproval());
                loader.LoadLevel("Ending");
                break;

            case "No:Test event 2":
                Debug.Log("Game ends, you said no to event 2");
                loader.SetFinalPoints(stats.GetApproval());
                loader.LoadLevel("Ending");
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

    public string GetGameState()
    {
        return gameState;
    }

    public void SetGameState(string state)
    {
        gameState = state;
    }

    public string GetLogs()
    {
        return logText;
    }

    public void SetLogs(string logs)
    {
        logText = logs;
    }
}
