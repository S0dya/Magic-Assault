using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgrades : UIPanelGame
{
    [Header("Other scripts")]
    [SerializeField] PassiveUpgrades passiveUpgrades;
    [SerializeField] UIInGameStats uiInGameStats;
    [SerializeField] UIInGame uiInGame;

    [Header("Upgrades")]
    [SerializeField] UIUpgrade[] uiUpgrades;

    [SerializeField] List<SO_GameItem> allItems = new List<SO_GameItem>();
    [SerializeField] SO_GameItem[] additionalItems;

    [Header("Other")]
    [SerializeField] ParticleSystem upgradeEffect;

    //local
    ActiveUpgrades activeUpgrades;

    //upgrades
    SO_GameItem[] curUpgrades = new SO_GameItem[3];
    bool[] upgradesAddBackCheck = new bool[3];

    //tracking active upgrades amount 
    int curAmountOfActiveUpgrades;

    void Awake()
    {
        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { -Settings.height, -20 };
    }

    protected override void Start()
    {
        base.Start();

        activeUpgrades = GameObject.FindGameObjectWithTag("Player").GetComponent<ActiveUpgrades>();
    }

    //panel
    public override void OpenTab()
    {
        SetUpgrades();
        upgradeEffect.Play();

        base.OpenTab();
    }

    public override void CloseTab()
    {
        base.CloseTab();

        upgradeEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    //other methods
    void SetUpgrades()
    {
        //get random upgrade, then remove this upgrade and get 2 more random upgrades
        for (int i = 0; i < 3; i++)
        {
            if (allItems.Count == 0)
            {
                curUpgrades[i] = additionalItems[i]; //if there are no upgrades - place items 

                AddUpgradeBack(i, false);
            }
            else
            {
                int randomI = Random.Range(0, allItems.Count);
                curUpgrades[i] = allItems[randomI];
                allItems.RemoveAt(randomI);
                
                AddUpgradeBack(i, true);
            }
            
            uiUpgrades[i].SetInfo(curUpgrades[i]);
        }
    }

    public void VisualiseChooseOfAnItem(int index)
    {
        //open currently interacted item and hide last interacted item if there is one
        for (int i = 0; i < 3; i++)
        {
            if (index == i) uiUpgrades[i].Open();
            else if (uiUpgrades[i].amountOfClicks == 1) uiUpgrades[i].Hide();
        }
    }

    public void ChooseUpgrade(int index)
    {
        SetUpgrade(index);

        for (int i = 0; i < 3; i++) if (i != index && upgradesAddBackCheck[i]) allItems.Add(curUpgrades[i]);

        CloseTab();
    }

    void SetUpgrade(int index)
    {
        SO_GameItem item = curUpgrades[index];
        uiInGameStats.AddItem(item);

        switch (item.parentType)
        {
            case UpgradeTypeParent.ActiveUpgrade:
                activeUpgrades.PerformActiveUpgrade(item.type, item.typeOfDamage);
                AddNewUpgrades(item);
                curAmountOfActiveUpgrades++;
                if (curAmountOfActiveUpgrades == 1) StartCoroutine(RemoveAllActiveUpgradesCor());
                break;
            case UpgradeTypeParent.PassiveUpgrade:
                if (!passiveUpgrades.CanPerformPassiveUpgrade(item.type)) AddNewUpgrades(item);//add this passive upgrade back only if can use it more times
                break;
            case UpgradeTypeParent.PassiveActiveUpgrade:
                if (!passiveUpgrades.CanPerformActivePassiveUpgrade(item.type)) AddNewUpgrades(item);//add this passive upgrade back only if can use it more times
                break;
            case UpgradeTypeParent.PickUps:
                uiInGame.UsePickUpUpgrade(item.type);
                break;
            default: break;
        }
    }



    void AddNewUpgrades(SO_GameItem item) //since some active upgrades can additional passive or active upgrades - add them to all upgrades
    {
        foreach (SO_GameItem newItem in item.itemsOnUpgrade) allItems.Add(newItem);
    }

    IEnumerator RemoveAllActiveUpgradesCor()//skip current frame and remove all active upgrades from all items
    {
        yield return null;

        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].parentType == UpgradeTypeParent.ActiveUpgrade)
            {
                allItems.RemoveAt(i);
                i--;
            }
        }
    }

    //other
    void AddUpgradeBack(int i, bool val) => upgradesAddBackCheck[i] = val;//since items on upgrades finished are not added back to all items for further upgrades
}
