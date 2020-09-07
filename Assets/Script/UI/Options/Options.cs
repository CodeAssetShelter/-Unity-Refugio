using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider bgmSlider;
    public Sprite[] bgmSprites = new Sprite[5];
    public Image bgmSprite;

    [Space(20)]
    public Slider effectSlider;
    public Sprite[] effectSprites = new Sprite[5];
    public Image effectSprite;

    [Space(20)]
    public GameObject buttonGPGS;
    public GameObject buttonPauseOptions;

    SoundManager soundManager;

    // Start is called before the first frame update
    void OnEnable()
    {
        soundManager = SoundManager.Instance;

        SaveData.Instance.LoadVolumeData();

        if (bgmSlider != null)
        {
            bgmSlider.maxValue = effectSlider.maxValue = 1.0f;

            bgmSlider.value = SoundManager.Instance.bgmVolume;
            effectSlider.value = SoundManager.Instance.effectVolume;

            StartCoroutine(SoundSetup());
        }
    }

    public void SetOptionMode(bool isGamePlay)
    {
        buttonGPGS.SetActive(!isGamePlay);
        buttonPauseOptions.SetActive(isGamePlay);
    }

    IEnumerator SoundSetup()
    {
        float bgmChecker = 0, efxChecker = 0;

        while (true)
        {
            //if (bgmChecker == bgmSlider.value && efxChecker == effectSlider.value)
            //{
            //    yield return null;
            //}

            if (bgmSlider.value != 0) bgmMute = false;
            if (effectSlider.value != 0) efxMute = false;

            soundManager.SetVolume(bgmSlider.value, effectSlider.value);
            SetSoundSprite();

            bgmChecker = bgmSlider.value;
            efxChecker = effectSlider.value;
            yield return null;
        }
    }
    private void SetSoundSprite()
    {
        if (bgmSlider.value <= 0f)
        {
            bgmSprite.sprite = bgmSprites[0];
        }
        else if (bgmSlider.value <= 0.20f)
        {
            bgmSprite.sprite = bgmSprites[1];
        }
        else if (bgmSlider.value <= 0.50f)
        {
            bgmSprite.sprite = bgmSprites[2];
        }
        else if (bgmSlider.value <= 0.75f)
        {
            bgmSprite.sprite = bgmSprites[3];
        }
        else
        {
            bgmSprite.sprite = bgmSprites[4];
        }

        ////////////////////////////////////
        ///

        if (effectSlider.value <= 0f)
        {
            effectSprite.sprite = effectSprites[0];
        }
        else if (effectSlider.value <= 0.20f)
        {
            effectSprite.sprite = effectSprites[1];
        }
        else if (effectSlider.value <= 0.50f)
        {
            effectSprite.sprite = effectSprites[2];
        }
        else if (effectSlider.value <= 0.75f)
        {
            effectSprite.sprite = effectSprites[3];
        }
        else
        {
            effectSprite.sprite = effectSprites[4];
        }
    }

    bool bgmMute, efxMute = false;
    float bgmSaver, efxSaver = 0;
    public void SetMute(bool isBGM)
    {
        if (isBGM == true)
        {
            if (bgmMute == false)
            {
                if (bgmSlider.value == 0)
                {
                    bgmSaver = bgmSlider.maxValue * 0.25f;
                    bgmSlider.value = bgmSaver;
                    soundManager.SetVolume(bgmSlider.value, effectSlider.value);
                    bgmMute = true;
                }
                else
                {
                    bgmSaver = bgmSlider.value;
                    bgmSlider.value = 0;
                    soundManager.SetVolume(bgmSlider.value, effectSlider.value);
                }
                bgmMute = !bgmMute;
            }
            else
            {
                bgmSlider.value = bgmSaver;
                soundManager.SetVolume(bgmSlider.value, effectSlider.value);
                bgmMute = !bgmMute;
            }
        }
        else
        {
            if (efxMute == false)
            {
                if (effectSlider.value == 0)
                {
                    efxSaver = effectSlider.maxValue * 0.25f;
                    effectSlider.value = efxSaver;
                    soundManager.SetVolume(bgmSlider.maxValue * 0.25f, effectSlider.value);
                    efxMute = true;

                    soundManager.ActiveUiClickSound();
                }
                else
                {
                    efxSaver = effectSlider.value;
                    effectSlider.value = 0;
                    soundManager.SetVolume(bgmSlider.value, effectSlider.value);
                }
                efxMute = !efxMute;
            }
            else
            {
                effectSlider.value = efxSaver;
                soundManager.SetVolume(bgmSlider.value, effectSlider.value);
                efxMute = !efxMute;
                soundManager.ActiveUiClickSound();
            }
        }

        SetSoundSprite();
    }

    private void OnDisable()
    {
        if (soundManager == null) return;

        soundManager.SetVolume(bgmSlider.value, effectSlider.value);
        StopAllCoroutines();
    }

    private void OnApplicationQuit()
    {
        if (soundManager == null) return;

        soundManager.SetVolume(bgmSlider.value, effectSlider.value);
        StopAllCoroutines();
    }
}
