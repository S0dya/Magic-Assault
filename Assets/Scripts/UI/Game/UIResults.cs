using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResults : SingletonMonobehaviour<UIResults>
{
    [Header("Other scripts")]
    [SerializeField] GameData gameData;
    [SerializeField] UIInGame uiInGame;
    [SerializeField] UIInGameStats uiInGameStats;

    [Header("Panel")]
    [SerializeField] CanvasGroup bgCg;
    [SerializeField] CanvasGroup gameoverCg;
    [SerializeField] CanvasGroup panelCg;

    [Header("prefabs")]
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject itemPrefab;

    [Header("Dicts part")]
    [SerializeField] Transform[] linesParents;
    [SerializeField] Transform[] curLinesTransforms;

    [Header("Stats")]
    //header
    [SerializeField] TextMeshProUGUI survivedAmountText;
    [SerializeField] TextMeshProUGUI goldAmountText;
    [SerializeField] TextMeshProUGUI levelAmountText;
    [SerializeField] TextMeshProUGUI killedAmountText;

    //body 
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI characterName;

    [SerializeField] UIResultStatsItem[] multipliersItems;
    [SerializeField] UIResultStatsItem[] spellsItems;


    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI[] elementalDamageTexts;

    //public
    public Dictionary<SO_Item, int> pickedItems;

    //local
    Dictionary<SO_Item, int>[] dics = new Dictionary<SO_Item, int>[3]; // 0 - active upgrades, 1 - passive upgrades, 2 - pickablve items
    int[] curLinesN = new int[3];

    //other data vars
    int killed;
    int goldEarned;
    int levelReached;

    void Start()
    {
        for (int i = 0; i < 3; i++) dics[i] = new Dictionary<SO_Item, int>();
    }

    public void SetTransparentBg()
    {
        GameManager.I.Open(bgCg, 0.2f, 0.4f);
        GameManager.I.Open(gameoverCg, 0.2f);

        uiInGame.StopTimeScale();
    }
    
    public void OpenPanel()
    {
        GameManager.I.Open(bgCg, 0.3f);
        GameManager.I.Close(gameoverCg, 0.2f);

        SetStats();
        SetData();
        GameManager.I.Open(panelCg, 0.6f);
    }

    //buttons
    public void OnQuit()
    {
        OpenPanel();
    }

    public void OnWatchAd()
    {

    }

    public void OnDone()
    {
        LoadingScene.I.OpenMenu();
    }

    //main methods
    void SetStats()
    {
        killed = uiInGame.killedAmount;
        goldEarned = (int)uiInGame.moneyAmount;
        levelReached = uiInGame.curLvl;

        survivedAmountText.text = uiInGame.GetCurTime();
        SetString(goldEarned, goldAmountText);
        SetString(levelReached, levelAmountText);
        SetString(killed, killedAmountText);

        characterImage.sprite = Settings.characterSpriteInGame;
        characterName.text = Settings.characterName;

        for (int i = 0; i < 4; i++)
        {
            multipliersItems[i].SetAmountText((int)(Settings.damageMultipliers[i] * 10));
            spellsItems[i].SetInfo(uiInGameStats.GetSpellsItem(i));

            SetString(gameData.elementalDamageDone[i], elementalDamageTexts[i]);
        }

        for (int i = 0; i < 2; i++) dics[i] = uiInGameStats.GetUpgradesDict(i);
        SetDicsStats();

        SetString(gameData.damageDone, damageText);
    }
    void SetDicsStats()
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (var kvp in dics[i])
            {
                UIResultStatsItem uiItem = CreateItem(i).GetComponent<UIResultStatsItem>();
                uiItem.SetInfo(kvp.Key, kvp.Value);
            }
        }
    }
    GameObject CreateItem(int i)
    {
        if (curLinesN[i] == 10)
        {
            curLinesN[i] = 0;
            curLinesTransforms[i] = CreateNewLine(i);
        }
        else curLinesN[i]++;

        return Instantiate(itemPrefab, curLinesTransforms[i]);
    }
    Transform CreateNewLine(int i)//instantiate new line and set it as current
    {
        return Instantiate(linePrefab, linesParents[i]).GetComponent<Transform>();
    }

    void SetData()
    {
        Settings.totalKilledEnemies += killed;
        Settings.money += goldEarned;
        Settings.totalGoldEarned += goldEarned;

        Settings.totalTimeInGame += gameData.timeInGame;

        Settings.maxTotalLevelReached = Mathf.Max(Settings.maxTotalLevelReached, levelReached);

        Settings.totalDamageDone += gameData.damageDone;
        for (int i = 0; i < 4; i++) Settings.totalElementalDamageDone[i] += gameData.elementalDamageDone[i];
    }

    //outside methods
    public void AddPickableItem(SO_Item item)
    {
        if (dics[2].ContainsKey(item)) dics[2][item]++;
        else dics[2].Add(item, 1);
    }

    //other methods
    public void SetString(int val, TextMeshProUGUI text) => text.text = val.ToString();//a bit of spaghetti
}
