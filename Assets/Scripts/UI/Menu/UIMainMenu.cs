using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenu : MonoBehaviour
{
    [Header("Other scripts")]
    [SerializeField] UIMainMenuPlay uiMainMenuPlay;

    [Header("Canvas groups")]
    [SerializeField] CanvasGroup pressToStartCG;
    [SerializeField] CanvasGroup mainMenuCG;

    [Header("Money")]
    [SerializeField] TextMeshProUGUI moneyAmountText;

    void Start()
    {
        SetMoneyText();
    }

    //buttons
    public void OnPressToStart()
    {
        GameManager.I.Close(pressToStartCG, 0.25f);
        GameManager.I.Open(mainMenuCG, 0.75f);
    }

    public void OnPlayButton()
    {
        uiMainMenuPlay.OpenTabInMenu();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnOptionsButton()
    {

    }

    public void OnStatisticsButton()
    {

    }

    //main methods


    public void SetMoneyText() => moneyAmountText.text = Settings.money.ToString();
}
