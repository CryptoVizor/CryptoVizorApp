using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class setAddressCryptoSpells : MonoBehaviour
{

    public TMP_InputField addressField;

    public GetTokensData tokenData;
    // Start is called before the first frame update
    public void Click()
    {
        tokenData.cryptoSpellsAddress = addressField.text.ToLower();
        Debug.Log(tokenData.accountAddress);
        tokenData.CreateNewUser();
        gameObject.transform.parent.gameObject.SetActive(false);
        PlayerPrefs.SetString("cryptoSpellsAddress",addressField.text.ToLower());
        PlayerPrefs.Save();
    }

}
