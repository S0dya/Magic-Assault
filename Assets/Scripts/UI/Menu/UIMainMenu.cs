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

    [SerializeField] UIMainMenuSettings uiMainMenuSettings;

    [Header("Canvas groups")]
    [SerializeField] CanvasGroup pressToStartCG;
    [SerializeField] CanvasGroup mainMenuCG;

    [Header("Buttons objects")]
    [SerializeField] GameObject mainButtonsObj;
    [SerializeField] GameObject backButtonObj;

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
        characterDescription.OpenTab();
        ToggleMainMenuHeaderButtons(false);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnOptionsButton()
    {
        uiMainMenuSettings.OpenTab();
    }

    public void OnStatisticsButton()
    {

    }

    //characters panel
    public void OnConfirmCharacter()
    {
        mapDescription.OpenTab();
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
            mapDescription.CloseTab();
            mapDescriptionOpen = false;
        }
        else
        {
            characterDescription.CloseTab();
            ToggleMainMenuHeaderButtons(true);
        }
    }

    //main methods
    
    public void SetMoneyText() => moneyAmountText.text = Settings.money.ToString();

    //other methods
    void ToggleMainMenuHeaderButtons(bool val)
    {
        mainButtonsObj.SetActive(val);
        backButtonObj.SetActive(!val);
    }
}
