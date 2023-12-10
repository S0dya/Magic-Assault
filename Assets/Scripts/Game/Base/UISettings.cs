using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : UIPanel
{
    [Header("Music")]
    [SerializeField] Slider[] soundSliders;

    [Header("Toggles")]
    [SerializeField] UISettingToggle uiSettingToggleDamageNumbers;
    [SerializeField] UISettingToggle uiSettingToggleShowBlood;

    [Header("Two options")]
    [SerializeField] UISettingToggle uiSettingToggleFloatingJoystick;
    [SerializeField] UISettingToggle uiSettingToggleFixedJoystick;

    //initializing
    protected override void Start()
    {
        base.Start();

        SetSettings();
    }

    protected virtual void SetSettings()//set all current settings 
    {
        //for (int i = 0; i < soundSliders.Length; i++) soundSliders[i].value = Settings.musicStats[i];

        ToggleSetting(uiSettingToggleDamageNumbers, Settings.showDamageNumbers);
        ToggleSetting(uiSettingToggleShowBlood, Settings.showBlood);

        ToggleSettingTwoOptions(uiSettingToggleFloatingJoystick, uiSettingToggleFixedJoystick, Settings.isFloatingJoystick);
    }

    //options buttons
    //sliders
    public void OnSoundChange(int index)
    {
        AudioManager.I.SetVolume(index, soundSliders[index].value);
    }

    //toggles
    public void OnToggleDamageNumbers()
    {
        bool toggle = Settings.showDamageNumbers = !Settings.showDamageNumbers;
        ToggleSetting(uiSettingToggleDamageNumbers, toggle);
    }

    public void OnToggleShowBlood()
    {
        bool toggle = Settings.showBlood = !Settings.showBlood;
        ToggleSetting(uiSettingToggleShowBlood, toggle);
    }

    //other methods
    void ToggleSetting(UISettingToggle uiSettingToggle, bool toggle) => uiSettingToggle.Toggle(toggle);

    //2 buttons option
    public virtual void OnChangeJoystickToFloating() 
    {
        if (Settings.isFloatingJoystick) return;

        ToggleSettingTwoOptions(uiSettingToggleFloatingJoystick, uiSettingToggleFixedJoystick, true);

        Settings.isFloatingJoystick = true;
    }
    public virtual void OnChangeJoystickToFixed()
    {
        if (!Settings.isFloatingJoystick) return;

        ToggleSettingTwoOptions(uiSettingToggleFixedJoystick, uiSettingToggleFloatingJoystick, true);

        Settings.isFloatingJoystick = false;
    }

    public void ToggleSettingTwoOptions(UISettingToggle uiSettingToggleFirst, UISettingToggle uiSettingToggleSecond, bool toggle)
    {
        uiSettingToggleFirst.Toggle(toggle);
        uiSettingToggleSecond.Toggle(!toggle);
    }
}
