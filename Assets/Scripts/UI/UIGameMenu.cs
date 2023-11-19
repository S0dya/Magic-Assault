using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : SingletonMonobehaviour<UIGameMenu>
{


    protected override void Awake()
    {
        base.Awake();

    }

    //other methods
    public void ToggleTimeScale(bool val)
    {
        DrawManager.I.isOnUI = !val;
        Time.timeScale = val ? 1 : 0;
    }
}
