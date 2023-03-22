using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    GameControl control;
    Stats stats;
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
        stats = GetComponent<Stats>();
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
        data.approval = stats.GetApproval();
        data.conversationHistory = messages.GetConversationHistory();
        Debug.Log("Saved : " + data.conversationHistory.Count);
        data.eventHistory = events.GetUnlockedEvents();
        data.logText = GameControl.logText;

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
            stats.SetApproval(data.approval);

            if (data.conversationHistory != null)
            {
                GameControl.conversationHistory = data.conversationHistory;
            }
            events.LoadUnlockedEvents(data.eventHistory);
            GameControl.logText = data.logText;
            GameControl.SetGameState(data.gameState);
            levelLoader.LoadLevel("SumuPlayground");
            //control.GameState(data.gameState);
        }
        else
        {
            Debug.Log("File does not exist");
        }
    }
}

[System.Serializable] 
class GameData
{
    public string gameState;
    public int approval;
    public Dictionary<string, List<Message>> conversationHistory;
    public List<Event> eventHistory;
    public string logText;
}
