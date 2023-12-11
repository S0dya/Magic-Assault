using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuCharacterDescription : UIPanelMenu
{
    [Header("Characters")]
    [SerializeField] SO_Character[] characters;

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
    [SerializeField] Image characterInGameImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;

    //local
    [HideInInspector] public int curCharacterI;

    void Awake()
    {
        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { -Settings.height, 0 };
    }

    protected override void Start()
    {
        base.Start();

        OnCharacterClicked(0);
    }

    //buttons
    public void OnCharacterClicked(int i)
    {
        curCharacterI = i;

        SetInfo(characters[i]);
    }

    //main methods
    void SetInfo(SO_Character item)
    {
        SetString(item.hp, hpText);
        SetString(item.mana, manaText);
        SetString(item.shields, shieldsText);
        SetString(item.speed, speedText);

        for (int i = 0; i < 4; i++)
        {
            uiMainMenuStatsSpellItems[i].SetInfo(item.startingSpells[i]);
            SetString(item.multipliers[i], multipliersTexts[i]);

            SetString(item.damageMultipliers[i], damageMultipliersTexts[i]);
        }

        burningDealsDamageCheck.enabled = item.burningDealsDamage;
        waterDealsDamageCheck.enabled = item.waterDealsDamage;
        SetString(item.luck, luckText);

        characterInGameImage.sprite = item.ItemImage;
        nameText.text = item.Name;
        descriptionText.text = item.Description;
    }
}
