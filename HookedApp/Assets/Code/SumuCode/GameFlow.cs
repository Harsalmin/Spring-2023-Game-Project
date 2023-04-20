using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    // [SerializeField] private string fileName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void LoadFromFile()
    {
        TextAsset textData = Resources.Load("Story/" + Stats.language + "/" + fileName) as TextAsset;
        string txt = textData.text;
        var lines = txt.Split("\n");
        foreach(var line in lines)
        {
            string[] parts = line.Split("\t");
            GameNode gn = new GameNode();

            gn.Id = int.Parse(parts[0]);

            switch(parts[1])
            {
                case "M": 
                    gn.Type = NodeType.Message;
                    gn.Message = new MessageNode();
                    gn.Message.CharacterName = parts[2];
                    gn.Message.MessageText = parts[3];
                    gn.Message.Answers = new Dictionary<int, string>();
                    gn.Message.Answers.Add(int.Parse(parts[4]), parts[5]);
                    gn.Message.Approval = new Dictionary<int, int>();
                    gn.Message.Approval.Add(int.Parse(parts[4]), int.Parse(parts[6]));
                    gn.Message.Answers.Add(int.Parse(parts[7]), parts[8]);
                    gn.Message.Approval.Add(int.Parse(parts[7]), int.Parse(parts[9]));
                    break;

                case "E": 
                    gn.Type = NodeType.EventInvite;
                    gn.Event = new EventNode();
                    gn.Event.EventName = parts[2];
                    gn.Event.Description = parts[3];
                    gn.Event.Date = parts[4];
                    gn.Event.Approval = new Dictionary<int, int>();
                    gn.Event.Approval.Add(int.Parse(parts[5]), int.Parse(parts[6]));
                    gn.Event.Approval.Add(int.Parse(parts[7]), int.Parse(parts[8]));
                    break;

                case "L": 
                    gn.Type = NodeType.Log;
                    gn.LogText = parts[2];
                    gn.NextId = int.Parse(parts[3]);
                    break;

                case "X": 
                    gn.Type = NodeType.Ending;
                    gn.EndingText = parts[2];
                    break;
            }


        }
    }*/
}

[System.Serializable]
public class GameNode
{
    public int Id;
    public NodeType Type;
    public MessageNode Message;
    public EventNode Event;
    public string LogText;
    public int NextId;
    public string EndingText;
}

[System.Serializable]
public class MessageNode
{
    public string CharacterName;
    public string MessageText;
    public Dictionary<int, string> Answers;
    public Dictionary<int, int> Approval;
}

[System.Serializable]
public class EventNode
{
    public string EventName;
    public string Description;
    public string Date;
    public Dictionary<int, int> Approval;
}

[System.Serializable]
public enum NodeType
{
    Message,
    EventInvite,
    Log,
    Ending
}