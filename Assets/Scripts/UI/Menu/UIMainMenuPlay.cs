using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIMainMenuPlay : UIPanel
{
    [Header("Other scripts")]
    [SerializeField] UIMainMenuCharacterDescription CharacterDescription;

    void Awake()
    {
        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { -Settings.height, 0 };
    }

    //buttons 
    public void OnClosePlay()
    {
        CloseTabInMenu();
    }

    //description part

}
