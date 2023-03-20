using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private int approval;

    public void AddPoints(int amount)
    {
        Debug.Log(amount + " points added");
        approval += amount;
    }

    public void RemovePoints(int amount)
    {
        Debug.Log(amount + " points removed");
        approval -= amount;
    }

    public int GetApproval()
    {
        return approval;
    }

    public void SetApproval(int amount)
    {
        approval = amount;
    }
}
