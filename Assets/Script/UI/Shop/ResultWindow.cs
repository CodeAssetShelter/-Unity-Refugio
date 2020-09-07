using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultWindow : MonoBehaviour
{
    [Header("- Lack Of Coin")]
    public GameObject lackOfCoin;
    public Image bar;
    public Image goodsSprite;
    public Transform gameObjectMycoin;

    [Header("- Success Purchase")]
    public GameObject successPurchase;
    public Image centerSprite;


    [Header("- Ads Rewards")]
    public GameObject coinReward;

    [Header("- Audio")]
    public AudioSource audioSource;
    public AudioClip audioClipLackOfCoinClip, audioClipSuccessPurchase;

    private RectTransform barRect;
    private float yStart, yEnd;

    //[Range(0, 1.0f)]
    //public float filled;
    //public int testPrice;
    //public int testCoin;

    Vector3 newPos;
    private void Awake()
    {
        barRect = bar.GetComponent<RectTransform>();
        yStart = bar.transform.localPosition.y - (barRect.sizeDelta.x * 0.5f);
        yEnd = bar.transform.localPosition.y + (barRect.sizeDelta.x * 0.5f);
    }

    public void InitWindowLackOfCoin(int goodsPrice, int myCoin, Sprite goodsSprite = null)
    {
        if (goodsSprite != null)
        {
            this.goodsSprite.sprite = goodsSprite;
        }

        lackOfCoin.SetActive(true);

        audioSource.PlayOneShot(audioClipLackOfCoinClip, SoundManager.Instance.effectVolume);

        newPos = gameObjectMycoin.localPosition;
        float fill = (float)myCoin / goodsPrice;
        bar.fillAmount = fill;
        
        newPos.y = yStart + ((yEnd - yStart) * fill);

        gameObjectMycoin.localPosition = newPos;

        newPos = this.goodsSprite.transform.localPosition;
        newPos.y = yEnd;
        this.goodsSprite.transform.localPosition = newPos;
    }

    public void InitWindowSuccessPurchase(Sprite goodsSprite)
    {
        audioSource.PlayOneShot(audioClipSuccessPurchase, SoundManager.Instance.effectVolume);

        successPurchase.SetActive(true);
        centerSprite.sprite = goodsSprite;
    }

    public void InitAdsResult(int textValue)
    {
        coinReward.SetActive(true);
        coinReward.transform.GetChild(1).GetComponent<Text>().text = "+ " + textValue;
    }

    public void DeactiveWindows()
    {
        if (lackOfCoin.activeSelf == true) lackOfCoin.SetActive(false);
        if (successPurchase.activeSelf == true) successPurchase.SetActive(false);
        if (coinReward.activeSelf == true) coinReward.SetActive(false);
        
        gameObject.SetActive(false);
    }
}
