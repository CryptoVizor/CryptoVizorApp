using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadSettings : MonoBehaviour
{

  [SerializeField]
  TextMeshProUGUI m_addressSettings;

  public TextMeshProUGUI addressSettings
  {
    get { return m_addressSettings; }
    set { m_addressSettings = value; }
  }

    public GameObject AddressView;

    public string key;
    // Start is called before the first frame update
    private void OnEnable() {
        Debug.Log("OnEnable");
        Debug.Log(PlayerPrefs.GetString(key));
        addressSettings.SetText(PlayerPrefs.GetString(key));
    }

    public void onAccountDelete(){
        PlayerPrefs.DeleteAll();
        gameObject.SetActive(false);
        AddressView.SetActive(true);
    }

}
