using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmWindow : MonoBehaviour
{
    public Image goodsImage;

    [Space(20)]
    [Header("- Purchase Detail")]
    public Text lifeTimeText;
    public Text shieldText;
    public Text itemSlotText;

    [Space(20)]
    public Text myCoin;
    public Text goodPrice;
    public Text calculatedCoin;

    private ItemBlock selectedItemblock;

    public void InitConfirmWindow(ShopManager.ItemInfo info, ItemBlock itemBlock)
    {
        selectedItemblock = itemBlock;
        
        goodsImage.sprite = ShopManager.Instance.GetPlayerSprite(info.playerSprite);
        lifeTimeText.text = ""+info.lifeTime;
        shieldText.text = "" + info.shieldDuration;
        itemSlotText.text = "" + info.itemSlot;

        myCoin.text = "" + MainManager.Instance.GetMyCoin();
        goodPrice.text = "" + info.price;
        calculatedCoin.text = "" + (int.Parse(myCoin.text) - info.price);

        gameObject.SetActive(true);
    }

    public void OnPurchase()
    {
        bool result = ShopManager.Instance.PurchaseGoods(selectedItemblock.itemInfoIndex);
        if (result == true)
        {
            // 성공
            selectedItemblock.OnPurchasedResult(result);
            gameObject.SetActive(false);
        }
        else
        {
            // Popup Failed Window
            selectedItemblock.OnPurchasedResult(result);
            //Debug.LogError("Purchase failed");
        }
    }
}
