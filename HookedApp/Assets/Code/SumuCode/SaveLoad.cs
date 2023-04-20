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

    private const string STATE = "State";
    private const string APPROVALPOINTS = "Approval";
    private const string EVENTCOUNT = "Event count";
    private const string EVENT = "Event";
    private const string SAVEEXISTS = "Save exists";

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<Control>();
        events = GetComponent<EventControl>();
    }

    public void SaveGame()
    {
        // Saves the fact that there is a save
        PlayerPrefs.SetInt(SAVEEXISTS, 1);
        control.ActivateLoadButton();

        // Adds the data
        GameData data = new GameData();
        data.approval = Stats.GetApproval();
        data.conversationHistory = control.GetConversationHistory();
        data.eventHistory = events.GetUnlockedEvents();
        data.logText = events.GetLogs();

        // Does the saving
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
            // Loads file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            control = GetComponent<Control>();
            events = GetComponent<EventControl>();

            // Unpacks the data to their correct places
            events.SetLogs(data.logText);
            events.SetUnlockedEvents(data.eventHistory);
            control.SetConversationHistory(data.conversationHistory);
            Stats.SetApproval(data.approval);
        }
        else
        {
            // Should never come here
            Debug.Log("File does not exist");
        }
    }

    // Notifies the stats that there is a save to load, and reloads the game scene
    public void StartLoading()
    {
        Stats.SetNewGame(false);
        LevelLoader loader = FindObjectOfType<LevelLoader>();
        loader.LoadLevel("Game");
    }
}

[System.Serializable]
class GameData
{
    public int approval;
    public Dictionary<string, List<SentMessage>> conversationHistory;
    public List<EventInfo> eventHistory;
    public string logText;
}