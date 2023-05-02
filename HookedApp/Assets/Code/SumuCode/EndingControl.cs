using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingControl : MonoBehaviour
{
    [SerializeField] private TMP_Text title, description, pointsText;
    [SerializeField] private Image fade;
    [SerializeField] private TMP_Text promptText, yes, no;
    [SerializeField] private Animator promptAnim;
    [SerializeField] private AudioSource goodEndingSound, badEndingSound;
    int finalPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Makes sure that starting a new game doesn't load the game instead
        Stats.SetNewGame(true);

        /*
        if (GameObject.Find("Loader") != null)
        {
            finalPoints = GameObject.Find("Loader").GetComponent<LevelLoader>().GetFinalPoints();
        }*/

        if(Stats.language == "fi")
            pointsText.text = "Sinä sait " + Stats.GetApproval() + " tähtipistettä!";
        else
            pointsText.text = "And you got " + Stats.GetApproval() + " approval points!";

        title.text = Stats.GetEndCreditsTitle();
        description.text = Stats.GetEndCreditsDesc();

        // Plays either the good ending sound or the bad ending sound
        if(title.text.StartsWith("Good") || title.text.StartsWith("Hyvä"))
        {
            goodEndingSound.Play();
        }
        else
        {
            badEndingSound.Play();
        }
    }

    public void FadePopup(bool fadeIn)
    {
        if (fadeIn)
        {
            if (Stats.language == "fi")
            {
                promptText.text = "Haluatko varmasti lopettaa?";
                yes.text = "Kyllä";
                no.text = "Ei";
            }
            else
            {
                promptText.text = "Are you sure you want to quit?";
                yes.text = "Yes";
                no.text = "No";
            }
            StartCoroutine(FadeInPopUp());
        }
        else
        {
            StartCoroutine(FadeOutPopUp());
        }
    }

    public IEnumerator FadeInPopUp()
    {
        float timer = 0;
        float duration = 0.5f;
        while (timer <= duration)
        {
            timer += Time.deltaTime * 3;
            fade.color = new Color(0, 0, 0, timer);
            yield return null;
        }
        fade.color = new Color(0, 0, 0, 0.5f);
        promptAnim.SetTrigger("show");
    }

    public IEnumerator FadeOutPopUp()
    {
        promptAnim.SetTrigger("hide");
        float timer = 0;
        float duration = 0.5f;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            fade.color = new Color(0, 0, 0, 1 - timer);
            yield return null;
        }
        fade.color = new Color(0, 0, 0, 0);
    }

    public void BackToMenu()
    {
        if (GameObject.Find("LevelLoader") != null)
        {
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel("StartMenu");
        } 
        else
        {
            Stats.SetNewGame(true);
            SceneManager.LoadScene("StartMenu");
        }
    }

    public void ExitGame()
    {
        FadePopup(true);
    }

    public void ActuallyExit()
    {
        Application.Quit();
    }
}
