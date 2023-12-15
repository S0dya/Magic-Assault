using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(-1)]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    protected override void Awake()
    {
        base.Awake();

        LoadData();
        //Settings.firstTime = false;
    }

    //UI
    public void Open(CanvasGroup CG, float duration) => Open(CG, duration, 1);
    public void Open(CanvasGroup CG, float duration, float endVal)
    {
        LTDescr tween = LeanTween.alphaCanvas(CG, endVal, duration).setEase(LeanTweenType.easeInOutQuad);
        CG.blocksRaycasts = true;
        SetUseEstimatedTime(tween);
    }

    public void Close(CanvasGroup CG, float duration)
    {
        LTDescr tween = LeanTween.alphaCanvas(CG, 0, duration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => CloseComletely(CG));
        SetUseEstimatedTime(tween);
    }
    void CloseComletely(CanvasGroup CG) => CG.blocksRaycasts = false;

    public void FadeIn(CanvasGroup CG, float durationStart) => LeanTween.alphaCanvas(CG, 1f, durationStart).setEase(LeanTweenType.easeInOutQuad);
    public void FadeInAndOut(CanvasGroup CG, float durationStart, float durationEnd) => LeanTween.alphaCanvas(CG, 1f, durationStart).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => FadeOut(CG, durationEnd));
    public void FadeOut(CanvasGroup CG, float durationEnd) => LeanTween.alphaCanvas(CG, 0f, durationEnd).setEase(LeanTweenType.easeInOutQuad);

    public void FadeInAndOutAndDestroy(GameObject gO, float durationStart, float durationEnd)
    {
        CanvasGroup CG = gO.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(CG, 1f, durationStart).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => FadeOutAndDestroy(gO, CG, durationEnd));
    }
    public void FadeOutAndDestroy(GameObject gO, CanvasGroup CG, float durationEnd)
    {
        LeanTween.alphaCanvas(CG, 0f, durationEnd).setEase(LeanTweenType.easeInOutQuad);
        Destroy(gO);
    }

    public void MoveTransform(RectTransform transform, float x, float y, float duration)
    {
        LTDescr tween = LeanTween.move(transform, new Vector2(x, y), duration).setEaseOutQuad();
        SetUseEstimatedTime(tween);
    }

    public void ChangeScale(GameObject obj, float scale, float duration)
    {
        LTDescr tween = LeanTween.scale(obj, new Vector2(scale, scale), duration).setEase(LeanTweenType.easeOutBack);
        SetUseEstimatedTime(tween);
    }

    public void SetUseEstimatedTime(LTDescr tween) => tween.setUseEstimatedTime(true);

    //other outside methods
    public string GetTime(int secs, int mins)
    {
        StringBuilder time = new StringBuilder();

        if (mins < 10) time.Append('0');
        time.Append(mins.ToString());

        time.Append(':');

        if (secs < 10) time.Append('0');
        time.Append(secs.ToString());

        return time.ToString();
    }

    //save/load
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveData();
        }
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        /*
        PlayerPrefs.SetInt("money", Settings.money);
        for (int i = 0; i < Settings.itemPrices.Length; i++)
        {
            PlayerPrefs.SetInt($"itemPrice {i}", Settings.itemPrices[i]);
        }

        PlayerPrefs.SetInt("firstTime", Settings.firstTime ? 0 : 1);
        PlayerPrefs.SetInt("isMusicOn", Settings.isMusicOn ? 0 : 1);
        */
    }

    public void LoadData() 
    {
        /*
        if (PlayerPrefs.GetInt("firstTime") == 0) return;

        Settings.money = PlayerPrefs.GetInt("money");
        for (int i = 0; i < Settings.itemPrices.Length; i++)
        {
            Settings.itemPrices[i] = PlayerPrefs.GetInt($"itemPrice {i}");
        }

        Settings.isMusicOn = (PlayerPrefs.GetInt("isMusicOn") == 0);
        */
    }
}
