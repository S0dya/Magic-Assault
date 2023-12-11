using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInGameSettings : UISettings
{
    [Header("OtherScripts")]
    [SerializeField] UIInGame uiInGame;


    void Awake()
    {
        StartEndX = new float[2] { Settings.width, 0 };
        StartEndY = new float[2] { 0, 0 };
    }

    //initializing
    protected override void Start()
    {
        base.Start();

        SetSettings();
    }

    //buttons
    //toggle with two options
    public override void OnChangeJoystickToFloating()
    {
        base.OnChangeJoystickToFloating();

        uiInGame.SetJoystick();
    }
    public override void OnChangeJoystickToFixed()
    {
        base.OnChangeJoystickToFixed();

        uiInGame.SetJoystick();
    }
}
