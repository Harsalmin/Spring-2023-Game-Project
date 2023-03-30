using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private static int approval;
    private static bool newGame = false;
    public static string language = "fi";

    public static void AddPoints(int amount)
    {
        Debug.Log(amount + " points added");
        approval += amount;
    }

    public static void RemovePoints(int amount)
    {
        Debug.Log(amount + " points removed");
        approval -= amount;
    }

    public static int GetApproval()
    {
        return approval;
    }

    public static void SetApproval(int amount)
    {
        approval = amount;
    }

    public static void SetNewGame(bool setNewGame)
    {
        newGame = setNewGame;
    }

    public static bool IsNewGame()
    {
        return newGame;
    }
}
