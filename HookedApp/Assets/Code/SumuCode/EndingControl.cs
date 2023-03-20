using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingControl : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Loader") != null)
        {
            int finalPoints = GameObject.Find("Loader").GetComponent<LevelLoader>().GetFinalPoints();
            pointsText.text = "And you got " + finalPoints + " approval points!";
        }
    }

    public void BackToMenu()
    {
        if (GameObject.Find("Loader") != null)
        {
            GameObject.Find("Loader").GetComponent<LevelLoader>().LoadLevel("StartMenu");
        } 
        else
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}
