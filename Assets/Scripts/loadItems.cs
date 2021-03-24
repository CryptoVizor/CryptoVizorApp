using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class loadItems : MonoBehaviour, loadInterface
{

    public string Title;
    public string data;

    private string previousSlug;

    public int count{
    get;
    set;
  }

    public int page{
    get;
    set;
  }

    public GameObject cardPrefab;

    public GameObject ScrollViewGameObject;

    public GetTokensData canvasData;

    public UnityEngine.XR.Interaction.Toolkit.AR.CustomPlacing placementLogic;

    public GameObject scroll;

    public GameObject mask;

    [SerializeField]
    TextMeshProUGUI m_titleView;

    /// <summary>
    /// The UI Text element used to display plane detection messages.
    /// </summary>
    public TextMeshProUGUI titleView
    {
        get { return m_titleView; }
        set { m_titleView = value; }
    }
    
    // Start is called before the first frame update
    public async void Click()
    {
    pageLoader loader = ScrollViewGameObject.GetComponent<pageLoader>();
    loader.updatePageNumberLoading();
    Debug.Log("Click");

    Tokensdata tokensData = new Tokensdata();
    foreach (Transform child in ScrollViewGameObject.transform)
    {
      if (child.gameObject.tag != "UIOnly")
      {
        GameObject.Destroy(child.gameObject);
      }
      else
      {
        SetCollectionMetadata metadata = child.GetComponent<SetCollectionMetadata>();
        metadata.onSetData(Title, count);
      }
    }
    ScrollViewGameObject.transform.parent.parent.gameObject.SetActive(true);
    await GetTokens(data, assetTokens => tokensData = assetTokens);

      foreach (var tokenAssetData in tokensData.assets)
      {
      bool placed = false;
      if (canvasData.placedTokens.ContainsKey(data))
      {
        if (canvasData.placedTokens[data].ContainsKey(tokenAssetData.token_id))
        {
          if (canvasData.placedTokens[data][tokenAssetData.token_id])
          {
            placed = true;
          }
        }
      }
      MetaData tokenData = new MetaData();
      tokenData.image = tokenAssetData.image_url;
      tokenData.name = tokenAssetData.name;
      setItemData(tokenData, tokenAssetData.token_id,placed);
      loader.buttonLoader = this;
      loader.updatePageNumber();
      }
    }

  private void setItemData(MetaData tokensData, string tokenID, bool placed){
    Debug.Log($"tokenID {tokenID}");
    GameObject card = Instantiate(cardPrefab) as GameObject;
    DownloadSprite cardSprite = card.GetComponent<DownloadSprite>();
    cardSprite.URI = tokensData.image;
    cardSprite.canvasData = canvasData;
    cardSprite.mask = mask;
    cardSprite.slug = data;
    cardSprite.placed = placed;
    cardSprite.tokenID = tokenID;
    cardSprite.name = tokensData.name;
    cardSprite.scroll = ScrollViewGameObject;
    cardSprite.placementLogic = placementLogic;
    card.transform.SetParent(ScrollViewGameObject.transform, false);
  }

  IEnumerator GetTokens(string slug, Action<Tokensdata> action)
  {
    if (canvasData.accountAddress == "")
    {
      yield break;
    }

    if (data != previousSlug)
    {
        page = 1;
        previousSlug = data;
    }

    int offset = (page-1) * 10;

    string baseURL = $"https://api.opensea.io/api/v1/assets?owner={canvasData.accountAddress}&collection={slug}&offset={offset}&limit={10}";
  
    using (UnityWebRequest webRequest = UnityWebRequest.Get(baseURL))
    {
      // Request and wait for the desired page.
      yield return webRequest.SendWebRequest();
      if (webRequest.isNetworkError)
      {
        Debug.Log(webRequest.error);
      }
      else
      {
        Debug.Log(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        Debug.Log(baseURL);
        Tokensdata tokensData = JsonUtility.FromJson<Tokensdata>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        action(tokensData);
      }
    }
  }

  private static bool StringStartsWith(string a, string b)
  {
    int aLen = a.Length;
    int bLen = b.Length;
    int ap = 0;
    int bp = 0;

    while (ap < aLen && bp < bLen && a[ap] == b[bp])
    {
      ap++;
      bp++;
    }

    return (bp == bLen && aLen >= bLen) || (ap == aLen && bLen >= aLen);
  }

}

