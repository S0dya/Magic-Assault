using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : UIPanel
{
    [Header("Other scripts")]
    [SerializeField] UIInGameSettings uiSettings;
    [SerializeField] UIInGameStats uiInGameStats;

    //local
    bool isSettingsOpen;
    bool isStatsOpen;


    void Awake()
    {
        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { Settings.height, 0 };
    }

    //buttons 
    public void OnPauseButton()
    {
        OpenTabInGame();
    }
    public void OnResumeButton()
    {
        CloseCurOpenedTab();
        CloseTabInGame();
    }

    public void OnQuitButton()
    {

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
            uiSettings.OpenTabInMenu();
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
            uiInGameStats.OpenTabInMenu();
        }
    }

    //other methods
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
    

    void CloseSettings()
    {
        uiSettings.CloseTabInMenu();
        isSettingsOpen = false;
    }

    void CloseStats()
    {
        uiInGameStats.CloseTabInMenu();
        isStatsOpen= false;
    }
}
