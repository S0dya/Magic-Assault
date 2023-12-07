using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGameStats : UIPanel
{
    [Header("Prefabs")]
    [SerializeField] GameObject lineObject;

    [SerializeField] GameObject itemPrefab;

    [Header("Spells")]
    [SerializeField]

    [Header("Parents")]
    [SerializeField] Transform[] upgradesParents;

    [Header("Item description")]
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDescription;

    //local
    Dictionary<SO_Item, UIInGameStatsItem>[] upgradeItemsDics = new Dictionary<SO_Item, UIInGameStatsItem>[2];

    UIInGameStatsItem curUiInGameStatsItem;

    void Awake()
    {
        StartEndX = new float[2] { Settings.width, 0 };
        StartEndY = new float[2] { 0, 0 };
    }

    protected override void Start()
    {
        base.Start();

        SetSpellsStats();
    }


    //main methods
    public void AddItem(SO_Item item)
    {
        int type = (item.parentType == UpgradeTypeParent.ActiveUpgrade ? 0 : 1);

        if (upgradeItemsDics[type].ContainsKey(item)) 
        {
            upgradeItemsDics[type][item].AddUpgradeAmount();

            return;
        }

        Transform lineTransform = Instantiate(lineObject, upgradesParents[type]).GetComponent<Transform>();

        GameObject itemObj = Instantiate(itemPrefab, lineTransform);
        UIInGameStatsItem uiItem = itemObj.GetComponent<UIInGameStatsItem>();
        uiItem.SetInfo(item);
        uiItem.uiInGameStats = this;

        upgradeItemsDics[type].Add(item, uiItem);
    }

    //spells (header)
    public void SetNewSpellItem(SO_Item spellItem)
    {

    }

    public void SetSpellItem(SO_Item spellItem)
    {

    }

    //description (bottom)
    public void SetNewDescriptionInfo(UIInGameStatsItem uiInGameStatsItem, SO_Item item)
    {
        curUiInGameStatsItem.ToggleHighlight(false);

        SetDescriptionInfo(uiInGameStatsItem, item);  
    }

    public void SetDescriptionInfo(UIInGameStatsItem uiInGameStatsItem, SO_Item item) //outside method for showcasing upgrade
    {
        curUiInGameStatsItem = uiInGameStatsItem;
        curUiInGameStatsItem.ToggleHighlight(true);

        itemImage.sprite = item.ItemImage;
        itemName.text = item.Name;
        itemDescription.text = item.Description;
    }


}
