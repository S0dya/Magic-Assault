using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgrade : UIItem
{
    //local
    UIUpgrades uiUpgrades;

    void Start() => uiUpgrades = GetComponentInParent<UIUpgrades>();

    //buttons
    public void OnImageButton(int index)
    {
        if (amountOfClicks == 1)
        {
            uiUpgrades.ChooseUpgrade(index);
            Invoke("Hide", 0.1f);
            return;
        }

        AmountOfClicks++;
        uiUpgrades.VisualiseChooseOfAnItem(index);
    }
}
