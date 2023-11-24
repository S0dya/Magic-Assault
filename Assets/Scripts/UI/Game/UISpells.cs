using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpells : UIPanel
{
    [Header("Other scripts")]
    [SerializeField] SpellsManager spellsManger;

    [Header("Upgrades")]
    [SerializeField] UISpell[] uiSpells;

    //local

    void Awake()
    {
        StartEndX = new float[2] { -Settings.width, 0 };
        StartEndY = new float[2] { 0, 0 };
    }

    //panel
    public override void OpenTab()
    {
        SetSpells();

        base.OpenTab();
    }

    //public virtual void CloseTab()

    //main methods
    void SetSpells()
    {
    }

    public void VisualiseChooseOfAnItem(int index)
    {
        //open currently interacted item and hide last interacted item if there is one
        for (int i = 0; i < 3; i++)
        {
            if (index == i) uiSpells[i].Open();
            else if (uiSpells[i].amountOfClicks == 1) uiSpells[i].Hide();
        }
    }

    public void ChooseUpgrade(int index)
    {
        SetUpgrade(index);
        //for (int i = 0; i < 3; i++) if (i != index) allItems.Add(curItems[i]);

        CloseTab();
    }

    void SetUpgrade(int index)
    {
        //SO_Item item = curItems[index];
    }
}
