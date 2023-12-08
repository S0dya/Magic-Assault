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
    [SerializeField] GameObject itemUpgradePrefab;

    [Header("Spells")]
    public UIInGameStatsItem[] uiSpellsitems;

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
    Transform[] curLinesTransforms = new Transform[2];

    UIInGameStatsItem curUiInGameStatsItem;
    bool curHighlighted;

    void Awake()
    {
        StartEndX = new float[2] { Settings.width, 0 };
        StartEndY = new float[2] { 0, 0 };

        for (int i = 0; i < upgradeItemsDics.Length; i++) upgradeItemsDics[i] = new Dictionary<SO_Item, UIInGameStatsUpgrade>();
    }

    //main methods
    public void AddItem(SO_Item item)
    {
        //get type index
        int type = (item.parentType == UpgradeTypeParent.ActiveUpgrade ? 0 : 1);

        if (upgradeItemsDics[type].ContainsKey(item)) //if there is alr an upgrade - add upgrade amount 
        {
            upgradeItemsDics[type][item].AddUpgradeAmount();

            return;
        }

        //check if new line is needed and change amount of created items in cur line
        bool createNewLineIsNeeded = curLinesN[type] < 10;
        if (createNewLineIsNeeded) curLinesN[type] = 0;
        else curLinesN[type]++;
        Debug.Log("I " + curLinesN[type]);

        //set line based on need of a new line
        Transform lineTransform = (createNewLineIsNeeded ? CreateNewLine(type) : curLinesTransforms[type]);

        //instantiate item object and set its info 
        GameObject itemObj = Instantiate(itemUpgradePrefab, lineTransform);
        UIInGameStatsUpgrade uiItemUpgrade = itemObj.GetComponent<UIInGameStatsUpgrade>();
        SetItem(uiItemUpgrade, item);//set ui item

        //add new item to dict for future
        upgradeItemsDics[type].Add(item, uiItemUpgrade);
    }
    Transform CreateNewLine(int type)//instantiate new line and set it as current
    {
        Debug.Log("asd");
        Transform lineTransform = Instantiate(lineObject, upgradesParents[type]).GetComponent<Transform>();
        curLinesTransforms[type] = lineTransform;
        return lineTransform;
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
}
