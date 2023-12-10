using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Character : SO_Item
{
    //main data
    [field: SerializeField] public int hp { get; set; }
    [field: SerializeField] public int mana { get; set; }
    [field: SerializeField] public int shields { get; set; }

    [field: SerializeField] public int speed { get; set; }

    [field: SerializeField] public SO_GameItem[] startingSpells { get; set; }
    [field: SerializeField] public int[] multipliers { get; set; }

    //elemental data
    [field: SerializeField] public int[] damageMultipliers { get; set; }

    //fire 
    [field: SerializeField] public bool burningDealsDamage { get; set; }

    //water
    [field: SerializeField] public bool waterDealsDamage { get; set; }

    [field: SerializeField] public int luck { get; set; }
}
