using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    /// <summary>
    /// script sets several settings at once, some settings are already described in UISettings script
    /// </summary>

public class UIMainMenuSettings : UISettings
{
    [Header("Tabs")]
    [SerializeField] UIMainMenuSettingTab[] tabs;

    [Header("Second music")]
    [SerializeField] Slider[] soundSlidersSecond;

    [Header("Second toggles")]
    [SerializeField] UISettingToggle uiSettingToggleDamageNumbersSecond;
    [SerializeField] UISettingToggle uiSettingToggleShowBloodSecond;

    [SerializeField] UISettingToggle uiSettingToggleAdditionalParticles;

    [Header("Second options")]
    [SerializeField] UISettingToggle uiSettingToggleFloatingJoystickSecond;
    [SerializeField] UISettingToggle uiSettingToggleFixedJoystickSecond;
    
    [SerializeField] UISettingToggle uiSettingToggleTransparentBorders;
    [SerializeField] UISettingToggle uiSettingToggleSolidBorders;

    [SerializeField] UISettingToggle uiSettingToggleQualityToMedium;
    [SerializeField] UISettingToggle uiSettingToggleQualityToLow;

    //local
    int curTabOpened = 0;

    //initializing
    void Awake()
    {
        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { Settings.height, 20 };
    }

    protected override void Start()
    {
        base.Start();

        SetSettings();
        tabs[0].OpenTab();//open favourites tab 
    }

    protected override void SetSettings()//set settings at start of the game
    {
        base.SetSettings();

        //sliders
        for (int i = 0; i < soundSlidersSecond.Length; i++) soundSlidersSecond[i].value = Settings.musicStats[i];

        //toggles
        ToggleSetting(uiSettingToggleDamageNumbersSecond, Settings.showDamageNumbers);
        ToggleSetting(uiSettingToggleShowBloodSecond, Settings.showBlood);
        ToggleSetting(uiSettingToggleAdditionalParticles, Settings.additionalParticles);

        //two options
        ToggleSettingTwoOptions(uiSettingToggleFloatingJoystickSecond, uiSettingToggleFixedJoystickSecond, Settings.isFloatingJoystick);
        ToggleSettingTwoOptions(uiSettingToggleTransparentBorders, uiSettingToggleSolidBorders, Settings.bordersTransparent);
        ToggleSettingTwoOptions(uiSettingToggleQualityToMedium, uiSettingToggleQualityToLow, Settings.isQualityMedium);
    }

    //buttons
    public void OnTabIcon(int i)//close last tab and open new one if pressed tab isnt alr opened
    {
        if (curTabOpened == i) return;

        tabs[curTabOpened].CloseTab();

        curTabOpened = i;

        tabs[curTabOpened].OpenTab();
    }


    public void OnBackButton()
    {
        CloseTab();

        UIMainMenu.I.optionsOpen = false;
    }

    //sliders
    public void OnSoundChangeFirst(int index)//(a bit of spaghetti if youre hungry)
    {
        OnSoundChange(index);

        soundSlidersSecond[index].value = soundSliders[index].value;
    }
    public void OnSoundChangeSecond(int index)
    {
        OnSoundChange(index);

        soundSliders[index].value = soundSlidersSecond[index].value;
    }

    //toggles
    public override void OnToggleDamageNumbers()
    {
        base.OnToggleDamageNumbers();

        ToggleSetting(uiSettingToggleDamageNumbersSecond, Settings.showDamageNumbers);
    }
    public override void OnToggleShowBlood()
    {
        base.OnToggleShowBlood();

        ToggleSetting(uiSettingToggleShowBloodSecond, Settings.showBlood);
    }

    public void OnToggleAdditionalParticles()
    {
        bool toggle = Settings.additionalParticles = !Settings.additionalParticles;
        ToggleSetting(uiSettingToggleAdditionalParticles, toggle);
    }

    //toggle with 2 options
    public override void OnChangeJoystickToFloating()
    {
        base.OnChangeJoystickToFloating();

        ToggleSettingTwoOptions(uiSettingToggleFloatingJoystickSecond, uiSettingToggleFixedJoystickSecond);
    }
    public override void OnChangeJoystickToFixed()
    {
        base.OnChangeJoystickToFixed();

        ToggleSettingTwoOptions(uiSettingToggleFixedJoystickSecond, uiSettingToggleFloatingJoystickSecond);
    }

    public void OnChangeBordersToTransparent()
    {
        if (Settings.bordersTransparent) return;

        ToggleSettingTwoOptions(uiSettingToggleTransparentBorders, uiSettingToggleSolidBorders);

        Settings.bordersTransparent = true;
    }
    public void OnChangeBordersToSolid()
    {
        if (!Settings.bordersTransparent) return;

        ToggleSettingTwoOptions(uiSettingToggleSolidBorders, uiSettingToggleTransparentBorders);

        Settings.bordersTransparent = false;
    }

    public void OnChangeQualityToMedium()
    {
        if (Settings.isQualityMedium) return;

        ToggleSettingTwoOptions(uiSettingToggleQualityToMedium, uiSettingToggleQualityToLow);

        Settings.isQualityMedium = true;
    }
    public void OnChangeQualityToLow()
    {
        if (!Settings.isQualityMedium) return;

        ToggleSettingTwoOptions(uiSettingToggleQualityToLow, uiSettingToggleQualityToMedium);

        Settings.isQualityMedium = false;
    }
}
