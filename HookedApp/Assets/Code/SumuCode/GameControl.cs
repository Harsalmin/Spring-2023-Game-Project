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
    EventInviteState eventOneState = EventInviteState.Unanswered;
    EventInviteState eventTwoState = EventInviteState.Unanswered;

    public enum EventInviteState
    {
        Unanswered,
        Going,
        NotGoing
    }

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
            gameState = "Megismarko convo";
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
            case "Megismarko convo":
                string characterName = "Megismarko";
                StartDialogue(characterName);
                break;

            case "Portsarinviesti":
                characterName = "Portsari";
                StartDialogue(characterName);
                break;

            case "Tapahtumakutsut":
                uiControl.AddEventButton("Ty�messut");
                uiControl.AddEventButton("Baari-ilta");
                break;

            case "Kolmashahmo":
                characterName = "Kolmashahmo";
                StartDialogue(characterName);
                break;

            case "Nelj�shahmo":
                characterName = "Nelj�shahmo";
                StartDialogue(characterName);
                break;

            case "Haastis":
                Debug.Log("Ending: " + state);
                loader.SetFinalPoints(stats.GetApproval());
                loader.LoadLevel("Ending");
                break;

            case "Huonoloppu":
                Debug.Log("Ending: " + state);
                loader.SetFinalPoints(stats.GetApproval());
                loader.LoadLevel("Ending");
                break;

            case "Shutin":
                Debug.Log("Ending: " + state);
                loader.SetFinalPoints(stats.GetApproval());
                loader.LoadLevel("Ending");
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
            case "Megismarko convo: Portsarinviesti":
                logText += "Sanoit Megismarkolle kyll� \n";
                GameState("Portsarinviesti");
                break;

                // test
            case "Megismarko convo: Portsarinviesti2":
                logText += "Sanoit Megismarkolle ei \n";
                GameState("Portsarinviesti");
                break;

                // test
            case "Portsarinviesti: Tapahtumakutsut":
                logText += "Sanoit Portsarille kyll� \n";
                messageControl.SaveMessages();
                GameState("Tapahtumakutsut");
                break;

            // test
            case "Portsarinviesti: Tapahtumakutsut2":
                logText += "Sanoit Portsarille ei \n";
                messageControl.SaveMessages();
                GameState("Tapahtumakutsut");
                break;

            case "Yes:Ty�messut":
                logText += "L�hdit ty�messuille. Siell� tapahtui kaikenlaista kivaa. \n";
                eventOneState = EventInviteState.Going;
                GameState("Kolmashahmo");
                break;

            case "No:Ty�messut":
                logText += "Et l�htenyt ty�messuille";
                eventOneState = EventInviteState.NotGoing;
                if(eventTwoState == EventInviteState.NotGoing)
                {
                    GameState("Shutin");
                }
                break;

            case "Yes:Baari-ilta":
                logText += "L�hdit baariin. Siell� tapahtui kaikenlaista kivaa. \n";
                eventTwoState = EventInviteState.Going;
                GameState("Nelj�shahmo");
                break;

            case "No:Baari-ilta":
                logText += "Kielt�ydyit baari-illasta";
                eventTwoState = EventInviteState.NotGoing;
                if (eventOneState == EventInviteState.NotGoing)
                {
                    GameState("Shutin");
                }
                break;

            case "Kolmashahmo: Haastis":
            case "Nelj�shahmo: Haastis":
                GameState("Haastis");
                break;

            case "Kolmashahmo: Huonoloppu":
            case "Nelj�shahmo: Huonoloppu":
                GameState("Huonoloppu");
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
