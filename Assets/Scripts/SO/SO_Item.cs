using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class SO_Item : ScriptableObject
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField][field: TextArea] public string Description { get; set; }
    [field: SerializeField] public Sprite ItemImage { get; set; }

    public UpgradeType type;

    [Header("Settings")]
    //spells and active upgrades
    public int typeOfDamage;
    public SO_Item[] itemsOnUpgrade;

    //spells
    public int spellI;
}