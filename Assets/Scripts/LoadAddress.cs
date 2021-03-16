using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAddress : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject loginPage;
    void Start()
    {
        bool hasAccount = false;
        GetTokensData tokesData = gameObject.GetComponent<GetTokensData>();
        if (PlayerPrefs.HasKey("address"))
        {
            tokesData.accountAddress = PlayerPrefs.GetString("address");
            hasAccount = true;
        }
        if (PlayerPrefs.HasKey("cryptoSpellsAddress"))
        {
            tokesData.cryptoSpellsAddress = PlayerPrefs.GetString("cryptoSpellsAddress");
        }
        if (hasAccount)
        {
            tokesData.CreateNewUser();
            loginPage.SetActive(false);
        }
    }

}
