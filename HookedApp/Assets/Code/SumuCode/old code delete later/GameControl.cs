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
    public static string lastGameState;
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
            Debug.Log("New game started");
            // saveLoad.StartFresh();
            gameState = "Megismarko1";
        }
        // StartCoroutine(uiControl.RefreshReferences());

        Debug.Log("Checking for saves");
        if (gameState == null)
        {
            gameState = "Megismarko1";
        }
        else
        {
            messageControl.SetConversationHistory(conversationHistory);
            events.LoadUnlockedEvents(eventHistory);
        }

        StartCoroutine(ReadFilesFirst());
        //GameState(gameState);
    }

    // controls game flow
    public void GameState(string state)
    {
        // print("Game state: " + state);
        gameState = state;
        switch (state)
        {
            case "Megismarko1":
                Debug.Log("Start game");
                lastGameState = state;
                string characterName = "Megismarko";
                uiControl.AddConvoButton(characterName);
                StartDialogue(characterName);
                GameState("Portsari");
                break;

            case "Megismarko2":
                lastGameState = state;
                characterName = "Megismarko";
                messageControl.SendMessage("ChangeConversation", characterName);
                characterName += " 2";
                StartDialogue(characterName);
                break;

            case "Portsari":
                characterName = "Portsari";
                uiControl.AddConvoButton(characterName);
                StartDialogue(characterName);
                break;

            case "Portsari2":
                lastGameState = state;
                characterName = "Portsari";
                messageControl.SendMessage("ChangeConversation", characterName);
                characterName += " 2";
                StartDialogue(characterName);
                break;

            case "Bilekutsu":
                lastGameState = state;
                uiControl.AddEventButton("Baari-ilta");
                break;

            case "Messukutsu":
                lastGameState = state;
                uiControl.AddEventButton("Työmessut");
                break;

            case "DJ":
                lastGameState = state;
                uiControl.AddEventButton("DJ ilta");
                break;

            case "Messut ja DJ":
                lastGameState = state;
                uiControl.AddEventButton("DJ ilta");
                uiControl.AddEventButton("Työmessut 2");
                break;

            case "Messut":
                lastGameState = state;
                uiControl.AddEventButton("Työmessut 2");
                break;

            case "Portsarikaipaa":
                lastGameState = state;
                characterName = "Portsari";
                messageControl.SendMessage("ChangeConversation", characterName);
                characterName += " 3";
                StartDialogue(characterName);
                break;

            case "Miranviesti":
                lastGameState = state;
                characterName = "Mira";
                uiControl.AddConvoButton(characterName);
                StartDialogue(characterName);
                break;

            case "Baari2":
                lastGameState = state;
                uiControl.AddEventButton("Baari-ilta 2");
                break;

            case "Haastis":
                lastGameState = state;
                Debug.Log("Ending: " + state);
                loader.SetFinalPoints(Stats.GetApproval());
                if (Stats.language == "fi")
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
                lastGameState = state;
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
                lastGameState = state;
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
        // uiControl.AddConvoButton(characterName);
        // messageControl.ChangeConversation(characterName);
        // messageControl.SendMessage("ChangeConversation", characterName);
        getDialogueByName(characterName).SetStateName(gameState);
        StartCoroutine(getDialogueByName(characterName).SendDelay());
    }

    // When a conversation ends, it comes back here
    // adds text to logs and starts the next phase
    public void EndPhase(string stateName)
    {
        Debug.Log("Phase ended: " +stateName);
        stateName = stateName.Trim();

        switch (stateName)
        {
                // test
            case "Megismarko1: Bilekutsu":
                logText += "Megismarko kutsui sinut bilettämään \n";
                logTextEnglish += "Megismarko has invited you to party \n";
                GameState("Bilekutsu");
                break;

                // test
            case "Megismarko2: Miranviesti":
                logText += "Annoit Megismarkolle luvan antaa numerosi Miralle \n";
                logTextEnglish += "You allowed Megismarko to give your number to Mira \n";
                GameState("Miranviesti");
                break;

                // test
            case "Megismarko2: Portsarikaipaa":
                logText += "Et suostunut bilettämään Megismarkon kanssa uudestaan \n";
                logTextEnglish += "You didn't want to party with Megismarko again \n";
                messageControl.SaveMessages();
                GameState("Portsarikaipaa");
                break;

            // test
            case "Megismarko2: Baari2":
                logText += "Megismarko kutsui sinut uudestaan bilettämään \n";
                logTextEnglish += "Megismarko has invited you to a party again \n";
                messageControl.SaveMessages();
                GameState("Baari2");
                break;

            case "Portsari: Messukutsu":
                logText += "Portsari kutsui sinut työmessuille \n";
                logTextEnglish += "Portsari has invited you to a work fair \n";
                GameState("Messukutsu");
                break;

            case "Portsari2: Messut ja DJ":
                logText += "Portsari on lähettänyt sinulle kutsun uusille messuille ja DJ:n keikalle \n";
                logTextEnglish += "Portsari has invited you to a work fair and a DJ's gig \n";
                GameState("Messut ja DJ");
                break;

            case "Portsari2: DJ":
                logText += "Portsari on kutsunut sinut DJ:n keikalle \n";
                logTextEnglish += "Portsari has invited you to a DJ's gig \n";
                GameState("DJ");
                break;

            case "Portsari2: Shutin":
                GameState("Shutin");
                break;

            case "Portsari3: Messut":
                logText += "Portsari on kutsunut sinut työmessuille \n";
                logTextEnglish += "Portsari has invited you to a work fair \n";
                GameState("Messut");
                break;

            case "Portsari3: Shutin":
                GameState("Shutin");
                break;

            case "Mira1: DJ":
                logText += "Mira kutsui sinut DJ:n keikalle \n";
                logTextEnglish += "Mira has invited you to a DJ's gig \n";
                GameState("DJ");
                break;

            case "Mira1: Messut":
                logText += "Mira kutsui sinut työmessuille \n";
                logTextEnglish += "Mira has invited you to a work fair \n";
                GameState("Messut");
                break;

            case "Mira1: Shutin":
                GameState("Shutin");
                break;

            case "Yes:Työmessut":
                logText += "Lähdit työmessuille. Siellä tapahtui kaikenlaista kivaa. \n";
                logTextEnglish += "You went to a work fair. There happened lots of great things \n";
                eventOneState = EventInviteState.Going;
                GameState("Portsari2");
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
                GameState("Megismarko2");
                break;

            case "No:Baari-ilta":
                logText += "Kieltäydyit baari-illasta \n";
                logTextEnglish += "You said no to a fun bar night \n";
                eventTwoState = EventInviteState.NotGoing;
                if (eventOneState == EventInviteState.NotGoing)
                {
                    GameState("Shutin");
                }
                break;

            case "Yes:Työmessut 2":
                logText += "Menit työmessuille. Olet saanut uusia ystäviä. \n";
                logTextEnglish += "You went to a workfair. You got new friends. \n";
                eventTwoState = EventInviteState.Going;
                GameState("Haastis");
                break;

            case "No:Työmessut 2":
                logText += "Kieltäydyit työmessuista \n";
                logTextEnglish += "You said no to the epic work fair 2 \n";
                eventTwoState = EventInviteState.NotGoing;
                if (eventOneState == EventInviteState.NotGoing)
                {
                    GameState("Huonoloppu");
                }
                break;

            case "Yes:DJ ilta":
                logText += "Lähdit kuuntelemaan DJ:n keikkaa. Sinulla oli hyvä meno! \n";
                logTextEnglish += "You wen to a DJ's gig. You had fun! \n";
                eventTwoState = EventInviteState.Going;
                GameState("Huonoloppu");
                break;

            case "No:DJ ilta":
                logText += "Kieltäydyit DJ:n keikasta. \n";
                logTextEnglish += "You refused to got to a DJ's gig \n";
                eventTwoState = EventInviteState.NotGoing;
                if (eventOneState == EventInviteState.NotGoing)
                {
                    GameState("Shutin");
                }
                break;

            case "Yes:Baari-ilta 2":
                logText += "Lähdit baariin. Taas! \n";
                logTextEnglish += "You went to a bar again \n";
                eventTwoState = EventInviteState.Going;
                GameState("Huonoloppu");
                break;

            case "No:Baari-ilta 2":
                logText += "Et mennyt baariin. \n";
                logTextEnglish += "You didn't go to a bar this time \n";
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

    IEnumerator ReadFilesFirst()
    {
        foreach(Choices ch in choices)
        {
            ch.LoadFromFile();
            yield return null;
        }

        GameState(gameState);
    }
}
