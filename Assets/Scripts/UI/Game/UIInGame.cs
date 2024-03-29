using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using TMPro;

public class UIInGame : SingletonMonobehaviour<UIInGame>
{
    [Header("Other scripts")]
    [SerializeField] UIMultipliers uiMultipliers;
    [SerializeField] UISpells uiSpells;
    [SerializeField] UIUpgrades uiUpgrades;

    [Header("Joystick")]
    [SerializeField] CanvasGroup joysticksCG;
    [SerializeField] FloatingJoystick flJoystick;
    [SerializeField] FixedJoystick fxJoystick;

    [Header("Exp")]
    [SerializeField] Image expLine;
    [SerializeField] GameObject expTextObj;
    [SerializeField] TextMeshProUGUI expText;
    public float curNeededExp;
    public float nextLevelMultiplier;

    [Header("Time")]
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Money and killed enemies")]
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI killedText;

    //local
    Player player;

    //exp
    int curLvl = 0;

    float curExp = 0;
    bool isUpgrading;

    //time
    int curTime;
    int curSecs;
    int curMins;

    //killed and money
    [HideInInspector] public int killedAmount;
    [HideInInspector] public int moneyAmount;

    //cor
    Coroutine timerCor;

    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        SetJoystick();
    }

    void Start()
    {
        StartCoroutine(TImerCor());
    }

    //exp methods
    public void ChangeExp(float val)
    {
        curExp += val;

        SetExpLine();

        //if alr upgrading we skip reaching new level 
        if (!isUpgrading) CheckIfNextLevelIsReached();
    }

    void CheckIfNextLevelIsReached()
    {
        if (curExp >= curNeededExp)//enough exp to reach next level, show upgrade tab
        {
            curExp -= curNeededExp;
            
            NextLevel();
        }
    }

    void NextLevel()
    {
        //is upgrading rn 
        isUpgrading = true;
        //set new lvl, set new needed exp
        curNeededExp *= nextLevelMultiplier;
        curLvl++;

        //show player they reached new level and open upgrade panel
        VisualiseReachinNewLevelStart();
    }

    //set value of curExp from 0 - 1 for fillAmount
    void SetExpLine() => expLine.fillAmount = curExp / curNeededExp;

    //level text methods
    void VisualiseReachinNewLevelStart() => LeanTween.scale(expTextObj, new Vector2(2, 2), 1f).setEase(LeanTweenType.easeOutBack).setOnComplete(() => SetExpText());
    void SetExpText()
    {
        //we set exp one more time, since if we upgraded several times fillAmount will not be changed for last upgrade
        SetExpLine();
        uiUpgrades.OpenTab();

        expText.text = curLvl.ToString();
        //return text of lvl to normal size and check if we need to call upgrade once more
        VisualiseReachinNewLevelEnd();
        isUpgrading = false;
    }
    void VisualiseReachinNewLevelEnd() => LeanTween.scale(expTextObj, new Vector2(1, 1), 0.75f).setEase(LeanTweenType.easeOutBack).setOnComplete(() => CheckIfNextLevelIsReached());


    //time methods
    IEnumerator TImerCor()//we add 1 to timer each second
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            curSecs++;
            SetTime();
        }
    }

    void SetTime()
    {
        if (curSecs == 60)
        {
            curSecs = 0;
            curMins++;

            curTime += 60;
            
            if (curMins == 60 && timerCor != null) StopCoroutine(timerCor);
        }

        var time = new StringBuilder();

        if (curMins < 10) time.Append('0');
        time.Append(curMins.ToString());
        
        time.Append(':');
        
        if (curSecs < 10) time.Append('0');
        time.Append(curSecs.ToString());
        
        timerText.text = time.ToString();
    }

    //money and killed enemies visualisation
    public void AddKill()
    {
        killedAmount++;
        killedText.text = killedAmount.ToString();
    }
    
    public void ChangeMoney(int val)
    {
        moneyAmount += val;
        moneyText.text = moneyAmount.ToString();
    }

    //other methods
    public void OpenSpellsPanel() => uiSpells.OpenTab();
    public void OpenMultipliersPanel() => uiMultipliers.OpenTab();

    void SetJoystick()
    {
        //set joystick's type
        bool isFJ = Settings.isFloatingJoystick;

        if (isFJ) flJoystick.gameObject.SetActive(true);
        else fxJoystick.gameObject.SetActive(true);

        //set this joystick
        player.joystick = isFJ ? flJoystick : fxJoystick;
    }

    public void ToggleJoystickVisibility(float val)
    {
        joysticksCG.alpha = val;
    }
}
