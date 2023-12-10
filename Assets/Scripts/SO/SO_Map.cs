using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Map : SO_Item
{
    //main data
    [field: SerializeField] public string timeLimit { get; set; }
    [field: SerializeField] public int clockSpeed { get; set; }
    [field: SerializeField] public int moveSpeed { get; set; }

    //additional
    [field: SerializeField] public int goldBonus { get; set; }
    [field: SerializeField] public int luckBonus { get; set; }
    [field: SerializeField] public int expBonus { get; set; }
}
