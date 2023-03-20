using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    GameControl control;
    Stats stats;
    private const string STATE = "State";
    private const string APPROVALPOINTS = "Approval";

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<GameControl>();
        stats = GetComponent<Stats>();
    }

    public void SaveGame()
    {
        string gameState = control.GetGameState();
        PlayerPrefs.SetString(STATE, gameState);

        int approval = stats.GetApproval();
        PlayerPrefs.SetInt(APPROVALPOINTS, approval);
    }

    public void LoadGame()
    {
        string gameState = PlayerPrefs.GetString(STATE);
        control.GameState(gameState);

        int approval = PlayerPrefs.GetInt(APPROVALPOINTS);
        stats.SetApproval(approval);
    }
}
