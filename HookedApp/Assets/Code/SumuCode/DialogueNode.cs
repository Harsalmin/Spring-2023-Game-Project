using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode
{
    public int Id { get; set; }
    public string Line { get; set; }

    public string Name { get; set; }

    public List<int> AnswerIds { get; set; }
    public List<string> Answers { get; set; }
    public List<int> Approval { get; set; }
}
