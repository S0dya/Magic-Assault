using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonobehaviour<LevelManager>
{
    [Header("UI")]
    [SerializeField] FloatingJoystick flJoystick;
    [SerializeField] FixedJoystick fxJoystick;

    protected override void Awake()
    {
        base.Awake();

        if (Settings.isFloatingJoystick) flJoystick.enabled = true;
        else fxJoystick.enabled = true;
    }
}
