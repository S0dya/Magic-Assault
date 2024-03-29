using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMultipliers : UIPanel
{
    [SerializeField] CanvasGroup quitButtonCG;

    [Header("Bonus")]
    [SerializeField] TextMeshProUGUI bonusText;
    [SerializeField] Color positiveBonusColor;
    [SerializeField] Color zeroBonusColor;
    [SerializeField] Color negativeBonusColor;

    [Header("Multipliers")]
    [SerializeField] UIMultiplier[] multipliers;

    //local
    [HideInInspector] public int[] curMultipliers ;
    int curBonus;
    bool isInteractable;

    void Awake()
    {
        StartEndX = new float[2] { Settings.width, 0 };
        StartEndY = new float[2] { 0, 0 };

        curMultipliers = new int[4];
    }

    //panel 
    public override void OpenTab()
    {
        SetMultipliers();

        base.OpenTab();
    }

    public override void CloseTab()
    {
        base.CloseTab();
     
        for (int i = 0; i < 4; i++) Settings.damageMultipliers[i] = (float)curMultipliers[i] * 0.1f;
    }

    //buttons 
    public void CloseTabButton() => CloseTab();

    //main methods
    void SetMultipliers()
    {
        curBonus = 4;

        for (int i = 0; i < 4; i++)
        {
            curMultipliers[i] = (int)(Settings.damageMultipliers[i] * 10);
            multipliers[i].SetMultiplierText(curMultipliers[i]);
        }

        SetBonusText();
    }

    void SetBonusText()
    {
        Color newColor = new Color();

        if (curBonus > 0) 
        {
            if (isInteractable) ToggleQuitButton(false);
            newColor = positiveBonusColor;
        }
        else if (curBonus == 0)
        {
            if (!isInteractable) ToggleQuitButton(true);
            newColor = zeroBonusColor;
        }
        else
        {
            if (isInteractable) ToggleQuitButton(false);
            newColor = negativeBonusColor;
        }
        bonusText.color = newColor;
        bonusText.text = curBonus.ToString();
    }

    //outside methods 
    public void Increase(int index) => ChangeBonus(-1, index);
    public void Decrease(int index) => ChangeBonus(1, index);

    //other methods
    void ChangeBonus(int val, int index)
    {
        curBonus += val;

        SetBonusText();
        curMultipliers[index] -= val;
        multipliers[index].SetMultiplierText(curMultipliers[index]);
    }

    void ToggleQuitButton(bool val)
    {
        quitButtonCG.interactable = val;
        isInteractable = val;
    }
}
