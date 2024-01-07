using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuCharacterDescription : UIPanelMenu
{
    [Header("Other scripts")]
    [SerializeField] UIMainMenu uiMainMenu;

    [Header("Characters")]
    public SO_Character[] characters;

    [SerializeField] Image[] charactersImages;

    public Sprite[] unlockedCharacters;
    [SerializeField] Sprite[] lockedCharacters;

    [Header("Stats panel")]
    //first line
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] TextMeshProUGUI shieldsText;

    [SerializeField] TextMeshProUGUI speedText;

    //second line
    [SerializeField] UIMainMenuStatsSpellItem[] uiMainMenuStatsSpellItems;
    [SerializeField] TextMeshProUGUI[] multipliersTexts;

    //third line 
    [SerializeField] TextMeshProUGUI[] damageMultipliersTexts;
    [SerializeField] Image burningDealsDamageCheck;
    [SerializeField] Image waterDealsDamageCheck;
    [SerializeField] TextMeshProUGUI luckText;

    [Header("character description")]
    [SerializeField] Image[] characterInGameImages;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;

    [Header("Additional part")]
    [SerializeField] GameObject buyCharacterPanel;
    [SerializeField] CanvasGroup confirmButtonCG;
    [SerializeField] TextMeshProUGUI priceText;

    //local
    [HideInInspector] public int curCharacterI;

    void Awake()
    {
        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { -Settings.height, -20 };
    }

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < 5; i++)
        {
            charactersImages[i].sprite = (Settings.charactersPrices[i] == 0 ? unlockedCharacters[i] : lockedCharacters[i]);
        }

        OnCharacterClicked(0);
    }

    //buttons
    public void OnCharacterClicked(int i)
    {
        //switch images of characters since images of different size are used
        characterInGameImages[curCharacterI].enabled = false;

        curCharacterI = i;
        characterInGameImages[curCharacterI].enabled = true;

        SetInfo(characters[curCharacterI]);

        ToggleBuyCharacterPanel(Settings.charactersPrices[curCharacterI] != 0);
        SetString(Settings.charactersPrices[curCharacterI], priceText);
    }

    public void OnUnlock()//buy character
    {
        int curPrice = Settings.charactersPrices[curCharacterI];
        if (Settings.money >= curPrice)// turn off buying tab, decrease money amount, set character as unlocked in settings and switch their image
        {
            Settings.money -= curPrice;
            uiMainMenu.SetMoneyText();

            Settings.charactersPrices[curCharacterI] = 0;
            charactersImages[curCharacterI].sprite = unlockedCharacters[curCharacterI];

            ToggleBuyCharacterPanel(false);
        }
    }

    //main methods
    void SetInfo(SO_Character item)//set all characters statistics 
    {
        SetString(item.hp, hpText);
        SetString(item.mana, manaText);
        SetString(item.shields, shieldsText);
        SetString(item.speed, speedText);

        for (int i = 0; i < 4; i++)
        {
            uiMainMenuStatsSpellItems[i].SetInfo(item.startingSpells[i]);
            SetString(item.damageMultipliers[i], multipliersTexts[i]);

            SetString(item.weaknessMultipliers[i], damageMultipliersTexts[i]);
        }

        burningDealsDamageCheck.enabled = item.burningDealsDamage;
        waterDealsDamageCheck.enabled = item.waterDealsDamage;
        SetString(item.luck, luckText);

        nameText.text = item.Name;
        descriptionText.text = item.Description;
    }

    //other methods
    void ToggleBuyCharacterPanel(bool toggle)
    {
        buyCharacterPanel.SetActive(toggle);

        confirmButtonCG.interactable = !toggle;
    }
}
