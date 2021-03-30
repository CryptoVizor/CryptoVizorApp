using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class loadCryptoSpells : MonoBehaviour, loadInterface
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

  public GetTokensData.CryptoSpellsCollectionsData cryptoSpellsCollectionsData;

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

    List<CryptoSpellsItemMetadataData> tokensData = new List<CryptoSpellsItemMetadataData>();
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

    foreach (var tokenAssetData in tokensData)
    {
      bool placed = false;
      if (canvasData.placedTokens.ContainsKey(data))
      {
        if (canvasData.placedTokens[data].ContainsKey(tokenAssetData.collections.token_id))
        {
          if (canvasData.placedTokens[data][tokenAssetData.collections.token_id])
          {
            placed = true;
          }
        }
      }
      MetaData tokenData = new MetaData();
      tokenData.image = tokenAssetData.collections.image_url.en;
      tokenData.name = tokenAssetData.collections.name.en;
      setItemData(tokenData, tokenAssetData.collections.token_id, placed);
      loader.buttonLoader = this;
      loader.updatePageNumber();
    }
  }

  private void setItemData(MetaData tokensData, string tokenID, bool placed)
  {
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

  IEnumerator GetTokens(string slug, Action<List<CryptoSpellsItemMetadataData>> action)
  {
    if (canvasData.cryptoSpellsAddress == "")
    {
      yield break;
    }

    List<CryptoSpellsItemsData> tokensdatas = new List<CryptoSpellsItemsData>();
    List<CryptoSpellsItemMetadataData> tokensdatasMetadata = new List<CryptoSpellsItemMetadataData>();

    Debug.Log("cryptoSpellsCollectionsDataGetTokens");
    Debug.Log(cryptoSpellsCollectionsData.collections.player_card_token_ids.Count);
    Debug.Log(cryptoSpellsCollectionsData.collections.player_id);
    Debug.Log(cryptoSpellsCollectionsData.collections.player_address);

    foreach (var item in cryptoSpellsCollectionsData.collections.player_card_token_ids)
    {
      string baseURL = $"https://cryptospells.jp/public_api/card_tokens/{item}.json";

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
          Debug.Log("{\"collections\":" + System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data) + "}");
          Debug.Log(baseURL);
          CryptoSpellsItemsData tokensData = JsonUtility.FromJson<CryptoSpellsItemsData>("{\"collections\":" + System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data) + "}");
          Debug.Log($"tokensData {tokensData.collections.card_number}");
          tokensdatas.Add(tokensData);
        }
      }   
    }

    if (canvasData.accountAddress == "")
    {
      yield break;
    }

    if (data != previousSlug)
    {
      page = 1;
      previousSlug = data;
    }

    int offset = (page - 1) * 10;
    if (tokensdatas.Count < offset+10)
    {
      tokensdatas = tokensdatas.GetRange(offset, tokensdatas.Count-offset);
    } else {
      tokensdatas = tokensdatas.GetRange(offset,10);
    }

    Dictionary<string, CryptoSpellsItemMetadataData>  chacheData = new Dictionary<string, CryptoSpellsItemMetadataData>();

    foreach (var item in tokensdatas)
    {
      if (chacheData.ContainsKey(item.collections.card_number) != false)
      {
        CryptoSpellsItemMetadataData chacheToken;
        chacheData.TryGetValue(item.collections.card_number, out chacheToken);
        chacheToken.collections.token_id = item.collections.token_id;
        tokensdatasMetadata.Add(chacheToken);
        continue;
      }
      string baseURL = $"https://cryptospells.jp/public_api/cards/{item.collections.card_number}.json";

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
          CryptoSpellsItemMetadataData tokensData = JsonUtility.FromJson<CryptoSpellsItemMetadataData>("{\"collections\":" + System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data) + "}");
          tokensData.collections.token_id = item.collections.token_id;
          tokensdatasMetadata.Add(tokensData);
          if (chacheData.ContainsKey(item.collections.card_number) != false)
          {
            chacheData.Add(item.collections.card_number,tokensData);
          }
        }
      }
    }
    action(tokensdatasMetadata);
  }


  [System.Serializable]
  public class CryptoSpellsItemsData
  {
    public CryptoSpellsItem collections;
  }

  [System.Serializable]
  public class CryptoSpellsItem
  {
    public string token_id;
    public string card_number;
    public int serial_number;
    public int level;
    public int experience_point;
    public string flavor_text;
  }

  [System.Serializable]
  public class Name
  {
    public string en;
    public string ja;
  }

  [System.Serializable]
  public class ImageUrl
  {
    public string en;
    public string ja;
  }

  [System.Serializable]
  public class CryptoSpellsItemMetadataData
  {
    public CryptoSpellsItemMetadata collections;
  }

  [System.Serializable]
  public class CryptoSpellsItemMetadata
  {
    public int card_number;
    public Name name;
    public int issued_num;
    public string rarity;
    public string kind;
    public string color;
    public string card_type;
    public string token_id;
    public ImageUrl image_url;
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

