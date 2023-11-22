using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgradePanel : SingletonMonobehaviour<UIUpgradePanel>
{
    [Header("Settings")]
    [SerializeField] UIGameMenu uiGameMenu;
    [SerializeField] UIInGame uiInGame;
    [SerializeField] ActiveUpgrades activeUpgrades;
    [SerializeField] PassiveUpgrades passiveUpgrades;

    [Header("Upgrade tab")]
    [SerializeField] GameObject upgradeTab;
    [SerializeField] RectTransform upgradeTabTransform;
    [SerializeField] CanvasGroup upgradeTabCG;

    [Header("Upgrades")]
    [SerializeField] UIUpgrade[] uiUpgrades;

    [SerializeField] List<SO_Item> allItems = new List<SO_Item>();

    //local

    //upgrades
    SO_Item[] curItems = new SO_Item[3];

    protected override void Awake()
    {
        base.Awake();

    }

    public void Start()
    {
        upgradeTabTransform.anchoredPosition = new Vector2(0, -Screen.height);
    }

    //panel
    public void OpenUpgradeTab()
    {
        GameManager.I.MoveTransform(upgradeTabTransform, 0, 0, 0.75f);
        GameManager.I.Open(upgradeTabCG, 0.75f);

        SetUpgrades();
        uiGameMenu.ToggleTimeScale(false);
        DrawManager.I.StopCreatingSpell();
    }

    public void CloseUpgradeTab()
    {
        GameManager.I.MoveTransform(upgradeTabTransform, 0, -Settings.height, 0.25f);
        GameManager.I.Close(upgradeTabCG, 0.25f);

        uiGameMenu.ToggleTimeScale(true);
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

        CloseUpgradeTab();
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
