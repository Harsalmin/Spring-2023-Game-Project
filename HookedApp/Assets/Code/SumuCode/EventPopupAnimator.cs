using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPopupAnimator : MonoBehaviour
{
    EventControl eventControl;
    // Start is called before the first frame update
    void Start()
    {
        eventControl = FindObjectOfType<EventControl>();
    }

    public void AnimationPlayed()
    {
        eventControl.FadePopup(false);
    }
}
