using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public interface loadInterface
{
   int count{
    get;
    set;
  }

   int page{
    get;
    set;
  }

   void Click();
}