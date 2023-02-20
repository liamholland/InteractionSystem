using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public Text notificationText;

    void Start()
    {
        notificationText.enabled = false;
    }

    public void DisplayNotification(string text)
    {
        notificationText.text = text;
        notificationText.enabled = true;
        Invoke("HideText", 3f);
    }

    private void HideText()
    {
        notificationText.enabled = false;
    }
}
