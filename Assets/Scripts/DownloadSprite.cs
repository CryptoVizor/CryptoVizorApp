using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadSprite : MonoBehaviour
{

  public bool placed;
  public Image cardRendered;

  public UnityEngine.XR.Interaction.Toolkit.AR.WallPlacing placementLogic;

  public GameObject scroll;

  public GameObject mask;

  public GameObject placedLabel;

  public GetTokensData canvasData;

  private bool hideScroll;

  public string URI;

  public string slug;

  public string tokenID;

  public string name;

  public Texture2D placeholder;

  public Texture2D loading;


  // Start is called before the first frame update
  void Start()
  {
    if (placed)
    {
      placedLabel.SetActive(true);
    }
    StartCoroutine(GetCardImage(URI));
  }

  IEnumerator GetCardImage(string uri)
  {
    using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
    {
      // Request and wait for the desired page.
      yield return webRequest.SendWebRequest();
      if (webRequest.isNetworkError)
      {
        Debug.Log(webRequest.error);
      }
      else
      {
        Texture2D webTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture as Texture2D;
        if (isBogus(webTexture))
        {
          Sprite webSpriteBogus = SpriteFromTexture2D(placeholder);
          cardRendered.sprite = webSpriteBogus;
        } else {
        Sprite webSprite = SpriteFromTexture2D(webTexture);
        cardRendered.sprite = webSprite;
        }
      }
    }
  }

  public bool isBogus(Texture tex)
  {
    if (!tex) return true;

    byte[] png1 = (tex as Texture2D).EncodeToPNG();
    byte[] questionMarkPNG = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 8, 0, 0, 0, 8, 8, 2, 0, 0, 0, 75, 109, 41, 220, 0, 0, 0, 65, 73, 68, 65, 84, 8, 29, 85, 142, 81, 10, 0, 48, 8, 66, 107, 236, 254, 87, 110, 106, 35, 172, 143, 74, 243, 65, 89, 85, 129, 202, 100, 239, 146, 115, 184, 183, 11, 109, 33, 29, 126, 114, 141, 75, 213, 65, 44, 131, 70, 24, 97, 46, 50, 34, 72, 25, 39, 181, 9, 251, 205, 14, 10, 78, 123, 43, 35, 17, 17, 228, 109, 164, 219, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130, };

    return Equivalent(png1, questionMarkPNG);
  }

  public bool Equivalent(byte[] bytes1, byte[] bytes2)
  {
    if (bytes1.Length != bytes2.Length) return false;
    for (int i = 0; i < bytes1.Length; i++)
      if (!bytes1[i].Equals(bytes2[i])) return false;
    return true;
  }

  public void ClickButton(){
    if (placed)
    {
        return;
    }
    if (cardRendered.sprite.Equals(loading))
    {
        return;
    }
    placementLogic.card = cardRendered.sprite;
    placementLogic.tokenID = tokenID;
    placementLogic.name = name;
    placementLogic.slug = slug;
    scroll.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
    mask.SetActive(false);
    if(!canvasData.placedTokens.ContainsKey(slug)){
      Debug.Log("dictionary added");
      canvasData.placedTokens.Add(slug,new Dictionary<string, bool>());
    }
  }


  Sprite SpriteFromTexture2D(Texture2D texture)
  {

    return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
  }

}
