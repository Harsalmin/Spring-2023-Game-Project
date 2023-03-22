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
    private static string gameState;
    public static Dictionary<string, List<Message>> conversationHistory;
    public static string logText;

    private void Awake()
    {
        uiControl = GetComponent<UIcontrol>();
        messageControl = GetComponent<MessageControl>();
        choices = GetComponents<Choices>();
        loader = GameObject.Find("Loader").GetComponent<LevelLoader>();
        stats = GetComponent<Stats>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(uiControl.RefreshReferences());

        if (gameState == null)
        {
            gameState = "character.1 convo";
        }
        else
        {
            Debug.Log(conversationHistory.Count);
            messageControl.SetConversationHistory(conversationHistory);
        }

        GameState(gameState);
    }

    // Controls the game flow
    public void GameState(string state)
    {
        // print("Game state: " + state);
        gameState = state;
        switch (state)
        {
            case "character.1 convo":
                string characterName = "character.1";
                StartDialogue(characterName);
                break;

            case "character.2 convo":
                characterName = "character.2";
                StartDialogue(characterName);
                break;

            case "Test event invite":
                uiControl.AddEventButton("Test event");
                uiControl.AddEventButton("Test event 2");
                break;
        }
    }

    void StartDialogue(string characterName)
    {
        // print("start dialogue" + characterName);
        uiControl.AddConvoButton(characterName);
        // messageControl.ChangeConversation(characterName);
        messageControl.SendMessage("ChangeConversation", characterName);
        getDialogueByName(characterName).SetStateName(gameState);
        StartCoroutine(getDialogueByName(characterName).SendDelay());
    }

    // When a conversation ends, it comes back here
    public void EndPhase(string stateName)
    {
        // Debug.Log("Phase ended: " +stateName);
        stateName = stateName.Trim();

        switch (stateName)
        {
                // test
            case "character.1 convo: Testi1":
                logText += "You said yes to character.1 \n";
                GameState("character.2 convo");
                break;

                // test
            case "character.1 convo: Testi2":
                logText += "You said no to character.1 \n";
                GameState("character.2 convo");
                break;

                // test
            case "character.2 convo: Testi1":
                logText += "You said yes to character.2 \n";
                messageControl.SaveMessages();
                GameState("Test event invite");
                break;

            // test
            case "character.2 convo: Testi2":
                logText += "You said no to character.2 \n";
                messageControl.SaveMessages();
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

    public static string GetGameState()
    {
        return gameState;
    }

    public static void SetGameState(string state)
    {
        gameState = state;
    }
}
