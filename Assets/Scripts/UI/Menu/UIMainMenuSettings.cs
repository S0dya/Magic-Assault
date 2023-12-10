using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuSettings : UISettings
{
    [Header("Tabs")]
    [SerializeField] UIMainMenuSettingTab[] tabs;

    //local
    int curTabOpened;

    void Awake()
    {
        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { Settings.height, 0 };
    }

    protected override void Start()
    {
        base.Start();

        OnTabIcon(0);
    }

    //buttons
    public void OnTabIcon(int i)
    {
        if (curTabOpened == i) return;

        tabs[i].CloseTabInMenu();

        curTabOpened = i;

        tabs[i].OpenTabInMenu();
    }


    public void OnBackButton()
    {
        CloseTabInMenu();
    }
}
