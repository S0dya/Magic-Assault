using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_GameItem : SO_Item
{
    public UpgradeTypeParent parentType;
    public UpgradeType type;

    [Header("Settings")]
    //spells and active upgrades
    public int typeOfDamage;
    public SO_GameItem[] itemsOnUpgrade;

    //spells
    public int spellI;
}
