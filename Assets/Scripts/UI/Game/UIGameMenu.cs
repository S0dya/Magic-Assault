using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class UIGameMenu : UIPanelGame
{
    [Header("Other scripts")]
    [SerializeField] UIInGameSettings uiSettings;
    [SerializeField] UIInGameStats uiInGameStats;
    [SerializeField] UIInGame uiInGame;

    [Header("Character portrait")]
    [SerializeField] Image characterPortrait;

    [Header("Player stats")]
    [SerializeField] TextMeshProUGUI[] barsTexts;
    [SerializeField] Image[] barsImages;
    [SerializeField] UIResultStatsItem[] multipliersItems;

    //local
    //player stats
    Player player;
    
    float maxHp;
    float maxMana;

    //bools
    bool isSettingsOpen;
    bool isStatsOpen;

    void Awake()
    {
        Time.timeScale = 1;

        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { Settings.height, 0 };
    }

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        maxHp = player.maxHp;
        maxMana = player.maxMana;

        characterPortrait.sprite = Settings.characterSprite;

        for (int i = 0; i < 4; i++) SetMultipliersTexts(i, (int)(Settings.damageMultipliers[i] * 10));
    }

    //buttons 
    public void OnPauseButton()
    {
        SetPlayerStats();

        OpenTab();
    }
    public void OnResumeButton()
    {
        CloseCurOpenedTab();
        
        CloseTab();

        uiInGameStats.StopHighlight();// close highlight stats highlight if its on
    }

    public void OnQuitButton()
    {
        LoadingScene.I.OpenMenu();
    }

    public void OnSettingsButton()
    {
        if (isSettingsOpen)
        {
            CloseSettings();
        }
        else
        {
            CloseCurOpenedTab();
            isSettingsOpen = true;
            uiSettings.OpenTab();
        }

    }

    public void OnStatsButton()
    {
        if (isStatsOpen)
        {
            CloseStats();
        }
        else
        {
            CloseCurOpenedTab();
            isStatsOpen = true;
            uiInGameStats.OpenTab();
        }
    }

    //main methods
    void CloseCurOpenedTab()
    {
        if (isSettingsOpen)
        {
            CloseSettings();
        }
        else if (isStatsOpen)
        {
            CloseStats();
        }
    }

    public void SetPlayerStats()
    {
        float[] curVals = new float[3] { player.curHp, player.curMana, uiInGame.curExp };
        float[] maxVals = new float[3] { maxHp, maxMana, uiInGame.curNeededExp };

        for (int i = 0; i < 3; i++)
        {
            barsTexts[i].text = GetAmountText((int)curVals[i], (int)maxVals[i]);
            barsImages[i].fillAmount = curVals[i] / maxVals[i];
        }
    }

    string GetAmountText(int curVal, int maxVal)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(curVal.ToString());
        sb.Append("/");
        sb.Append(maxVal.ToString());

        return sb.ToString();
    }

    public void SetMultipliersTexts(int i, int amount) => multipliersItems[i].SetAmountText(amount);

    //other methods
    void CloseSettings()
    {
        uiSettings.CloseTab();
        isSettingsOpen = false;
    }

    void CloseStats()
    {
        uiInGameStats.CloseTab();
        isStatsOpen= false;
    }
}
