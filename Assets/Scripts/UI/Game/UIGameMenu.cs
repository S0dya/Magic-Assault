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

    float[] curPlayerVals = new float[3];
    float[] maxPlayerVals = new float[3];

    //bools
    bool isMenuOpened;

    bool isSettingsOpen;
    bool isStatsOpen;

    void Awake()
    {
        Time.timeScale = 1;

        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { Settings.height, 20 };
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuOpened) OnResumeButton();
            else OnPauseButton();
        }
    }

    //buttons 
    public void OnPauseButton()
    {
        SetPlayerStats();

        isMenuOpened = true;
        OpenTab();
    }
    public void OnResumeButton()
    {
        CloseCurOpenedTab();
        
        isMenuOpened = false;
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

    public void OnButtonPressed() => AudioManager.I.PlayOneShot("button");
    public void OnCancelButtonPressed() => AudioManager.I.PlayOneShot("backButton");
    public void OnTogglePressed() => AudioManager.I.PlayOneShot("toggle");

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
        curPlayerVals[0] = player.curHp; curPlayerVals[1] = player.curMana; curPlayerVals[2] = uiInGame.curExp;
        maxPlayerVals[0] = maxHp; maxPlayerVals[1] = maxMana; maxPlayerVals[2] = uiInGame.curNeededExp;

        for (int i = 0; i < 3; i++)
        {
            barsTexts[i].text = GetAmountText((int)curPlayerVals[i], (int)maxPlayerVals[i]);
            barsImages[i].fillAmount = curPlayerVals[i] / maxPlayerVals[i];
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
