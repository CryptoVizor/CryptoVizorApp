using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pageLoader : MonoBehaviour
{
    public loadInterface buttonLoader;

    public TextMeshProUGUI pageNumber;

    public void Next()
    {
        if ((buttonLoader.count+9)/10<= buttonLoader.page)
        {
            return;
        }
        Debug.Log("next");
        buttonLoader.page = buttonLoader.page+1;
        buttonLoader.Click();
        updatePageNumber();
    }

    // Update is called once per frame
    public void Prev()
    {
        if(buttonLoader.page == 1){return;}
        buttonLoader.page = buttonLoader.page -1;
        buttonLoader.Click();
        updatePageNumber();
    }

    public void updatePageNumber(){
        pageNumber.SetText($"{buttonLoader.page}/{(buttonLoader.count + 9) / 10}");
    }

  public void updatePageNumberLoading()
  {
    pageNumber.SetText("Loading");
  }
}
