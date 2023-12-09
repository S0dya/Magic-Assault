using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuStatsSpellItem : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI name;

    public void SetInfo(SO_GameItem item)
    {
        image.sprite = item.ItemImage;
        name.text = item.Name;
    }
}
