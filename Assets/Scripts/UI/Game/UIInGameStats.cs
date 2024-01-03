using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGameStats : UIPanelMenu
{
    [Header("Prefabs")]
    [SerializeField] GameObject lineObject;

    [SerializeField] GameObject itemPrefab;
    [SerializeField] GameObject itemUpgradePrefab;

    [Header("Spells")]
    [SerializeField] UIInGameStatsItem[] uiSpellsitems;
    [SerializeField] SO_Item NoItemItem;

    [Header("Parents")]
    [SerializeField] Transform[] upgradesParents;

    [Header("Item description")]
    [SerializeField] CanvasGroup DescriptionCG;

    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDescription;

    //local
    Dictionary<SO_Item, UIInGameStatsUpgrade>[] upgradeItemsDics = new Dictionary<SO_Item, UIInGameStatsUpgrade>[2];

    int[] curLinesN = new int[2];
    [SerializeField] Transform[] curLinesTransforms = new Transform[2];

    UIInGameStatsItem curUiInGameStatsItem;
    bool curHighlighted;

    void Awake()
    {
        StartEndX = new float[2] { Settings.width, 20 };
        StartEndY = new float[2] { 0, 0 };

        for (int i = 0; i < upgradeItemsDics.Length; i++) upgradeItemsDics[i] = new Dictionary<SO_Item, UIInGameStatsUpgrade>();
    }

    //main methods
    public void AddItem(SO_GameItem item)
    {
        //get type index
        int type = (item.parentType == UpgradeTypeParent.ActiveUpgrade || item.parentType == UpgradeTypeParent.PassiveActiveUpgrade ? 0 : 1);

        if (upgradeItemsDics[type].ContainsKey(item)) //if there is alr an upgrade - add upgrade amount 
        {
            upgradeItemsDics[type][item].AddUpgradeAmount();

            return;
        }

        //check if new line is needed and change amount of created items in cur line
        if (curLinesN[type] == 10)
        {
            curLinesN[type] = 0;
            //set new line 
            curLinesTransforms[type] = CreateNewLine(lineObject, upgradesParents[type]);
        }
        else curLinesN[type]++;

        GameObject itemObj = Instantiate(itemUpgradePrefab, curLinesTransforms[type]);
        UIInGameStatsUpgrade uiItemUpgrade = itemObj.GetComponent<UIInGameStatsUpgrade>();
        SetItem(uiItemUpgrade, item);//set ui item

        //add new item to dict for future
        upgradeItemsDics[type].Add(item, uiItemUpgrade);
    }
    Transform CreateNewLine(GameObject linePrefab, Transform parent)//instantiate new line and set it as current
    {
        return Instantiate(linePrefab, parent).GetComponent<Transform>();
    }
    
    void SetItem(UIInGameStatsUpgrade uiItemUpgrade, SO_Item item)//set info 
    {
        uiItemUpgrade.SetInfo(item);
        uiItemUpgrade.uiInGameStats = this;
    }

    //spells (header)
    public void SetSpellItem(int i, SO_Item item)
    {
        uiSpellsitems[i].SetInfo(item);
        uiSpellsitems[i].uiInGameStats = this;
    }
    public void SetNoSpellItem(int i) => SetSpellItem(i, NoItemItem);

    //description (bottom)
    public void SetDescriptionInfo(UIInGameStatsItem uiInGameStatsItem, SO_Item item) //outside method for showcasing upgrade
    {
        //set highlight
        if (curHighlighted) curUiInGameStatsItem.ToggleHighlight(false);
        else
        {
            curHighlighted = true;
            GameManager.I.Open(DescriptionCG, 0.2f);
        }

        //set cur upgrade and toggle higlhight visualisation of choose
        curUiInGameStatsItem = uiInGameStatsItem;
        curUiInGameStatsItem.ToggleHighlight(true);

        //set description
        itemImage.sprite = item.ItemImage;
        itemName.text = item.Name;
        itemDescription.text = item.Description;
    }

    //other methods
    public void StopHighlight()//stop highlighting item and showing its description
    {
        if (!curHighlighted) return;

        curHighlighted = false;
        curUiInGameStatsItem.ToggleHighlight(false);
        GameManager.I.Close(DescriptionCG, 0.1f);
    }

    //outside method for results
    public Dictionary<SO_Item, int> GetUpgradesDict(int i)
    {
        var dict = new Dictionary<SO_Item, int>();

        foreach (var kvp in upgradeItemsDics[i])
        {
            dict.Add(kvp.Key, kvp.Value.curUpgradeAmount);
        }
        
        return dict;
    }

    public SO_Item GetSpellsItem(int i)
    {
        return uiSpellsitems[i].thisItem;
    }
}
