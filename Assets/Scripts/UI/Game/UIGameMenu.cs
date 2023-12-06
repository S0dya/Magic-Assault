using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : UIPanel
{
    [Header("Other scripts")]
    [SerializeField] UISettings uiSettings;

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
        CloseTabInGame();
    }

    public void OnSettingsButton()
    {
        if (isSettingsOpen)
            uiSettings.CloseTabInMenu();
        else
        {
            CloseCurOpenedTab();
            uiSettings.OpenTabInMenu();
        }
    }

    //other methods
    void CloseCurOpenedTab()
    {
        if (isSettingsOpen)
        {
            uiSettings.CloseTabInMenu();
        }
    }

}
