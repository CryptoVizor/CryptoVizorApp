using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hideUIButton : MonoBehaviour
{

    private bool UI = true;

    public Image rendered;
    public Sprite iconShow;
    public Sprite iconHide;

    public List<GameObject> canvas; 
    // Start is called before the first frame update
    public void Click()
    {
        updateIcon();
        UI = !UI;
        foreach (var item in canvas)
        {
            item.SetActive(UI);
        }
    }

    private void updateIcon(){
        if (UI)
        {
            rendered.sprite = iconHide;
        } else {
            rendered.sprite = iconShow;
        }
    }
}
