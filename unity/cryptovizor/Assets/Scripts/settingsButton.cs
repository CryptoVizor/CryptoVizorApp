using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsButton : MonoBehaviour
{
    public void Click()
    {
        var UnityMessageManager = GetComponent<UnityMessageManager>();
        UnityMessageManager.SendMessageToFlutter("settings");
    }
}
