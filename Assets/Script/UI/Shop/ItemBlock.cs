using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class ItemBlock : MonoBehaviour
{
    public Button buyButton;
    public Sprite[] purchasedSprite = new Sprite[2];
    public Sprite[] sortieSprite = new Sprite[2];
    public Sprite[] spritesBuy = new Sprite[2];

    [Space(20)]
    public ObscuredBool purchased = false;
    public ObscuredBool selected = false;
    public Image playerSprite;
    private Sprite[] playerSprites;
    public Text[] details;

    public int itemInfoIndex = -1;

    public void CheckEnoughMoney()
    {
        if (purchased == false)
            FindObjectOfType<ShopManager>().CheckEnoughMoney(itemInfoIndex);
            //ShopManager.Instance.CheckEnoughMoney(itemInfoIndex);
        else // 거래가 된 경우 출격으로 변환
        {
            FindObjectOfType<ShopManager>().ChangeSortiePlayer(itemInfoIndex);
            //ShopManager.Instance.ChangeSortiePlayer(itemInfoIndex);
            //Debug.Log("selected : " + name);
        }
    }

    public void OnPurchasedResult(bool success)
    {
        if (success == true)
        {
            ChangeButton(purchasedSprite);
            purchased = true;
        }
        else
        {

        }
    }

    public void ChangeSortieState(bool active)
    {
        if (active)
        {
            ChangeButton(sortieSprite);
            MainManager.Instance.ChangePlayerSprite(playerSprites);
        }
        else
        {
            ChangeButton(purchasedSprite);
        }
    }

    public void SetItemInfo(ShopManager.ItemInfo info, int infoIdx, Sprite[] sprites)
    {
        itemInfoIndex = infoIdx;
        this.playerSprites = sprites;
        this.purchased = info.purchased;
        this.selected = info.selected;
        this.playerSprite.sprite = sprites[0];
        for(int i = 0; i < details.Length; i++)
        {
            details[i].text = ""+info.details[i];
        }
    }

    public void ChangeButton(Sprite[] sprites)
    {
        buyButton.image.sprite = sprites[0];
        SpriteState newState = new SpriteState();
        newState = buyButton.spriteState;
        newState.pressedSprite = sprites[1];

        buyButton.spriteState = newState;
    }
}
