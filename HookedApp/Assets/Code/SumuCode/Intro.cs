using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] Image fade;
    [SerializeField] TMP_Text[] introTexts;
    int tutorialCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!Stats.IsNewGame())
        {
            Destroy(fade.gameObject);
            return;
        }

        Time.timeScale = 0;
        if (introTexts.Length >= 1)
        {
            for (int i = 1; i < introTexts.Length; i++)
            {
                introTexts[i].gameObject.SetActive(false);
            }
        }

        if (Stats.language == "fi")
        {
            introTexts[0].text = "Tässä pelissä tehtäväsi on verkostoitua mahdollisimman kattavasti!";
            introTexts[1].text = "Vastaa viesteihin ja tapahtumakutsuihin parhaan harkintakykysi mukaan";
        }
        else
        {
            introTexts[0].text = "In this game your goal is to connect with as many people as you can!";
            introTexts[1].text = "Answer to various messages and event invites and get to know a lot of new people";
        }
    }

    public void TapScreen()
    {
        tutorialCount++;
        if (tutorialCount >= introTexts.Length)
        {
            StartCoroutine(FadeToGame());
        } else
        {
            introTexts[tutorialCount].gameObject.SetActive(true);
        }
    }

    private IEnumerator FadeToGame()
    {
        Time.timeScale = 1;
        foreach (TMP_Text txt in introTexts)
        {
            txt.gameObject.SetActive(false);
        }
        fade.GetComponent<Button>().interactable = false;

        float timer = 0;
        float duration = 1;
        float alpha = fade.color.a;
        while (timer <= duration)
        {
            timer += Time.deltaTime * 2;
            fade.color = new Color(0, 0, 0, alpha - timer);
            yield return null;
        }
        fade.color = new Color(0, 0, 0, 0);
        Destroy(fade.gameObject);
    }
}
