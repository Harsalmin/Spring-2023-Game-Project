using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Choices : MonoBehaviour
{
    private List<DialogueNode> dialogue = new List<DialogueNode>();
    [SerializeField] string filePath;
    public string characterName;
    private DialogueNode currDialogue;
    private Stats stats;
    private MessageControl messageControl;
    private GameControl gameControl;
    private string stateName;
    
    // Start is called before the first frame update
    void Start()
    {
        gameControl = GetComponent<GameControl>();
        stats = GetComponent<Stats>();
        messageControl = GetComponent<MessageControl>();
        LoadFromFile(filePath);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Changes the state name so the game control script knows where we're at
    public void SetStateName(string stateName)
    {
        this.stateName = stateName;
    }

    // Player has pressed a button in this conversation
    public void ButtonClicked(int btnNumber)
    {
        messageControl.AddMessage(currDialogue.Answers[btnNumber], Message.Sender.player);
        ChooseAnswer(btnNumber);
        StartCoroutine(SendDelay());
    }

    // Sends a message with a delay
    public IEnumerator SendDelay()
    {
        float delay = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(delay);
        messageControl.AddMessage(currDialogue.Line, Message.Sender.npc);

        // Automatically sends a new message if the npc double texts
        while (currDialogue.Answers[0] == "cont")
        {
            ChooseAnswer(0);
            delay = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(delay);
            messageControl.AddMessage(currDialogue.Line, Message.Sender.npc);

            // If the next line ends the conversation, break this loop
            if (currDialogue.Answers.Count == 0)
            {
                break;
            }
        }

        if (currDialogue.AnswerIds[0] == -1)
        {
            // Ends conversation if there is no dialogue left
            gameControl.EndPhase(stateName);
        }
        else
        {
            // Adds buttons if there is a choice to be made
            for (int i = 0; i < currDialogue.Answers.Count; i++)
            {
                messageControl.AddButton(currDialogue.Answers[i], i, this);
            }
        }
    }

    // Chooses an answer
    private void ChooseAnswer(int answer)
    {
        foreach (DialogueNode dn in dialogue)
        {
            if (dn.Id == currDialogue.AnswerIds[answer])
            {
                // If there is no answer, do not add points
                if (currDialogue.Answers[answer] != "cont")
                {
                    stats.AddPoints(currDialogue.Approval[answer]);
                }
                currDialogue = dn;
                return;
            }
        }
    }

    // Reads the text file containing information about this conversation
    public void LoadFromFile(string path)
    {
        using (StreamReader reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] parts = line.Split('\t');
                DialogueNode dn = new DialogueNode();

                // ID and the line said by the character
                dn.Id = int.Parse(parts[0]);
                dn.Line = parts[1];

                dn.AnswerIds = new List<int>();
                dn.Answers = new List<string>();
                dn.Approval = new List<int>();

                // Answers, their IDs and approval values
                int countToThree = 1;
                for (int i = 2; i < parts.Length; i++)
                {
                    switch(countToThree)
                    {
                        case 1:
                            // ID of the line this answer 'i' would lead to
                            dn.AnswerIds.Add(int.Parse(parts[i]));
                            break;
                        case 2:
                            // Answer line of the answer i
                            dn.Answers.Add(parts[i]);
                            break;
                        case 3:
                            // Approval value of the answer i
                            dn.Approval.Add(int.Parse(parts[i]));
                            break;
                    }

                    if (countToThree == 3)
                    {
                        countToThree = 1;
                    }
                    else
                    {
                        countToThree++;
                    }
                }
                dialogue.Add(dn);
            }
        }

        currDialogue = dialogue[0];
    }

    // Returns true if the current line of dialogue is the last one
    public bool EndDialogue()
    {
        return currDialogue.AnswerIds[0] == -1;
    }
}
