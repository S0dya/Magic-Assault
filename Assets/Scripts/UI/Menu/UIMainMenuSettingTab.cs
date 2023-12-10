using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuSettingTab : UIPanel
{
    void Awake()
    {
        StartEndX = new float[2] { Settings.width, 0 };
        StartEndY = new float[2] { 0, 0 };
    }
}
