using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPGSConfirmWindow : MonoBehaviour
{
    public AutoResizeRect childScript;

    public Transform details;
    public Text leftName, rightName;

    public Sprite circle, cross;

    public Button confirmButton;

    SaveData.UserData userDataLeft, userDataRight;
    List<ShopManager.ItemInfo> itemInfoLeft, itemInfoRight;
    List<GameObject> purchaseListLeft, purchaseListRight;
    ShopManager shopManager;

    private void Awake()
    {
        purchaseListLeft = new List<GameObject>();
        purchaseListRight = new List<GameObject>();
    }
    //private void OnEnable()
    //{
    //    ShopManager shopManager = FindObjectOfType<ShopManager>();
    //    itemInfoLeft = itemInfoRight = new List<ShopManager.ItemInfo>();
    //    userDataLeft = userDataRight = new SaveData.UserData();

    //    itemInfoLeft.Clear();
    //    itemInfoRight.Clear();
    //    userDataLeft = new SaveData.UserData();
    //    userDataRight = new SaveData.UserData();

    //    Transform left = details.Find("Left");
    //    if (left == null) { Debug.LogError("NULL"); }
    //    Debug.Log("LEFT : " + left.name);
    //    Transform right = details.Find("Right");
    //    if (right == null) { Debug.LogError("NULL"); }
    //    userDataLeft.bestScore = MainManager.Instance.GetMyScore();
    //    userDataLeft.coins = MainManager.Instance.GetMyCoin();
    //    itemInfoLeft = shopManager.itemInfoList;

    //    userDataRight = SaveData.Instance.GetLocalUserData();
    //    itemInfoRight = SaveData.Instance.GetLocalPurchasedData();

    //    left.Find("Coin Text").GetComponent<Text>().text = "" + userDataLeft.coins;
    //    left.Find("Score Text").GetComponent<Text>().text = "" + userDataLeft.bestScore;
    //    right.Find("Coin Text").GetComponent<Text>().text = "" + userDataRight.coins;
    //    right.Find("Score Text").GetComponent<Text>().text = "" + userDataRight.bestScore;

    //    itemInfoLeft = shopManager.itemInfoList;

    //    Debug.Log("DEBUG : Left : " + left.childCount + " // Right : " + right.childCount + "\n" +
    //      "DEBUG : LeftList : " + purchaseListLeft.Count + " // Right : " + purchaseListRight.Count);
    //    for (int i = 0; i < purchaseListLeft.Count; i++)
    //    {
    //        Destroy(purchaseListLeft[i].gameObject);
    //    }
    //    for (int i = 0; i < purchaseListRight.Count; i++)
    //    {
    //        Destroy(purchaseListRight[i].gameObject);
    //    }
    //    purchaseListLeft.Clear();
    //    purchaseListRight.Clear();

    //    for (int i = 0; i < itemInfoLeft.Count; i++)
    //    {
    //        Image image;
    //        GameObject gameObject = new GameObject();
    //        image = gameObject.AddComponent<Image>();
    //        image.sprite = shopManager.playerSprites[i].sprites[0];
    //        gameObject.transform.SetParent(left);
    //        gameObject.transform.localScale = Vector3.one;
    //        purchaseListLeft.Add(gameObject);

    //        gameObject = new GameObject();
    //        image = gameObject.AddComponent<Image>();
    //        if (itemInfoLeft[i].purchased == true)
    //        {
    //            image.sprite = circle;
    //        }
    //        else
    //        {
    //            image.sprite = cross;
    //        }
    //        gameObject.transform.SetParent(left);
    //        gameObject.transform.localScale = Vector3.one;
    //        purchaseListLeft.Add(gameObject);
    //    }

    //    for (int i = 0; i < itemInfoLeft.Count; i++)
    //    {
    //        Image image;
    //        GameObject gameObject = new GameObject();
    //        image = gameObject.AddComponent<Image>();
    //        image.sprite = shopManager.playerSprites[i].sprites[0];
    //        gameObject.transform.SetParent(right);
    //        gameObject.transform.localScale = Vector3.one;
    //        purchaseListRight.Add(gameObject);


    //        gameObject = new GameObject();
    //        image = gameObject.AddComponent<Image>();
    //        if (itemInfoLeft[i].purchased == true)
    //        {
    //            image.sprite = circle;
    //        }
    //        else
    //        {
    //            image.sprite = cross;
    //        }
    //        gameObject.transform.SetParent(right);
    //        gameObject.transform.localScale = Vector3.one;
    //        purchaseListRight.Add(gameObject);
    //    }
    //    //childScript.RefreshReSize(true, details.childCount);
    //}

    public void SetDetails(bool isSave, System.Action action)
    {
        if (details == null)
        {
            //Debug.Log("DEBUG : Null Detail");
            return;
        }
        if (SaveData.Instance.GetCloudPurchasedData() == null)
        {
            //Debug.Log("Cloud Purchase Data null");
            return;
        }
        if (SaveData.Instance.GetCloudUserData() == null)
        {
            //Debug.Log("Cloud User Data null");
            return;
        }

        itemInfoLeft = new List<ShopManager.ItemInfo>();
        itemInfoRight = new List<ShopManager.ItemInfo>();
        userDataLeft = new SaveData.UserData();
        userDataRight = new SaveData.UserData();

        shopManager = FindObjectOfType<ShopManager>();

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => gameObject.SetActive(false));
        confirmButton.onClick.AddListener(() => action.Invoke());

        if (isSave == true)
        {
            // Left = Local, Right = Cloud
            //Debug.Log("DEBUG isSave : " + isSave);
            SaveData.CloudGameData rightCloudData = SaveData.Instance.GetCloudData();

            userDataLeft.bestScore = MainManager.Instance.GetMyScore();
            userDataLeft.coins = MainManager.Instance.GetMyCoin();
            itemInfoLeft = SaveData.Instance.GetLocalPurchasedData();


            userDataRight = rightCloudData.userData;
            itemInfoRight = rightCloudData.purchasedData;

            //userDataLeft.bestScore = MainManager.Instance.GetMyScore();
            //userDataLeft.coins = MainManager.Instance.GetMyCoin();
            //itemInfoLeft = SaveData.Instance.GetLocalPurchasedData();
            ////Debug.Log("LIST \n" + MainManager.Instance.GetMyScore() + " \n " + MainManager.Instance.GetMyCoin() + "\n " +
            ////           SaveData.Instance.GetCloudUserData().bestScore + " \n " + SaveData.Instance.GetCloudUserData().coins);
            //userDataRight = SaveData.Instance.GetCloudUserData();
            //itemInfoRight = SaveData.Instance.GetCloudPurchasedData();
            //Debug.Log("LEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFT");
            //GoogleCloudManager.Instance.PrintData(itemInfoLeft);

            //Debug.Log("RIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHT");
            //GoogleCloudManager.Instance.PrintData(itemInfoRight);

            //Debug.Log("RIGHTCLOUD RIGHTCLOUD RIGHTCLOUD RIGHTCLOUD RIGHTCLOUD RIGHTCLOUD RIGHTCLOUD");
            //GoogleCloudManager.Instance.PrintData(SaveData.Instance.GetCloudData());


            leftName.text = "Player Data";
            rightName.text = "Cloud Data";
        }
        else
        {
           //Debug.Log("DEBUG isSave : " + isSave);
            // Left = Cloud, Right = Local

            SaveData.CloudGameData leftCloudData = SaveData.Instance.GetCloudData();

            userDataLeft = leftCloudData.userData;
            itemInfoLeft = leftCloudData.purchasedData;

            // Debug.Log("LIST \n" + MainManager.Instance.GetMyScore() + " \n " + MainManager.Instance.GetMyCoin() + "\n " +
            //SaveData.Instance.GetCloudUserData().bestScore + " \n " + SaveData.Instance.GetCloudUserData().coins);
            userDataRight.bestScore = MainManager.Instance.GetMyScore();
            userDataRight.coins = MainManager.Instance.GetMyCoin();
            itemInfoRight = SaveData.Instance.GetLocalPurchasedData();


            // userDataLeft = SaveData.Instance.GetCloudUserData();
            // itemInfoLeft = SaveData.Instance.GetCloudPurchasedData();
            //// Debug.Log("LIST \n" + MainManager.Instance.GetMyScore() + " \n " + MainManager.Instance.GetMyCoin() + "\n " +
            ////SaveData.Instance.GetCloudUserData().bestScore + " \n " + SaveData.Instance.GetCloudUserData().coins);
            // userDataRight.bestScore = MainManager.Instance.GetMyScore();
            // userDataRight.coins = MainManager.Instance.GetMyCoin();
            // itemInfoRight = SaveData.Instance.GetLocalPurchasedData();

            //Debug.Log("LEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFTLEFT");
            //GoogleCloudManager.Instance.PrintData(itemInfoLeft);

            //Debug.Log("RIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHTRIGHT");
            //GoogleCloudManager.Instance.PrintData(itemInfoRight);
                      
            //Debug.Log("LEFTCLOUD LEFTCLOUD LEFTCLOUD LEFTCLOUD LEFTCLOUD LEFTCLOUD LEFTCLOUD LEFTCLOUD");
            //GoogleCloudManager.Instance.PrintData(SaveData.Instance.GetCloudData());


            leftName.text = "Cloud Data";
            rightName.text = "Player Data";
        }

        //Debug.Log("DATA LIST \n Left : " + userDataLeft.coins + " \\ " + userDataLeft.bestScore);
        //Debug.Log("DATA LIST \n Right : " + userDataRight.coins + " \\ " + userDataRight.bestScore);

        // Caching Transforms
        Transform left = details.Find("Left");
        Transform right = details.Find("Right");

        if (left == null || right == null) return;

        // Set User Data
        left.Find("Coin Text").GetComponent<Text>().text = "" + userDataLeft.coins;
        left.Find("Score Text").GetComponent<Text>().text = "" + userDataLeft.bestScore;
        right.Find("Coin Text").GetComponent<Text>().text = "" + userDataRight.coins;
        right.Find("Score Text").GetComponent<Text>().text = "" + userDataRight.bestScore;

        //Debug.Log("DEBUG : Left : " + left.childCount + " // Right : " + right.childCount + "\n" +
        //          "DEBUG : LeftList : " + purchaseListLeft.Count + " // Right : " + purchaseListRight.Count);


        for (int i = 0; i < itemInfoLeft.Count; i++)
        { 
            Image image;
            GameObject gameObjectBird = new GameObject();
            image = gameObjectBird.AddComponent<Image>();
            image.sprite = shopManager.playerSprites[i].sprites[0];
            gameObjectBird.transform.SetParent(left);
            gameObjectBird.transform.localScale = Vector3.one;
            purchaseListLeft.Add(gameObjectBird);

            GameObject gameObjectShape = new GameObject();
            image = gameObjectShape.AddComponent<Image>();
            //Debug.LogError("Left Number " + i + " : " + itemInfoLeft[i].purchased);
            if (itemInfoLeft[i].purchased == true)
            {
                image.sprite = circle;
            }
            else
            {
                image.sprite = cross;
            }
            gameObjectShape.transform.SetParent(left);
            gameObjectShape.transform.localScale = Vector3.one;
            purchaseListLeft.Add(gameObjectShape);
        }

        for (int i = 0; i < itemInfoRight.Count; i++)
        {
            Image image;
            GameObject gameObjectBird = new GameObject();
            image = gameObjectBird.AddComponent<Image>();
            image.sprite = shopManager.playerSprites[i].sprites[0];
            gameObjectBird.transform.SetParent(right);
            gameObjectBird.transform.localScale = Vector3.one;
            purchaseListRight.Add(gameObjectBird);

            GameObject gameObjectShape = new GameObject();
            image = gameObjectShape.AddComponent<Image>();
            //Debug.LogError("Left Number " + i + " : " + itemInfoRight[i].purchased);
            if (itemInfoRight[i].purchased == true)
            {
                image.sprite = circle;
            }
            else
            {
                image.sprite = cross;
            }
            gameObjectShape.transform.SetParent(right);
            gameObjectShape.transform.localScale = Vector3.one;
            purchaseListRight.Add(gameObjectShape);
        }

        //Debug.Log("DEBUG 2 : Left : " + left.childCount + " // Right : " + right.childCount + "\n" +
        //          "DEBUG 2 : LeftList : " + purchaseListLeft.Count + " // Right : " + purchaseListRight.Count);
    }

    private void OnDisable()
    {
        if (purchaseListLeft.Count != 0)
        {
            for (int i = 0; i < purchaseListLeft.Count; i++)
            {
                Destroy(purchaseListLeft[i].gameObject);
            }
            purchaseListLeft.Clear();
        }
        if (purchaseListRight.Count != 0)
        {
            for (int i = 0; i < purchaseListRight.Count; i++)
            {
                Destroy(purchaseListRight[i].gameObject);
            }
            purchaseListRight.Clear();
        }
    }
}
