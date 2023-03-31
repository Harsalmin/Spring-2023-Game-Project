using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPopupAnimator : MonoBehaviour
{
    UIcontrol uIcontrol;
    // Start is called before the first frame update
    void Start()
    {
        uIcontrol = FindObjectOfType<UIcontrol>();
    }

    public void AnimationPlayed()
    {
        uIcontrol.FadePopup(false);
    }
}
