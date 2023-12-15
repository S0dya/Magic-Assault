using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGameStatsItem : UIStatsItem
{
    [SerializeField] Image outsideImage;
    
    //local
    [HideInInspector] public UIInGameStats uiInGameStats;
    [HideInInspector] public SO_Item thisItem;


    public override void SetInfo(SO_Item item)
    {
        base.SetInfo(item);

        thisItem = item;
    }

    //buttons
    public void OnImageClicked()
    {
        uiInGameStats.SetDescriptionInfo(this, thisItem);
    }

    //outside methods
    public void ToggleHighlight(bool toggle) => outsideImage.enabled = toggle;
}
