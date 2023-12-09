using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpells : UIPanel
{
    [Header("Other scripts")]
    [SerializeField] SpellsManager spellsManager;
    [SerializeField] UIInGameStats uiInGameStats;

    [Header("Upgrades")]
    [SerializeField] UISpell[] uiSpells;

    public List<SO_GameItem> allSpells;

    //local
    SO_GameItem[] curUsedSpells = new SO_GameItem[4];
    SO_GameItem[] curCheckedSpells = new SO_GameItem[3];

    void Awake()
    {
        StartEndX = new float[2] { -Settings.width, 0 };
        StartEndY = new float[2] { 0, 0 };
    }

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < 4; i++) //set cur spells player uses
        {
            int curIndex = spellsManager.curTypeOfSpell[i] + (i * 4 - i);
            curUsedSpells[i] = allSpells[curIndex];

            uiInGameStats.SetSpellItem(i, allSpells[curIndex]);//set spell in statistics

            allSpells.RemoveAt(curIndex);
        }
    }

    //panel
    public override void OpenTabInGame()
    {
        SetSpells();

        base.OpenTabInGame();
    }

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

        for (int i = 0; i < 3; i++) if (i != index) allSpells.Add(curCheckedSpells[i]);

        CloseTabInGame();
    }
    
    void SetSpell(int index)
    {
        SO_GameItem newSpell = curCheckedSpells[index];
        int spellI = newSpell.spellI;

        allSpells.Add(curUsedSpells[spellI]);
        curUsedSpells[spellI] = newSpell;

        spellsManager.curTypeOfSpell[spellI] = newSpell.typeOfDamage;
    }

    //Button
    public void SkipButton()
    {
        foreach (SO_GameItem spell in curCheckedSpells) allSpells.Add(spell);

        CloseTabInGame();
    }
}
