using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class GetTokensData : MonoBehaviour
{
  public GameObject collections;

  public GameObject collectionPrefab;

  public GameObject cardPrefab;

  public List<string> contractsAddresses;

  public UnityEngine.XR.Interaction.Toolkit.AR.CustomPlacing placementLogic;

  public GameObject scroll;

  public GameObject ScrollViewGameObject;

  public GameObject mask;

  public Dictionary<string, Dictionary<string, bool>> placedTokens = new Dictionary<string, Dictionary<string, bool>>();


  public string accountAddress;

  public string cryptoSpellsAddress = "";

  // Start is called before the first frame update
  void Start()
  {
    CreateNewUser();
  }

  public async void CreateNewUser()
  {
    GetCardCollectionsData();
  }

  CryptoSpellsCollectionsData cryptoSpellsCollectionsData;

  public async void GetCardCollectionsData()
  {

    var TokensMap = new Dictionary<string, Tuple<string, int>>();

    CollectionsData tokensData = new CollectionsData();
    cryptoSpellsCollectionsData = new CryptoSpellsCollectionsData();

    await GetCryptoSpellsCollections(tokens => cryptoSpellsCollectionsData = tokens);
    await GetCollections(tokens => tokensData = tokens);
    if (tokensData.collections == null)
    {
        Debug.Log("tokensData.collections == null");
        return;
    }
    foreach (var item in tokensData.collections)
    {
      TokensMap.Add(item.name, new Tuple<string, int>(item.slug,item.owned_asset_count));
    }
    if (cryptoSpellsCollectionsData.collections != null && cryptoSpellsCollectionsData.collections.player_card_token_ids.Count != 0)
    {
      TokensMap.Add("CryptoSpells", new Tuple<string, int>("cryptoSpells", cryptoSpellsCollectionsData.collections.player_card_token_ids.Count));
    }

    GetCardCollections(TokensMap);
  }

  private void GetCardCollections(Dictionary<string, Tuple<string, int>> uris)
  {
    foreach (Transform child in scroll.transform)
    {
      GameObject.Destroy(child.gameObject);
    }
    foreach (var item in uris)
    {
      GameObject collectionsItem = Instantiate(collectionPrefab) as GameObject;
      Button buttonLogic = collectionsItem.GetComponent<Button>();
      TMPro.TextMeshProUGUI textView = collectionsItem.GetComponentInChildren<TMPro.TextMeshProUGUI>();
      if (item.Key == "CryptoSpells")
      {
        loadCryptoSpells collectionsCryptoSpellsItemDownload = collectionsItem.AddComponent<loadCryptoSpells>();
        buttonLogic.onClick.AddListener(collectionsCryptoSpellsItemDownload.Click);
        collectionsCryptoSpellsItemDownload.Title = item.Key;
        collectionsCryptoSpellsItemDownload.cryptoSpellsCollectionsData = cryptoSpellsCollectionsData;
        collectionsCryptoSpellsItemDownload.data = "CryptoSpellsGameitemsData";
        collectionsCryptoSpellsItemDownload.count = item.Value.Item2;
        collectionsCryptoSpellsItemDownload.mask = mask;
        collectionsCryptoSpellsItemDownload.canvasData = this;
        collectionsCryptoSpellsItemDownload.ScrollViewGameObject = ScrollViewGameObject;
        collectionsCryptoSpellsItemDownload.placementLogic = placementLogic;
        collectionsCryptoSpellsItemDownload.cardPrefab = cardPrefab;
        collectionsCryptoSpellsItemDownload.titleView = textView;
        collectionsCryptoSpellsItemDownload.titleView.SetText(item.Key);
        collectionsCryptoSpellsItemDownload.transform.SetParent(scroll.transform, false);
        return;
      }
      loadItems collectionsItemDownload = collectionsItem.AddComponent<loadItems>();
      buttonLogic.onClick.AddListener(collectionsItemDownload.Click);
      collectionsItemDownload.Title = item.Key;
      collectionsItemDownload.data = item.Value.Item1;
      collectionsItemDownload.count = item.Value.Item2;
      collectionsItemDownload.scroll = scroll;
      collectionsItemDownload.mask = mask;
      collectionsItemDownload.canvasData = this;
      collectionsItemDownload.ScrollViewGameObject = ScrollViewGameObject;
      collectionsItemDownload.placementLogic = placementLogic;
      collectionsItemDownload.cardPrefab = cardPrefab;
      collectionsItemDownload.titleView = textView;
      collectionsItemDownload.titleView.SetText(item.Key);
      collectionsItem.transform.SetParent(scroll.transform, false);
    }
  }


  IEnumerator GetCollections(Action<CollectionsData> action)
  {
    if (accountAddress == "")
    {
      yield break;
    }
    string baseURL = $"https://api.opensea.io/api/v1/collections?asset_owner={accountAddress}&limit=300";
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
        CollectionsData tokensData = JsonUtility.FromJson<CollectionsData>( "{\"collections\":" + System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data) + "}");
        action(tokensData);
      }
    }
  }


  IEnumerator GetCryptoSpellsCollections(Action<CryptoSpellsCollectionsData> action)
  {

    if (cryptoSpellsAddress == "")
    {
      yield break;
    }
    string baseURL = $"https://cryptospells.jp/public_api/players/{cryptoSpellsAddress}/cards.json";
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
        CryptoSpellsCollectionsData tokensData = JsonUtility.FromJson<CryptoSpellsCollectionsData>("{\"collections\":" + System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data) + "}");
        action(tokensData);
      }
    }
  }

  [System.Serializable]
  public class CryptoSpellsCollectionsData
  {
    public CryptoSpellsCollections collections;
  }

  [System.Serializable]
  public class CryptoSpellsCollections
  {
    public string player_id;
    public string player_address;
    public List<string> player_card_token_ids;
  }

  IEnumerator GetTokens(int page, Action<Tokensdata>action) 
  {
    if (accountAddress == "")
    {
      yield break;
    }
    string baseURL = $"https://api.opensea.io/api/v1/assets?owner={accountAddress}";
    foreach (var contractsAddress in contractsAddresses)
    {
      baseURL = baseURL + $"&asset_contract_addresses={contractsAddress}";
    }
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
        Tokensdata tokensData = JsonUtility.FromJson<Tokensdata>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        action(tokensData);
      }
    }
  }

}

[System.Serializable]
public class Tokensdata
{
  public List<URIdata> assets;
}

[System.Serializable]
public class URIdata
{
  public string image_url;
  public string name;
  public string token_id;

  public ContractData asset_contract;
}

[System.Serializable]
public class ContractData
{
  public string slug;
  public string name;
  public int owned_asset_count;
}

[System.Serializable]
public class CollectionsData
{
  public List<ContractData> collections;
}

[System.Serializable]
public class ContractAssets
{
  public string address;
}

[System.Serializable]
public class MetaData
{
  public string image;
  public string name;
}
