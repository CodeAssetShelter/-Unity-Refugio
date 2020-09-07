using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private static ShopManager _instance;
    public static ShopManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(ShopManager)) as ShopManager;

                if (_instance == null)
                {
                    //Debug.LogError("No Active GameManager!");
                }
            }

            return _instance;
        }
    }

    [SerializeField]
    [System.Serializable]
    //public class ItemInfo
    //{
    //    public string name;
    //    public bool purchased = false;
    //    public bool selected = false;
    //    [System.NonSerialized]
    //    public Sprite playerSprite;
    //    public float lifeTime = 60;
    //    public float shieldDuration = 0.5f;
    //    public int itemSlot = 1;
    //    public int price = 10;

    //    [HideInInspector]
    //    public float[] details;

    //    public void MergeDetail()
    //    {
    //        details = new float[4] { lifeTime, shieldDuration, itemSlot, price };
    //    }
    //}
    public class ItemInfo : ICloneable
    {
        public object Clone()
        {
            ItemInfo newData = new ItemInfo();
            newData.name = this.name;
            newData.purchased = this.purchased;
            newData.selected = this.selected;
            newData.playerSprite = this.playerSprite;
            newData.lifeTime = this.lifeTime;
            newData.shieldDuration = this.shieldDuration;
            newData.itemSlot = this.itemSlot;
            newData.price = this.price;

            return newData;
        }
        public string name;
        public ObscuredBool purchased = false;
        public ObscuredBool selected = false;
        //[System.NonSerialized]
        //public Sprite playerSprite;
        public int playerSprite = 0;
        public ObscuredFloat lifeTime = 60;
        public ObscuredFloat shieldDuration = 0.5f;
        public ObscuredInt itemSlot = 1;
        public ObscuredInt price = 10;

        [HideInInspector]
        public float[] details;

        public void MergeDetail()
        {
            details = new float[4] { lifeTime, shieldDuration, itemSlot, price };
        }
    }
    [System.Serializable]
    public class PlayerSprites
    {
        public Sprite []sprites = new Sprite[3];
    }
    

    [Header("- Prefabs")]
    public ItemBlock itemBlockPrefab;

    [Header("- Windows")]
    public ConfirmWindow confirmWindow;
    public ResultWindow resultWindow;

    [Header("- Holders")]
    public Transform contents;
    public Text textMyCoin;

    [Header("- Goods sprites")]
    public PlayerSprites[] playerSprites;


    public List<ItemInfo> itemInfoList = new List<ItemInfo>();
    private List<ItemBlock> itemBlockList = new List<ItemBlock>();

    private MainManager mainManager;

    private void Start()
    {
        mainManager = FindObjectOfType<MainManager>();

        itemBlockList.Clear();

        List<ItemInfo> loadedList = SaveData.Instance.LoadPurchasedShopData();
        if (loadedList == null)
        {
            Debug.Log("Can't found Load");
            for (int i = 0; i < itemInfoList.Count; i++)
            {
                ItemInfo info = itemInfoList[i];
                info.MergeDetail();

                ItemBlock send = Instantiate(itemBlockPrefab, contents);
                //info.playerSprite = playerSprites[i].sprites[0];
                info.playerSprite = i;
                send.SetItemInfo(itemInfoList[i], i, playerSprites[i].sprites);

                if (info.purchased == true)
                {
                    send.ChangeButton(send.purchasedSprite);
                }
                if (info.selected == true)
                {
                    send.ChangeButton(send.sortieSprite);
                    MainManager.Instance.ChangePlayerSprite(playerSprites[i].sprites);
                    holdedItemBlock = send;
                }
                itemBlockList.Add(send);
            }
        }
        else
        {
            //Debug.Log("Found shop data");
            //itemInfoList = loadedList;

            for (int i = 0; i < loadedList.Count; i++)
            {
                ItemInfo itemInfo = itemInfoList.Find(obj => obj.name == loadedList[i].name);

                // 클라에 저장된 값으로 덮어쓰기
                // 구매 정보만 받아온다
                if (itemInfo != null)
                {
                    itemInfo.purchased = loadedList[i].purchased;
                    itemInfo.selected = loadedList[i].selected;
                }
            }

            for (int i = 0; i < itemInfoList.Count; i++)
            {
                ItemInfo info = itemInfoList[i];
                info.MergeDetail();

                ItemBlock send = Instantiate(itemBlockPrefab, contents);
                //info.playerSprite = playerSprites[i].sprites[0];
                info.playerSprite = i;
                send.SetItemInfo(itemInfoList[i], i, playerSprites[i].sprites);

                if (info.purchased == true)
                {
                    send.ChangeButton(send.purchasedSprite);
                }
                if (info.selected == true)
                {
                    if (holdedItemBlock != null && holdedItemBlock.selected == true)
                    {
                        holdedItemBlock.selected = false;
                    }
                    send.ChangeButton(send.sortieSprite);
                    MainManager.Instance.ChangePlayerSprite(playerSprites[i].sprites);
                    holdedItemBlock = send;
                }
                itemBlockList.Add(send);
            }


            //for (int i = 0; i < itemInfoList.Count; i++)
            //{
            //    ItemBlock send = Instantiate(itemBlockPrefab, contents);
            //    ItemInfo info = itemInfoList[i];
            //    //info.playerSprite = goodsSprites[i];

            //    info.playerSprite = playerSprites[i].sprites[0];

            //    if (info.purchased == true)
            //    {
            //        send.ChangeButton(send.purchasedSprite);
            //    }
            //    if (info.selected == true)
            //    {
            //        send.ChangeButton(send.sortieSprite);
            //        mainManager.ChangePlayerSprite(playerSprites[i].sprites);
            //        holdedItemBlock = send;
            //    }
            //    send.SetItemInfo(info, i, playerSprites[i].sprites);
            //    //Debug.Log("S : " + info.itemSlot);
            //    itemBlockList.Add(send);
            //}
        }
        SaveData.Instance.SavePurchasedShopData(itemInfoList);
        UIManager.Instance.RefreshCoin();
    }

    public bool PurchaseGoods(int itemInfoIdx)
    {
        ItemInfo info = itemInfoList[itemInfoIdx];

        ObscuredInt myCoins = mainManager.GetMyCoin();
        // Lack Money
        if (myCoins - info.price < 0)
        {
            // 실패창 띄우기
            SaveData.Instance.SavePurchasedShopData(itemInfoList);
            return false;
        }
        else
        {
            myCoins -= info.price;

            // 성공시 아이템 인포에 거래내역 저장
            info.purchased = true;

            if (myCoins < 0)
            {
                myCoins = 0;
            }

            mainManager.SetMyCoin(myCoins);
            RefreshCoin();
            // 성공창 띄우기

            resultWindow.gameObject.SetActive(true);
            resultWindow.InitWindowSuccessPurchase(playerSprites[itemInfoIdx].sprites[0]);
            SaveData.Instance.SavePurchasedShopData(itemInfoList);
            return true;
        }
    }

    public void CheckEnoughMoney(int itemInfoIdx)
    {
        ItemInfo info = itemInfoList[itemInfoIdx];
        ObscuredInt myCoins = mainManager.GetMyCoin();

        if (myCoins - info.price < 0)
        {
            // 실패창 띄우기
            resultWindow.gameObject.SetActive(true);
            resultWindow.InitWindowLackOfCoin(info.price, myCoins, playerSprites[itemInfoIdx].sprites[0]);
        }
        else
        {
            // 성공
            confirmWindow.gameObject.SetActive(true);
            confirmWindow.InitConfirmWindow(info, itemBlockList[itemInfoIdx]);
        }

        SaveData.Instance.SavePurchasedShopData(itemInfoList);
    }

    ItemBlock holdedItemBlock;
    public void ChangeSortiePlayer(int itemBlockIndex)
    {
        ItemBlock selectedItemBlock = itemBlockList[itemBlockIndex];
        if(holdedItemBlock == null)
        {
            // 첫 출격 설정 or 설정이 안된 경우 대비
            selectedItemBlock.ChangeSortieState(true);
            holdedItemBlock = selectedItemBlock;
            itemInfoList[itemBlockIndex].selected = true;
        }
        else
        {   // 정상작동 출격 설정
            itemInfoList[holdedItemBlock.itemInfoIndex].selected = false;
            holdedItemBlock.ChangeSortieState(false);
            holdedItemBlock = selectedItemBlock;
            selectedItemBlock.ChangeSortieState(true);
            itemInfoList[itemBlockIndex].selected = true;
        }

        SaveData.Instance.SavePurchasedShopData(itemInfoList);
    }

    public void RefreshCoin()
    {
        textMyCoin.text = "" + mainManager.GetMyCoin();
    }

    public void ActiveShopGameObject()
    {
        mainManager.SetActiveText(false);
        transform.GetChild(0).gameObject.SetActive(true);
        RefreshCoin();
    }

    public void DeactiveShopGameObject()
    {
        SaveData.Instance.SavePurchasedShopData(itemInfoList);
        transform.GetChild(0).gameObject.SetActive(false);
        UIManager.Instance.CheckActiveCoinText();
        mainManager.SetActiveText(true);
    }

    private void OnApplicationQuit()
    {
        //SaveData.Instance.SavePurchasedShopData(itemInfoList);
    }

    public ItemInfo GetSelectedPlayerInfo()
    {
        if (holdedItemBlock == null)
        {
            //ItemInfo dummy = new ItemInfo()
            return itemInfoList[0];
        }
        else
            return itemInfoList[holdedItemBlock.itemInfoIndex];
    }

    public Sprite GetPlayerSprite(int index)
    {
        return playerSprites[index].sprites[0];
    }

    public void RefreshShopList()
    {
        for (int i = 0; i < itemInfoList.Count; i++)
        {
            if (contents.GetChild(i) == null) return;

            ItemInfo info = itemInfoList[i];
            info.MergeDetail();

            ItemBlock send = contents.GetChild(i).GetComponent<ItemBlock>();
            //info.playerSprite = playerSprites[i].sprites[0];
            info.playerSprite = i;
            send.SetItemInfo(itemInfoList[i], i, playerSprites[i].sprites);

            if (info.purchased == true)
            {
                send.ChangeButton(send.purchasedSprite);
                if (info.selected == true)
                {
                    if (holdedItemBlock != null && holdedItemBlock.selected == true)
                    {
                        holdedItemBlock.selected = false;
                    }
                    send.ChangeButton(send.sortieSprite);
                    MainManager.Instance.ChangePlayerSprite(playerSprites[i].sprites);
                    holdedItemBlock = send;
                }
            }
            else
            {
                send.ChangeButton(send.spritesBuy);
            }
        }
    }
}
