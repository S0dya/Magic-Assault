using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : UIPanel
{
    void Awake()
    {

        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { Settings.height, 0 };
    }

    //buttons 
    public void PauseButton()
    {
        OpenTab();
    }

    public void ResumeButton()
    {
        CloseTab();
    }

    
}
