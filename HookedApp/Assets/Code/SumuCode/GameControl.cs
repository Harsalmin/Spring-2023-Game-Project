using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    UIcontrol uiControl;
    MessageControl messageControl;
    Choices[] choices;
    LevelLoader loader;
    SaveLoad saveLoad;
    EventManager events;
    // Stats stats;
    private static string gameState;
    public static Dictionary<string, List<Message>> conversationHistory;
    public static List<Event> eventHistory;
    public static string logText, logTextEnglish;
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
        saveLoad = GetComponent<SaveLoad>();
        events = GetComponent<EventManager>();
        // stats = GetComponent<Stats>();
    }

    private void Start()
    {
        if(Stats.IsNewGame())
        {
            saveLoad.StartFresh();
            gameState = "Megismarko convo";
        }
        // StartCoroutine(uiControl.RefreshReferences());

        if (gameState == null)
        {
            gameState = "Megismarko convo";
        }
        else
        {
            messageControl.SetConversationHistory(conversationHistory);
            events.LoadUnlockedEvents(eventHistory);
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
                uiControl.AddEventButton("Työmessut");
                uiControl.AddEventButton("Baari-ilta");
                break;

            case "Kolmashahmo":
                characterName = "Kolmashahmo";
                StartDialogue(characterName);
                break;

            case "Neljäshahmo":
                characterName = "Neljäshahmo";
                StartDialogue(characterName);
                break;

            case "Haastis":
                Debug.Log("Ending: " + state);
                loader.SetFinalPoints(Stats.GetApproval());
                if(Stats.language == "fi")
                {
                    Stats.ChangeEndCredits(
                        "Peli loppui!",
                        "Sait paljon työhaastatteluita! Jee"
                        );
                }
                else
                {
                    Stats.ChangeEndCredits(
                        "Game over!",
                        "You got a ton of job interviews! Yay"
                        );
                }
                // gameState = "Megismarko convo";
                loader.LoadLevel("Ending");
                break;

            case "Huonoloppu":
                Debug.Log("Ending: " + state);
                loader.SetFinalPoints(Stats.GetApproval());
                if (Stats.language == "fi")
                {
                    Stats.ChangeEndCredits(
                        "Peli loppui!",
                        "Sait huonon lopun :/"
                        );
                }
                else
                {
                    Stats.ChangeEndCredits(
                        "Game over!",
                        "You got a bad ending :/"
                        );
                }
                loader.LoadLevel("Ending");
                break;

            case "Shutin":
                Debug.Log("Ending: " + state);
                loader.SetFinalPoints(Stats.GetApproval());
                if (Stats.language == "fi")
                {
                    Stats.ChangeEndCredits(
                        "Peli loppui!",
                        "Et mennyt mihinkään! Pysyit kotona! Et saanut työhaastatteluita!"
                        );
                }
                else
                {
                    Stats.ChangeEndCredits(
                        "Game over!",
                        "You stayed at home! That's no way to get job interviews!"
                        );
                }
                loader.LoadLevel("Ending");
                break;
        }
    }

    void StartDialogue(string characterName)
    {
        uiControl.AddConvoButton(characterName);
        // messageControl.ChangeConversation(characterName);
        messageControl.SendMessage("ChangeConversation", characterName);
        getDialogueByName(characterName).SetStateName(gameState);
        StartCoroutine(getDialogueByName(characterName).SendDelay());
    }

    // When a conversation ends, it comes back here
    public void EndPhase(string stateName)
    {
        Debug.Log("Phase ended: " +stateName);
        stateName = stateName.Trim();

        switch (stateName)
        {
                // test
            case "Megismarko convo: Portsarinviesti":
                logText += "Sanoit Megismarkolle kyllä \n";
                logTextEnglish += "You said yes to Megismarko \n";
                GameState("Portsarinviesti");
                break;

                // test
            case "Megismarko convo: Portsarinviesti2":
                logText += "Sanoit Megismarkolle ei \n";
                logTextEnglish += "You said no to Megismarko \n";
                GameState("Portsarinviesti");
                break;

                // test
            case "Portsarinviesti: Tapahtumakutsut":
                logText += "Sanoit Portsarille kyllä \n";
                logTextEnglish += "You said yes to Portsari \n";
                messageControl.SaveMessages();
                GameState("Tapahtumakutsut");
                break;

            // test
            case "Portsarinviesti: Tapahtumakutsut2":
                logText += "Sanoit Portsarille ei \n";
                logTextEnglish += "You said no to Portsari \n";
                messageControl.SaveMessages();
                GameState("Tapahtumakutsut");
                break;

            case "Yes:Työmessut":
                logText += "Lähdit työmessuille. Siellä tapahtui kaikenlaista kivaa. \n";
                logTextEnglish += "You went to a work fair. There happened lots of great things \n";
                eventOneState = EventInviteState.Going;
                GameState("Kolmashahmo");
                break;

            case "No:Työmessut":
                logText += "Et lähtenyt työmessuille";
                logTextEnglish += "You didn't go to the work fair \n";
                eventOneState = EventInviteState.NotGoing;
                if(eventTwoState == EventInviteState.NotGoing)
                {
                    GameState("Shutin");
                }
                break;

            case "Yes:Baari-ilta":
                logText += "Lähdit baariin. Siellä tapahtui kaikenlaista kivaa. \n";
                logTextEnglish += "You went to a bar. You had fun \n";
                eventTwoState = EventInviteState.Going;
                GameState("Neljäshahmo");
                break;

            case "No:Baari-ilta":
                logText += "Kieltäydyit baari-illasta";
                logTextEnglish += "You said no to a fun bar night \n";
                eventTwoState = EventInviteState.NotGoing;
                if (eventOneState == EventInviteState.NotGoing)
                {
                    GameState("Shutin");
                }
                break;

            case "Kolmashahmo: Haastis":
            case "Neljäshahmo: Haastis":
                GameState("Haastis");
                break;

            case "Kolmashahmo: Huonoloppu":
            case "Neljäshahmo: Huonoloppu":
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

    public void ChangeLanguage(bool finnish)
    {
        if (finnish)
        {
            Stats.language = "fi";
        }
        else
        {
            Stats.language = "en";
        }
        messageControl.languageChanged = true;

        // changes language and loads from file
        foreach (Choices ch in choices)
        {
            ch.LoadFromFile();
        }

        uiControl.ChangeLanguage();
        GameState(gameState);
    }
}
