using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpell : Spell
{
    public CircleCollider2D circleCol;
    protected virtual void Start() => circleCol.radius = size / 2;

    //disable for proper trigger detection
    protected override void OnDisable()
    {
        circleCol.enabled = false;

        base.OnDisable();
    }
}
