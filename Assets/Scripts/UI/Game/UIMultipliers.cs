using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMultipliers : UIPanelGame
{
    [Header("Other scripts")]
    [SerializeField] UIGameMenu uiGameMenu;

    [Header("Canvas group of a button")]
    [SerializeField] CanvasGroup quitButtonCG;

    [Header("Bonus")]
    [SerializeField] TextMeshProUGUI bonusText;
    [SerializeField] Color[] bonusColors;

    [Header("Multipliers")]
    [SerializeField] UIMultiplier[] multipliers;

    [Header("Arrows")]
    [SerializeField] GameObject[] arrowsObj;

    //local
    [HideInInspector] public int[] curMultipliers;
    int[] damageMultipliersMins = new int[4];
    int curBonus;
    bool isInteractable;

    bool isPositive;

    //arrows
    LTDescr[] arrowTweens = new LTDescr[2];
    Image[] arrowImages = new Image[2];

    float[] arrowEndAnimationY = new float[2] { 20f, -20f };

    void Awake()
    {
        StartEndX = new float[2] { Settings.width, 20 };
        StartEndY = new float[2] { 0, 0 };

        for (int i = 0; i < 4; i++) damageMultipliersMins[i] = (int)(Settings.damageMultipliersMins[i] * 10);
        curMultipliers = new int[4];
        
        for (int i = 0; i < 2; i++) arrowImages[i] = arrowsObj[i].GetComponent<Image>();
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

        for (int i = 0; i < 4; i++)
        {
            int curVal = curMultipliers[i];
            uiGameMenu.SetMultipliersTexts(i, curVal);
            Settings.damageMultipliers[i] = (float)curVal * 0.1f;
            UIInGame.I.SetHandleSprite();
        }

    }

    //buttons 
    public void CloseTabButton() => CloseTab();

    //main methods
    void SetMultipliers()
    {
        curBonus = 4;
        SetPositive();
        SetString(curBonus, bonusText);

        for (int i = 0; i < 4; i++)
        {
            curMultipliers[i] = (int)(Settings.damageMultipliers[i] * 10);
            multipliers[i].SetMultiplierText(curMultipliers[i]);
        }
    }

    void CheckBonus()
    {
        switch (curBonus)
        {
            case 1:
                SetPositive();
                break;
            case 0:
                //stop animation of arrows
                StopArrowAnimation(0);
                StopArrowAnimation(1);

                ToggleQuitButton(true);
                bonusText.color = bonusColors[1];
                break;
            case -1:
                StopArrowAnimation(1);
                StartArrowAnimation(0);

                ToggleQuitButton(false);
                bonusText.color = bonusColors[2];
                break;
            default: break;
        }
    }

    void SetPositive()
    {
        //toggle animation of arrow
        StopArrowAnimation(0);
        StartArrowAnimation(1);

        ToggleQuitButton(false);
        bonusText.color = bonusColors[0];
    }

    //outside methods 
    public void Increase(int index)
    {
        if (curBonus != -50) ChangeBonus(-1, index);
    }
    public void Decrease(int index)
    {
        if (CanDecrease(index) && curBonus != 50) ChangeBonus(1, index);
    }
    bool CanDecrease(int index)
    {
        return curMultipliers[index] - 1 >= damageMultipliersMins[index];
    }


    //other methods
    void ChangeBonus(int val, int index)
    {
        curBonus += val;

        CheckBonus();
        SetString(curBonus, bonusText);
        curMultipliers[index] -= val;
        multipliers[index].SetMultiplierText(curMultipliers[index]);
    }

    void ToggleQuitButton(bool val)
    {
        quitButtonCG.interactable = val;
        isInteractable = val;
    }

    //arrows
    void StartArrowAnimation(int i)
    {
        if (arrowTweens[i] == null)//only play if arrow animation is not playing
        {
            //move to end position in loop
            arrowTweens[i] = LeanTween.moveLocalY(arrowsObj[i], -30, 0.5f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong();
            //make animation work despite scaled time being 0
            GameManager.I.SetUseEstimatedTime(arrowTweens[i]);

            ToggleArrowImage(i, true);
        }
    }
    
    void StopArrowAnimation(int i)
    {
        if (arrowTweens[i] != null)//only stop if arrow animation is playing
        {
            ToggleArrowImage(i, false);

            LeanTween.cancel(arrowTweens[i].id);
            arrowTweens[i] = null; 
        }
    }
    void ToggleArrowImage(int i, bool val) => arrowImages[i].enabled = val;

}
