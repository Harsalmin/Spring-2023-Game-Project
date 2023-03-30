using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    GameControl control;
    // Stats stats;
    EventManager events;
    MessageControl messages;
    UIcontrol uiControl;
    LevelLoader levelLoader;

    private const string STATE = "State";
    private const string APPROVALPOINTS = "Approval";
    private const string EVENTCOUNT = "Event count";
    private const string EVENT = "Event";

    // Start is called before the first frame update
    void Start()
    {
        levelLoader = GameObject.Find("Loader").GetComponent<LevelLoader>();
        control = GetComponent<GameControl>();
        events = GetComponent<EventManager>();
        messages = GetComponent<MessageControl>();
        uiControl = GetComponent<UIcontrol>();
    }

    private void Update()
    {
        // FOR TESTING ONLY
        if (Input.GetKeyDown(KeyCode.A))
        {
            SaveGame();
        }

        // FOR TESTING ONLY
        if (Input.GetKeyDown(KeyCode.D))
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        Debug.Log("Saving");
        // gets the data
        GameData data = new GameData();
        data.gameState = GameControl.GetGameState();
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
          
            levelLoader.LoadLevel("SumuPlayground");
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
        events.ResetEvents();
        GameControl.logText = "";
        GameControl.SetGameState("Megismarko convo");
        Stats.SetNewGame(true);
    }
}

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
