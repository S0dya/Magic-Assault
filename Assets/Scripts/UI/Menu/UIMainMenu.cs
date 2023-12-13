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
    public void OnPressToStart()//switch to main menu panel
    {
        GameManager.I.Close(pressToStartCG, 0.25f);
        GameManager.I.Open(mainMenuCG, 0.75f);
    }

    public void OnPlayButton()//open chossing character and map
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
        int i = characterDescription.curCharacterI;
        GameManager.I.SetCharacterVars(characterDescription.unlockedCharacters[i], i);//set value that will be used in LevelManager script in another scene

        Settings.damageMultipliers = characterDescription.characters[i].floatDamageMultipliers;
        Settings.damageMultipliersMins = characterDescription.characters[i].minDamageMultipliers;
        Settings.startingSpells = characterDescription.characters[i].startingSpellsIndexes;

        int sceneId = mapDescription.curMapI + 2;

        LoadingScene.I.OpenScene(sceneId);//add 2, since first 2 scenes are persistant and menu
        GameManager.I.curSceneId = sceneId;
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
