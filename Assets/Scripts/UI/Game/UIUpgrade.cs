using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : UIItem
{
    [SerializeField] CanvasGroup typeOfDamageOfSpell;
    [SerializeField] Image[] typesHighlightImages;

    //local
    UIUpgrades uiUpgrades;

    //active upgrade check
    SO_GameItem curItem;
    bool isActiveSpell;

    //highlight 
    int curHighlightedType;


    void Start() => uiUpgrades = GetComponentInParent<UIUpgrades>();

    //main methods overrided for active upgrade check
    public override void SetInfo(SO_GameItem item)
    {
        base.SetInfo(item);

        curItem = item;
        isActiveSpell = item.parentType == UpgradeTypeParent.ActiveUpgrade;
        if (isActiveSpell) SetHighlight(curItem.typeOfDamage);
    }

    public override void Open()
    {
        base.Open();

        if (isActiveSpell) GameManager.I.Open(typeOfDamageOfSpell, 0.1f);
    }

    public override void Hide()
    {
        base.Hide();

        if (isActiveSpell) GameManager.I.Close(typeOfDamageOfSpell, 0.1f);
    }



    //buttons
    public void OnImageButton(int index)
    {
        if (amountOfClicks == 1)
        {
            uiUpgrades.ChooseUpgrade(index);
            Invoke("Hide", 0.1f);//invoke for proper closing 
            return;
        }

        AmountOfClicks++;
        uiUpgrades.VisualiseChooseOfAnItem(index);
    }

    public void OnChooseTypeOfDamageButton(int index)
    {
        curItem.typeOfDamage = index;

        SetHighlight(index);
    }

    void SetHighlight(int newIndex)
    {
        ToggleHighlight(curHighlightedType, false);
        curHighlightedType = newIndex;
        ToggleHighlight(curHighlightedType, true);
    }
    void ToggleHighlight(int index, bool val) => typesHighlightImages[index].enabled = val;
}
