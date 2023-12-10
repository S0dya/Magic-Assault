using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenu : MonoBehaviour
{
    [Header("Other scripts")]
    [SerializeField] UIMainMenuCharacterDescription characterDescription;
    [SerializeField] UIMainMenuMapDescription mapDescription;

    [Header("Canvas groups")]
    [SerializeField] CanvasGroup pressToStartCG;
    [SerializeField] CanvasGroup mainMenuCG;

    [Header("Money")]
    [SerializeField] TextMeshProUGUI moneyAmountText;

    //local
    bool mapDescriptionOpen;

    void Start()
    {
        SetMoneyText();
    }

    //buttons
    //main menu
    public void OnPressToStart()
    {
        GameManager.I.Close(pressToStartCG, 0.25f);
        GameManager.I.Open(mainMenuCG, 0.75f);
    }

    public void OnPlayButton()
    {
        characterDescription.OpenTabInMenu();
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

    //characters panel
    public void OnConfirmCharacter()
    {
        mapDescription.OpenTabInMenu();
        mapDescriptionOpen = true;
    }

    //maps panel
    public void OnConfirmMap()
    {
        
    }

    public void OnBack()
    {
        if (mapDescriptionOpen)
        {
            mapDescription.CloseTabInMenu();
            mapDescriptionOpen = false;
        }
        else
        {
            characterDescription.CloseTabInMenu();
        }
    }

    //main methods
    
    public void SetMoneyText() => moneyAmountText.text = Settings.money.ToString();
}
