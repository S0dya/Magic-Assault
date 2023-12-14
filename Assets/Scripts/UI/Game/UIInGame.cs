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
    [SerializeField] GameData gameData;

    [Header("Borders")]
    [SerializeField] Image[] borderImages;
    [SerializeField] Color borderTransparentColor;
    [SerializeField] Color borderSolidColor;

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

    [Header("Damage text")]
    [SerializeField] GameObject textPrefab;
    [SerializeField] Transform damageTextParent;

    //colors
    [SerializeField] Color[] nonElementDamageColors;
    [SerializeField] Color[] fireDamageColors;
    [SerializeField] Color[] waterDamageColors;
    [SerializeField] Color[] earthDamageColors;
    [SerializeField] Color[] airDamageColors;

    //local
    Player player;

    //exp
    int curLvl = 0;

    [HideInInspector] public float curExp = 0;
    bool isUpgrading;

    //time
    int curSecs;
    int curMins;

    //killed and money
    [HideInInspector] public int killedAmount;
    [HideInInspector] public int moneyAmount;

    //cor
    Coroutine timerCor;

    void Start()
    {
        for (int i = 0; i < borderImages.Length; i++) borderImages[i].color = (Settings.bordersTransparent ? borderTransparentColor : borderSolidColor);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        SetJoystick();

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
        gameData.levelReached += curLvl++;

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
            yield return new WaitForSeconds(1f);

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

            gameData.timeInGame += 60;
            
            if (curMins == 60 && timerCor != null) StopCoroutine(timerCor);
        }

        StringBuilder time = new StringBuilder();

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

    public void SetJoystick()
    {
        //set joystick's type
        bool isFlJ = Settings.isFloatingJoystick;

        flJoystick.gameObject.SetActive(isFlJ);
        fxJoystick.gameObject.SetActive(!isFlJ);

        //set this joystick
        player.joystick = isFlJ ? flJoystick : fxJoystick;
    }

    public void ToggleJoystickVisibility(float val)
    {
        joysticksCG.alpha = val;
    }

    //damage texts
    public void InstantiateTextOnDamage(Vector2 pos, int amountOfDamage, int typeOfDamage)//instantiate text above enemy
    {
        if (!Settings.showDamageNumbers) return;

        Color color = new Color();
        int textType = Mathf.Min(amountOfDamage / 30, 3);//get type of text

        switch (typeOfDamage)//get color
        {
            case -1:
                color = nonElementDamageColors[textType];
                break;
            case 0:
                color = fireDamageColors[textType];
                break;
            case 1:
                color = waterDamageColors[textType];
                break;
            case 2:
                color = earthDamageColors[textType];
                break;
            case 3:
                color = airDamageColors[textType];
                break;
            default: break;
        }

        //instantiate text obj
        GameObject textObj = Instantiate(textPrefab, pos, Quaternion.identity, damageTextParent);

        var startScaleValue = 1 + (float)textType * 0.25f; //get scale size
        textObj.transform.localScale = new Vector2(startScaleValue, startScaleValue);//set scale 

        //set amount of damage and color of text
        TextMeshPro textTmp = textObj.GetComponent<TextMeshPro>();
        textTmp.text = amountOfDamage.ToString();
        textTmp.color = color;

        //move text up and destroy on complete
        StartCoroutine(MoveTextCor(textObj));
    }

    IEnumerator MoveTextCor(GameObject textObj)
    {
        Transform textTransform = textObj.transform;
        Vector2 textPos = textTransform.position;
        float x = textPos.x;
        float y = textPos.y;

        float endValLimit = textPos.y + 2;
        float endVal = endValLimit + 2;

        while (textTransform.position.y < endValLimit)
        {
            y = Mathf.Lerp(y, endVal, 0.5f * Time.deltaTime);
            textTransform.position = new Vector2(x, y);

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        Destroy(textObj);
    }
}
