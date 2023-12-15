using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStatsItem : MonoBehaviour
{
    [Header("UI settings")]
    [SerializeField] Image insideImage;

    //main methods
    public virtual void SetInfo(SO_Item item)
    {
        insideImage.sprite = item.ItemImage;
    }
}
