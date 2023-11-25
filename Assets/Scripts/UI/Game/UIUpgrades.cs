using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgrades : UIPanel
{
    [Header("Other scripts")]
    [SerializeField] ActiveUpgrades activeUpgrades;
    [SerializeField] PassiveUpgrades passiveUpgrades;

    [Header("Upgrades")]
    [SerializeField] UIUpgrade[] uiUpgrades;

    [SerializeField] List<SO_Item> allItems = new List<SO_Item>();

    //local

    //upgrades
    SO_Item[] curItems = new SO_Item[3];

    void Awake()
    {
        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { -Settings.height, 0 };
    }

    //panel
    public override void OpenTab()
    {
        SetUpgrades();

        base.OpenTab();
    }

    //other methods
    void SetUpgrades()
    {
        //get random upgrade, then remove this upgrade to get 2 more random upgrades
        for (int i = 0; i < 3; i++)
        {
            int randomI = Random.Range(0, allItems.Count);
            curItems[i] = allItems[randomI];
            allItems.RemoveAt(randomI);

            uiUpgrades[i].SetInfo(curItems[i]);
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

        for (int i = 0; i < 3; i++) if (i != index) allItems.Add(curItems[i]);

        CloseTab();
    }

    void SetUpgrade(int index)
    {
        SO_Item item = curItems[index];
        switch (item.type)
        {
            //active upgrades
            case UpgradeType.ActiveShootingNearestEnemy:
                activeUpgrades.EnableShootingNearestEnemy(item.typeOfDamage);
                AddNewUpgrades(item);
                break;
            case UpgradeType.ActiveShootingRandomEnemy:
                activeUpgrades.EnableShootingNearestEnemy(item.typeOfDamage);
                AddNewUpgrades(item);
                break;
            case UpgradeType.ActiveShootingRandomPosition:
                activeUpgrades.EnableShootingNearestEnemy(item.typeOfDamage);
                AddNewUpgrades(item);
                break;

            //passive upgrades
            case UpgradeType.PassiveIncreasePlayerSpeed:
                if (passiveUpgrades.CanIncreasePlayerSpeed()) allItems.Add(item);//add this passive upgrade back only if can use it more times
                break;
            default: break;
        }
    }

    void AddNewUpgrades(SO_Item item) //since some active upgrades can additional passive or active upgrades - add them to all upgrades
    {
        foreach (SO_Item newItem in item.itemsOnUpgrade) allItems.Add(newItem);
    }

}
