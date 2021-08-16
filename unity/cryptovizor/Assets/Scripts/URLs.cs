using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLs : MonoBehaviour
{
  public void supportedTokens(){
    Application.OpenURL("https://www.notion.so/tart/Supported-tokens-list-db640eeaa45d402ebfc6d6a1608a646b");
  }

  // Start is called before the first frame update
  public void contacts(){
    Application.OpenURL("https://forms.gle/T5zg1pYbRhYVbkBm7");
  }

  public void privacyPolicy(){
    Application.OpenURL("https://www.notion.so/tart/Privacy-Policy-3fee0790959646d380ff4de8dfc19084");
  }

  public void team(){
    Application.OpenURL("https://www.notion.so/tart/Our-Team-ffc9f64068b44df397ffea9f7f33915a");
  }

   public void serviceSettings(){
    Application.OpenURL("https://www.notion.so/tart/How-to-get-user-infomation-b37a4b65c0ca47ff97a192f06be96a5a");
  }
}
