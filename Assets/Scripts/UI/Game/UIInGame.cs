using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class UIInGame : SingletonMonobehaviour<UIInGame>
{
    [Header("Other scripts")]
    [SerializeField] UIMultipliers uiMultipliers;
    [SerializeField] UISpells uiSpells;
    [SerializeField] UIUpgrades uiUpgrades;
    [SerializeField] GameData gameData;
    [SerializeField] DrawManager drawManager;
    [SerializeField] Map map;

    [Header("Borders")]
    [SerializeField] Image[] borderImages;
    [SerializeField] Color borderTransparentColor;
    [SerializeField] Color borderSolidColor;

    [Header("Joystick")]
    [SerializeField] CanvasGroup joysticksCG;
    [SerializeField] FloatingJoystick flJoystick;
    [SerializeField] FixedJoystick fxJoystick;

    [SerializeField] Image[] handlesImages;
    [SerializeField] Sprite[] handles;
    [SerializeField] Sprite defaultHandle;

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

    [SerializeField] Color hpRestoreColor;
    [SerializeField] Color manaRestoreColor;

    [Header("Upgrades")]
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform activeUpgradesLineTransform;

    //local
    GameManager gameManager;
    Player player;

    //exp
    [HideInInspector] public int curLvl = 0;

    [HideInInspector] public float curExp = 0;
    bool isUpgrading;

    //time
    int curSecs;
    int curMins;

    //killed and money
    [HideInInspector] public int killedAmount;
    [HideInInspector] public float moneyAmount;

    //joystick
    Image handleImage;

    //cor
    Coroutine timerCor;

    void Start()
    {
        for (int i = 0; i < borderImages.Length; i++) borderImages[i].color = (Settings.bordersTransparent ? borderTransparentColor : borderSolidColor);

        gameManager = GameManager.I;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        SetJoystick();

        SetHandleSprite();

        StartCoroutine(TimerCor());
    }

    //exp methods
    public void ChangeExp(float val)
    {
        curExp += val * gameData.growth;

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

        //show player they reached new level and open upgrade panel
        VisualiseReachinNewLevelStart();
    }

    //set value of curExp from 0 - 1 for fillAmount
    void SetExpLine() => expLine.fillAmount = curExp / curNeededExp;

    //level text methods
    void VisualiseReachinNewLevelStart() => LeanTween.scale(expTextObj, new Vector2(2, 2), 0.5f).setEase(LeanTweenType.easeOutBack).setOnComplete(() => SetExpText());
    void SetExpText()
    {
        //we set exp one more time, since if we upgraded several times fillAmount will not be changed for last upgrade
        SetExpLine();
        uiUpgrades.OpenTab();

        curLvl++;
        expText.text = curLvl.ToString() + "lvl";
        //return text of lvl to normal size and check if we need to call upgrade once more
        VisualiseReachinNewLevelEnd();
        isUpgrading = false;
    }
    void VisualiseReachinNewLevelEnd() => LeanTween.scale(expTextObj, new Vector2(1, 1), 0.25f).setEase(LeanTweenType.easeOutBack).setOnComplete(() => CheckIfNextLevelIsReached());


    //time methods
    IEnumerator TimerCor()//we add 1 to timer each second
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
            map.NextWave();
            curSecs = 0;
            curMins++;

            gameData.timeInGame += 60;
            
            if (curMins == 60 && timerCor != null) StopCoroutine(timerCor);
        }
        
        timerText.text = gameManager.GetTime(curSecs, curMins);
    }

    //money and killed enemies visualisation
    public void AddKill()
    {
        killedAmount++;
        killedText.text = killedAmount.ToString();
    }
    
    public void ChangeMoney(float val)
    {
        moneyAmount += val * gameData.greed;
        moneyText.text = ((int)moneyAmount).ToString();
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

        handleImage = isFlJ ? handlesImages[0] : handlesImages[1];

        //set this joystick
        player.SetJoystick(isFlJ? flJoystick : fxJoystick);
    }
    public void ToggleJoystickVisibility(float val) => joysticksCG.alpha = val;

    //pick ups on no upgrades
    public void UsePickUpUpgrade(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.PickUpsManaPotion: player.RestoreManaWithItem(75f); break;
            case UpgradeType.PickUpsCoinsBag: ChangeMoney(25); break;
            case UpgradeType.PickUpsHealthPotion: player.RestoreHPWithItem(75f); break;
            default: break;
        }
    }

    //damage texts
    public void InstantiateTextOnDamage(Vector2 pos, int amount, int typeOfDamage)//instantiate text above enemy
    {
        if (!Settings.showDamageNumbers) return;

        Color color = new Color();
        int textType = Mathf.Min(amount / 30, 3);//get type of text

        switch (typeOfDamage)//get color
        {
            case -1: color = nonElementDamageColors[textType]; break;
            case 0: color = fireDamageColors[textType]; break;
            case 1: color = waterDamageColors[textType]; break;
            case 2: color = earthDamageColors[textType]; break;
            case 3: color = airDamageColors[textType]; break;
            default: break;
        }

        SetDamageText(InstantiateText(pos, 1 + (float)textType * 0.1f), amount, color);
    }
    //instantiate text above player
    public void InstantiateTextOnHPRestore(Vector2 pos, int amount)
    {
        SetDamageText(InstantiateText(pos, 1.25f), amount, hpRestoreColor);
    }
    public void InstantiateTextOnManaRestore(Vector2 pos, int amount)
    {
        SetDamageText(InstantiateText(pos, 1.25f), amount, manaRestoreColor);
    }

    GameObject InstantiateText(Vector2 pos, float scale)
    {
        //instantiate text obj
        GameObject textObj = Instantiate(textPrefab, pos, Quaternion.identity, damageTextParent);
        textObj.transform.localScale = new Vector2(scale, scale);//set scale 

        return textObj;
    }

    void SetDamageText(GameObject textObj, int amount, Color color)
    {
        //set amount of damage and color of text
        TextMeshPro textTmp = textObj.GetComponent<TextMeshPro>();
        textTmp.text = amount.ToString();
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

        float endValLimit = y + 0.75f;
        float endVal = endValLimit + 0.75f;

        while (textTransform.position.y < endValLimit)
        {
            y = Mathf.Lerp(y, endVal, 0.5f * Time.deltaTime);
            textTransform.position = new Vector2(x, y);

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        Destroy(textObj);
    }

    //upgrades showcase
    public void ShowcaseActiveUpgrade(SO_Item item) => InstantiateItem(activeUpgradesLineTransform).SetInfo(item);

    UIStatsItem InstantiateItem(Transform parent)
    {
        return Instantiate(itemPrefab, parent).GetComponent<UIStatsItem>();
    }

    //other outside methods
    public void StopTimeScale()
    {
        drawManager.StopCreatingSpell();

        ToggleTimeScale(false);
    }

    public void ToggleTimeScale(bool val)
    {
        float floatVal = val ? 1 : 0;

        StartCoroutine(ToggleOnUICor(!val));
        Time.timeScale = floatVal;

        ToggleJoystickVisibility(floatVal);
    }

    //joystick outside methods
    public void SetHandleSprite()
    {
        float[] elemtalsMultipliers = Settings.damageMultipliers.ToArray();

        float firstMax = elemtalsMultipliers.Max();
        int firstMaxI = Array.IndexOf(elemtalsMultipliers, firstMax);

        elemtalsMultipliers[firstMaxI] = -1;

        float secondMax = elemtalsMultipliers.Max();

        handleImage.sprite = (firstMax != secondMax ? handles[firstMaxI] : defaultHandle);
    }

    //coroutine will make sure ui interaction is not considered in-game interaction
    IEnumerator ToggleOnUICor(bool val)
    {
        yield return null;

        drawManager.isOnUI = val;
        if (!val) drawManager.StopCreatingSpell();
    }

    public string GetCurTime()
    {
        return timerText.text;
    }
}
