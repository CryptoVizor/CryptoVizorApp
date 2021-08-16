using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetCollectionMetadata : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI m_titleView;

    public TextMeshProUGUI titleView
    {
        get { return m_titleView; }
        set { m_titleView = value; }
    }

    [SerializeField]
    TextMeshProUGUI m_counterView;

    public TextMeshProUGUI counterView
    {
        get { return m_counterView; }
        set { m_counterView = value; }
    }
    
    public void onSetData(string title, int count){
        titleView.SetText(title);
        string countData = $"{count} items";
        if (count == 1)
        {
            countData = $"{count} item";
        }
        counterView.SetText(countData);
    }
}
