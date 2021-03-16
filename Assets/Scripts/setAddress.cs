using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class setAddress : MonoBehaviour
{

    public TMP_InputField addressField;

    public GetTokensData tokenData;
    // Start is called before the first frame update
    public void Click()
    {
        tokenData.accountAddress = addressField.text.ToLower();
        Debug.Log(tokenData.accountAddress);
        tokenData.CreateNewUser();
        gameObject.transform.parent.gameObject.SetActive(false);
        PlayerPrefs.SetString("address",addressField.text.ToLower());
        PlayerPrefs.Save();
    }

}
