using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class cardMetadata : MonoBehaviour
{

    public string slug;

    public string tokenID;

    public string name;

    [SerializeField]
    TextMeshPro m_MetadataTitleView;

    public TextMeshPro MetadataTitleView
    {
        get { return m_MetadataTitleView; }
        set { m_MetadataTitleView = value; }
    }

    [SerializeField]
    TextMeshPro m_MetadataDetailView;
    public TextMeshPro MetadataDetailView
    {
        get { return m_MetadataDetailView; }
        set { m_MetadataDetailView = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        MetadataTitleView.SetText($"Name:{name}");
        MetadataDetailView.SetText($"TokenID:{tokenID}");
    }
}
