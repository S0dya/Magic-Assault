using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpells : UIPanel
{
    [Header("Other scripts")]
    [SerializeField] SpellsManager spellsManager;

    [Header("Upgrades")]
    [SerializeField] UISpell[] uiSpells;

    [SerializeField] List<SO_Item> allSpells;

    //local
    SO_Item[] curUsedSpells = new SO_Item[4];
    SO_Item[] curCheckedSpells = new SO_Item[3];

    void Awake()
    {
        StartEndX = new float[2] { -Settings.width, 0 };
        StartEndY = new float[2] { 0, 0 };
    }

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < 4; i++)
        {
            int curIndex = spellsManager.curTypeOfSpell[i] + (i * 4 - i);
            curUsedSpells[i] = allSpells[curIndex];
            allSpells.RemoveAt(curIndex);
        }
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
        for (int i = 0; i < 3; i++)
        {
            int randomI = Random.Range(0, allSpells.Count);
            curCheckedSpells[i] = allSpells[randomI];
            allSpells.RemoveAt(randomI);

            uiSpells[i].SetInfo(curCheckedSpells[i]);
        }
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
        SetSpell(index);

        for (int i = 0; i < 3; i++)
        {
            if (i != index)
            {
                allSpells.Add(curCheckedSpells[i]);
            }
        }

        CloseTab();
    }
    
    void SetSpell(int index)
    {
        SO_Item newSpell = curCheckedSpells[index];
        int spellI = newSpell.spellI;

        allSpells.Add(curUsedSpells[spellI]);
        curUsedSpells[spellI] = newSpell;

        spellsManager.curTypeOfSpell[spellI] = newSpell.typeOfDamage;
    }

    //Button
    public void SkipButton()
    {
        foreach (SO_Item spell in curCheckedSpells) allSpells.Add(spell);

        CloseTab();
    }
}
