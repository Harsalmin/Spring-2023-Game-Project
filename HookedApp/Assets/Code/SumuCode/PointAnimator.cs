using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAnimator : MonoBehaviour
{
    UIcontrol uiControl;

    private void Start()
    {
        uiControl = FindObjectOfType<UIcontrol>();
    }

    public void AnimationEnded()
    {
        uiControl.UpdatePoints();
    }
}
