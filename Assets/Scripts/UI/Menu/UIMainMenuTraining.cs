using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuTraining : UIPanelMenu
{
    [Header("Other scripts")]
    [SerializeField] UISettings uiSettings;

    [Header("Toggles")]
    [SerializeField] UISettingToggle uiSettingToggleShow;

    void Awake()
    {
        StartEndX = new float[2] { -Settings.width, -20 };
        StartEndY = new float[2] { 0, 0 };
    }

    protected override void Start()
    {
        base.Start();

        uiSettings.ToggleSetting(uiSettingToggleShow, !Settings.showTraining);
    }

    //toggles
    public void OnToggleShowTraining()
    {
        bool toggle = Settings.showTraining = !Settings.showTraining;
        uiSettings.ToggleSetting(uiSettingToggleShow, toggle);
    }

    //buttons
    public void OnContinueButton()
    {
        UIMainMenu.I.StartGame();
    }
}
