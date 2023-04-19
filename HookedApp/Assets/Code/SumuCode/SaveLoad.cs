using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    Control control;
    EventControl events;
    /*
    GameControl control;
    // Stats stats;
    EventManager events;
    MessageControl messages;
    UIcontrol uiControl;
    LevelLoader levelLoader;
    */
    private const string STATE = "State";
    private const string APPROVALPOINTS = "Approval";
    private const string EVENTCOUNT = "Event count";
    private const string EVENT = "Event";

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<Control>();
        events = GetComponent<EventControl>();
        /*
        levelLoader = GameObject.Find("Loader").GetComponent<LevelLoader>();
        control = GetComponent<GameControl>();
        events = GetComponent<EventManager>();
        messages = GetComponent<MessageControl>();
        uiControl = GetComponent<UIcontrol>();
        */
    }

    public void SaveGame()
    {
        GameData data = new GameData();
        data.approval = Stats.GetApproval();
        data.conversationHistory = control.GetConversationHistory();
        data.eventHistory = events.GetUnlockedEvents();
        data.logText = events.GetLogs();

        // does the saving
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            //loads file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            control = GetComponent<Control>();
            events = GetComponent<EventControl>();

            events.SetLogs(data.logText);
            events.SetUnlockedEvents(data.eventHistory);
            control.SetConversationHistory(data.conversationHistory);
            Stats.SetApproval(data.approval);
        }
        else
        {
            Debug.Log("File does not exist");
        }
    }

    public void StartLoading()
    {
        Stats.SetNewGame(false);
        LevelLoader loader = FindObjectOfType<LevelLoader>();
        loader.LoadLevel("Game");
    }
    
    /*
    public void SaveGame()
    {
        Debug.Log("Saving");
        // gets the data
        GameData data = new GameData();
        data.gameState = GameControl.lastGameState;
        data.approval = Stats.GetApproval();
        data.conversationHistory = messages.GetConversationHistory();
        data.eventHistory = events.GetUnlockedEvents();
        data.logText = GameControl.logText;
        data.logTextEnglish = GameControl.logTextEnglish;
        data.language = Stats.language;
        Debug.Log("saved in " + Stats.language);

        // does the saving
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadGame()
    {
        Debug.Log("Loading");
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            //loads file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            // sets the data 
            Stats.language = data.language;
            control.ChangeLanguage(Stats.language == "fi");
            Stats.SetApproval(data.approval);

            if (data.conversationHistory != null)
            {
                GameControl.conversationHistory = data.conversationHistory;
            }

            if(data.eventHistory != null)
            {
                GameControl.eventHistory = data.eventHistory;
            }

            GameControl.logText = data.logText;
            GameControl.logTextEnglish = data.logTextEnglish;
            GameControl.SetGameState(data.gameState);
            Stats.SetNewGame(false);
          
            levelLoader.LoadLevel("Game");
        }
        else
        {
            Debug.Log("File does not exist");
        }
    }

    public void StartFresh()
    {
        Debug.Log("New game start");
        Stats.SetApproval(0);
        GameControl.conversationHistory = null;
        GetComponent<EventManager>().ResetEvents();
        GameControl.logText = "";
        GameControl.SetGameState("Megismarko1");
        Stats.SetNewGame(true);
    }
    */
}

[System.Serializable]
class GameData
{
    public int approval;
    public Dictionary<string, List<SentMessage>> conversationHistory;
    public List<EventInfo> eventHistory;
    public string logText;
}

/*
[System.Serializable] 
class GameData
{
    public string gameState;
    public int approval;
    public string language;
    public Dictionary<string, List<Message>> conversationHistory;
    public List<Event> eventHistory;
    public string logText, logTextEnglish;
}
*/