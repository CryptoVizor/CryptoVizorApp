using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionButton : MonoBehaviour
{

    void Awake()
    {
        var UnityMessageManager = GetComponent<UnityMessageManager>();
        UnityMessageManager.SendMessageToFlutter("overlay");
    }
    // Start is called before the first frame update
    public void Click()
    {
        var UnityMessageManager = GetComponent<UnityMessageManager>();
        UnityMessageManager.SendMessageToFlutter("overlay");
    }

}
