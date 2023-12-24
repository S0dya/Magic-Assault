using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgrades : UIPanelGame
{
    [Header("Other scripts")]
    [SerializeField] PassiveUpgrades passiveUpgrades;
    [SerializeField] UIInGameStats uiInGameStats;

    [Header("Upgrades")]
    [SerializeField] UIUpgrade[] uiUpgrades;

    [SerializeField] List<SO_GameItem> allItems = new List<SO_GameItem>();

    [Header("Other")]
    [SerializeField] ParticleSystem upgradeEffect;

    //local
    ActiveUpgrades activeUpgrades;

    //upgrades
    SO_GameItem[] curUpgrades = new SO_GameItem[3];

    void Awake()
    {
        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { -Settings.height, 0 };
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
        //get random upgrade, then remove this upgrade to get 2 more random upgrades
        for (int i = 0; i < 3; i++)
        {
            int randomI = Random.Range(0, allItems.Count);
            curUpgrades[i] = allItems[randomI];
            allItems.RemoveAt(randomI);

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

        for (int i = 0; i < 3; i++) if (i != index) allItems.Add(curUpgrades[i]);

        CloseTab();
    }

    void SetUpgrade(int index)
    {
        SO_GameItem item = curUpgrades[index];
        uiInGameStats.AddItem(item);

        switch (item.parentType)
        {
            case UpgradeTypeParent.PassiveUpgrade:
                if (!passiveUpgrades.CanPerformPassiveUpgrade(item.type)) AddNewUpgrades(item);
                break;
            default: break;
        }

        switch (item.type)
        {
            //active upgrades
            case UpgradeType.ActiveDotInNearestEnemy:
                activeUpgrades.EnableDotInNearestEnemy(item.typeOfDamage);
                AddNewUpgrades(item);
                break;
            case UpgradeType.ActiveDotInRandomEnemy:
                activeUpgrades.EnableDotInNearestEnemy(item.typeOfDamage);
                AddNewUpgrades(item);
                break;
            case UpgradeType.ActiveDotInRandomPosition:
                activeUpgrades.EnableDotInNearestEnemy(item.typeOfDamage);
                AddNewUpgrades(item);
                break;
            case UpgradeType.ActiveDotInMovementDirection:
                activeUpgrades.EnableDotInMovementDirection(item.typeOfDamage);
                AddNewUpgrades(item);
                break;

            //passive upgrades
            case UpgradeType.PassiveIncreasePlayerSpeed:
                passiveUpgrades.IncreasePlayerSpeed();//add this passive upgrade back only if can use it more times
                break;
            default: break;
        }
    }

    void AddNewUpgrades(SO_GameItem item) //since some active upgrades can additional passive or active upgrades - add them to all upgrades
    {
        foreach (SO_GameItem newItem in item.itemsOnUpgrade) allItems.Add(newItem);
    }

}
