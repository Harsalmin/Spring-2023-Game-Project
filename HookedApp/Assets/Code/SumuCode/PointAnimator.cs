using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAnimator : MonoBehaviour
{
    Control control;

    private void Start()
    {
        control = FindObjectOfType<Control>();
    }

    public void AnimationEnded()
    {
        control.UpdatePoints();
    }
}
