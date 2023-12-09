using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuCharacterDescription : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] SO_Character[] characters;

    [Header("Description panel")]
    //main stats
    [SerializeField] Image characterInGameImage;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] TextMeshProUGUI speedText;

    [SerializeField] UIMainMenuStatsSpellItem[] uiMainMenuStatsSpellItems;

    //additional 



    //buttons
    public void OnCharacterClicked(int i)
    {
        SetInfo(characters[i]);
    }

    //main methods
    void SetInfo(SO_Character item)
    {
        characterInGameImage.sprite = item.ItemImage;

        SetString(item.hp, hpText);
        SetString(item.mana, hpText);
        SetString(item.speed, hpText);

        for (int i = 0; i < 4; i++)
        {
            uiMainMenuStatsSpellItems[i].SetInfo(item.startingSpells[i]);

        }


    }

    //other methods
    void SetString(int val, TextMeshProUGUI text) => text.text = val.ToString();
    void SetString(float val, TextMeshProUGUI text) => text.text = val.ToString();
}
