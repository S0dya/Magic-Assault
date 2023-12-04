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
    [SerializeField] Color[] bonusColors;

    [Header("Multipliers")]
    [SerializeField] UIMultiplier[] multipliers;

    [Header("Arrows")]
    [SerializeField] GameObject[] arrowsObj;

    //local
    [HideInInspector] public int[] curMultipliers ;
    int curBonus;
    bool isInteractable;

    bool isPositive;

    //arrows
    LTDescr[] arrowTweens = new LTDescr[2];
    Image[] arrowImages = new Image[2];
    RectTransform[] arrowsTransform = new RectTransform[2];

    Vector2[] arrowsAnimationStartPos = new Vector2[2];
    float arrowStartAnimationY;
    float[] arrowEndAnimationY = new float[2];

    void Awake()
    {
        StartEndX = new float[2] { Settings.width, 0 };
        StartEndY = new float[2] { 0, 0 };

        curMultipliers = new int[4];

        
        for (int i = 0; i < 2; i++)//get transform and starting position
        {
            arrowsTransform[i] = arrowsObj[i].GetComponent<RectTransform>();
            arrowImages[i] = arrowsObj[i].GetComponent<Image>();
            arrowsAnimationStartPos[i] = arrowsTransform[i].position;
        }
        arrowStartAnimationY = arrowsAnimationStartPos[0].y;//set animation points based on position
        arrowEndAnimationY[0] = arrowStartAnimationY + 30f;
        arrowEndAnimationY[1] = arrowEndAnimationY[0] - 60f;
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
        SetPositive();
        SetBonusText();

        for (int i = 0; i < 4; i++)
        {
            curMultipliers[i] = (int)(Settings.damageMultipliers[i] * 10);
            multipliers[i].SetMultiplierText(curMultipliers[i]);
        }
    }

    void CheckBonus()
    {
        if (curBonus == 1) 
        {
            SetPositive();
        }
        else if (curBonus == 0)
        {
            //stop animation of arrows
            StopArrowAnimation(0);
            StopArrowAnimation(1);

            ToggleQuitButton(true);
            bonusText.color = bonusColors[1];
        }
        else if (curBonus == -1)
        {
            StopArrowAnimation(1);
            StartArrowAnimation(0);

            ToggleQuitButton(false);
            bonusText.color = bonusColors[2];
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

    void SetBonusText() => bonusText.text = curBonus.ToString();

    //outside methods 
    public void Increase(int index) => ChangeBonus(-1, index);
    public void Decrease(int index) => ChangeBonus(1, index);

    //other methods
    void ChangeBonus(int val, int index)
    {
        curBonus += val;

        CheckBonus();
        SetBonusText();
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
            //set pos to start
            arrowsTransform[i].anchoredPosition = Vector2.zero;
            //move to end position in loop
            arrowTweens[i] = LeanTween.moveY(arrowsObj[i], arrowEndAnimationY[i], 0.5f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong();
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