using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGameStatsItem : MonoBehaviour
{
    [Header("UI settings")]
    [SerializeField] Image outsideImage;
    [SerializeField] Image insideImage;

    //local
    [HideInInspector] public UIInGameStats uiInGameStats;
    [HideInInspector] public SO_Item thisItem;

    //main methods
    public virtual void SetInfo(SO_Item item)
    {
        thisItem = item;
        insideImage.sprite = item.ItemImage;
    }

    //buttons
    public void OnImageClicked()
    {
        uiInGameStats.SetDescriptionInfo(this, thisItem);
    }

    //outside methods
    public void ToggleHighlight(bool toggle) => outsideImage.enabled = toggle;
}
