using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenu : SingletonMonobehaviour<UIMainMenu>
{
    [Header("Other scripts")]
    [SerializeField] UIMainMenuCharacterDescription characterDescription;
    [SerializeField] UIMainMenuMapDescription mapDescription;

    [SerializeField] UIMainMenuTraining uiMainMenuTraining;

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
    bool inputsInfoOpen;

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
        if (Settings.showTraining)
        {
            uiMainMenuTraining.OpenTab();
            inputsInfoOpen = true;
        }
        else StartGame();
    }

    //back button
    public void OnBack()
    {
        if (inputsInfoOpen)
        {
            uiMainMenuTraining.CloseTab();
            inputsInfoOpen = false;
        }
        else if (mapDescriptionOpen)
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

    public void StartGame()
    {
        int i = characterDescription.curCharacterI;
        int sceneId = mapDescription.curMapI + 2;

        LoadingScene.I.OpenScene(sceneId);//add 2, since first 2 scenes are persistant and menu

        SetCharacterSettings(characterDescription.characters[i], i, sceneId);
    }

    void SetCharacterSettings(SO_Character character, int i, int sceneId)//set all info for settings about character
    {
        Settings.characterSprite = characterDescription.unlockedCharacters[i];
        Settings.characterSpriteInGame = character.ItemImage;
        Settings.characterName = character.Name;

        Settings.characterI = i;
        Settings.curSceneId = sceneId;

        Settings.damageMultipliers = character.floatDamageMultipliers;
        Settings.damageMultipliersMins = character.minDamageMultipliers;
        Settings.startingSpells = character.startingSpellsIndexes;
    }
}
