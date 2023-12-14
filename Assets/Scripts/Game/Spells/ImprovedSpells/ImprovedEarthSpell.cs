using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedEarthSpell : ImprovedSpell
{
    public float directionRadius;

    protected override void OnDisable()
    {
        InstantiateWithForce(GameData.I.amountOfAdditionalForceEffects, directionRadius);
    }
}
