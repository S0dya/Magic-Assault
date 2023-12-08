using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInGameStatsUpgrade : UIInGameStatsItem
{
    [Header("This upgrade")]
    [SerializeField] TextMeshProUGUI upgradeIndex;

    //local 
    int curUpgradeAmount = 1;

    public override void SetInfo(SO_Item upgradeItem)
    {
        base.SetInfo(upgradeItem);

        SetUpgradeIndex();
    }

    public void AddUpgradeAmount() //visualise upgrade progression
    {
        curUpgradeAmount++;

        SetUpgradeIndex();
    }

    //other methods
    void SetUpgradeIndex() => upgradeIndex.text = curUpgradeAmount.ToString();
}
