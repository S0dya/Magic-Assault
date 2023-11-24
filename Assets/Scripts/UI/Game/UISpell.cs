using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpell : UIItem
{
    //local
    UISpells uiSpells;

    void Start() => uiSpells = GetComponentInParent<UISpells>();

    //buttons
    public void OnImageButton(int index)
    {
        if (amountOfClicks == 1)
        {
            uiSpells.ChooseUpgrade(index);
            Hide();
            return;
        }

        AmountOfClicks++;
        uiSpells.VisualiseChooseOfAnItem(index);
    }
}
