using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGame : SingletonMonobehaviour<UIInGame>
{
    [Header("Exp")]
    [SerializeField] Image expLine;
    [SerializeField] GameObject expTextObj;
    [SerializeField] TextMeshProUGUI expText;
    public float curNeededExp;
    public float nextLevelMultiplier;

    [Header("Upgrade")]
    [SerializeField] UIUpgradePanel uiUpgradePanel;

    //local
    int curLvl = 0;
    float curExp = 0;

    protected override void Awake()
    {
        base.Awake();

    }

    //exp methods
    public void ChangeExp(float val)
    {
        float newVal = curExp + val;

        if (newVal > curNeededExp)//enough exp to reach next level, show upgrade tab
        {
            curExp = newVal - curNeededExp;

            NextLevel();
        }
        else
        {
            curExp = newVal;
        }


        SetExpLine();
    }

    void NextLevel()
    {
        curNeededExp *= nextLevelMultiplier;
        curLvl++;

        VisualiseReachinNewLevelStart();
    }

    void SetExpLine() => expLine.fillAmount = curExp / curNeededExp;

    //level text methods
    void VisualiseReachinNewLevelStart() => LeanTween.scale(expTextObj, new Vector2(2, 2), 1f).setEase(LeanTweenType.easeOutBack).setOnComplete(() => SetExpText());
    void SetExpText()
    {
        uiUpgradePanel.OpenUpgradeTab();

        expText.text = curLvl.ToString();
        VisualiseReachinNewLevelEnd();
    }
    void VisualiseReachinNewLevelEnd() => LeanTween.scale(expTextObj, new Vector2(1, 1), 0.75f).setEase(LeanTweenType.easeOutBack);



}
