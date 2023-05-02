using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationSound : MonoBehaviour
{
    [SerializeField] AudioSource notificationSound;

    public void PlaySound()
    {
        notificationSound.Play();
    }
}
