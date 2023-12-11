using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : UIPanelMenu
{
    [Header("Music")]
    public Slider[] soundSliders;

    [Header("Toggles")]
    [SerializeField] UISettingToggle uiSettingToggleDamageNumbers;
    [SerializeField] UISettingToggle uiSettingToggleShowBlood;

    [Header("Two options")]
    [SerializeField] UISettingToggle uiSettingToggleFloatingJoystick;
    [SerializeField] UISettingToggle uiSettingToggleFixedJoystick;

    protected virtual void SetSettings()//set all current settings 
    {
        //sliders
        for (int i = 0; i < soundSliders.Length; i++) soundSliders[i].value = Settings.musicStats[i];

        //toggles
        ToggleSetting(uiSettingToggleDamageNumbers, Settings.showDamageNumbers);
        ToggleSetting(uiSettingToggleShowBlood, Settings.showBlood);

        //two options
        ToggleSettingTwoOptions(uiSettingToggleFloatingJoystick, uiSettingToggleFixedJoystick, Settings.isFloatingJoystick);
    }

    //options buttons
    //sliders
    public void OnSoundChange(int index)
    {
        AudioManager.I.SetVolume(index, soundSliders[index].value);
    }

    //toggles
    public virtual void OnToggleDamageNumbers()
    {
        bool toggle = Settings.showDamageNumbers = !Settings.showDamageNumbers;
        ToggleSetting(uiSettingToggleDamageNumbers, toggle);
    }

    public virtual void OnToggleShowBlood()
    {
        bool toggle = Settings.showBlood = !Settings.showBlood;
        ToggleSetting(uiSettingToggleShowBlood, toggle);
    }

    //other methods
    public void ToggleSetting(UISettingToggle uiSettingToggle, bool toggle) => uiSettingToggle.Toggle(toggle);

    //2 buttons option
    public virtual void OnChangeJoystickToFloating() 
    {
        if (Settings.isFloatingJoystick) return;

        ToggleSettingTwoOptions(uiSettingToggleFloatingJoystick, uiSettingToggleFixedJoystick);

        Settings.isFloatingJoystick = true;
    }
    public virtual void OnChangeJoystickToFixed()
    {
        if (!Settings.isFloatingJoystick) return;

        ToggleSettingTwoOptions(uiSettingToggleFixedJoystick, uiSettingToggleFloatingJoystick);

        Settings.isFloatingJoystick = false;
    }

    //other methods
    public void ToggleSettingTwoOptions(UISettingToggle uiSettingToggleFirst, UISettingToggle uiSettingToggleSecond)
    {
        uiSettingToggleFirst.Toggle(true);
        uiSettingToggleSecond.Toggle(false);
    }
    public void ToggleSettingTwoOptions(UISettingToggle uiSettingToggleFirst, UISettingToggle uiSettingToggleSecond, bool toggle)
    {
        uiSettingToggleFirst.Toggle(toggle);
        uiSettingToggleSecond.Toggle(!toggle);
    }
}
