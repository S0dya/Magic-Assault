using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuStatsSpellItem : MonoBehaviour
{
    [SerializeField] Image spellImage;
    [SerializeField] TextMeshProUGUI spellName;

    public void SetInfo(SO_GameItem item)
    {
        spellImage.sprite = item.ItemImage;
        spellName.text = item.Name;
    }
}
