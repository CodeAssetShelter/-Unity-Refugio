using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ListTester : MonoBehaviour
{
    List<ShopManager.ItemInfo> infoOne;
    List<ShopManager.ItemInfo> infoTwo;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("TEST", 1.0f);
    }
    public void TEST()
    {
        infoOne = new List<ShopManager.ItemInfo>();
        var m = ShopManager.Instance.itemInfoList;
        //Debug.Log("Count : " + m.Count);

        infoOne = m.ConvertAll(obj => (ShopManager.ItemInfo)obj.Clone());
        //for(int i = 0; i < m.Count; i++)
        //{
        //    //infoOne.Add((ShopManager.ItemInfo)m[i].Clone());
        //    infoOne.Add(m[i]);
        //}
        //infoTwo = new List<ShopManager.ItemInfo>(infoOne);
    }
    public void PrintList()
    {
        //Debug.LogError("ONE : " + infoOne.Count);
        //GoogleCloudManager.Instance.PrintData(infoOne);
        //Debug.LogError("TWO");
        //GoogleCloudManager.Instance.PrintData(infoTwo);
    }
}
