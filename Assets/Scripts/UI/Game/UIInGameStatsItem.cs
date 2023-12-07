using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGameStatsItem : MonoBehaviour
{
    [Header("UI settings")]
    [SerializeField] Image outsideImage;
    [SerializeField] Image insideImage;
    [SerializeField] TextMeshProUGUI upgradeIndex;

    //local
    [HideInInspector] public UIInGameStats uiInGameStats;
    [HideInInspector] public SO_Item thisItem;

    int curUpgradeAmount;

    //main methods
    public void SetInfo(SO_Item item)
    {
        thisItem = item;
        insideImage.sprite = item.ItemImage;
        SetUpgradeIndex();
    }

    //buttons
    public void OnImageClicked()
    {
        uiInGameStats.SetDescriptionInfo(this, thisItem);
    }

    //outside methods
    public void ToggleHighlight(bool toggle) => outsideImage.enabled = toggle;
    public void AddUpgradeAmount() //visualise upgrade progression
    {
        curUpgradeAmount++;

        SetUpgradeIndex();
    }

    //other methods
    void SetUpgradeIndex() => upgradeIndex.text = curUpgradeAmount.ToString();
}
