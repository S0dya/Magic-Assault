using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Spell
{
    [Header("This spell")]
    [SerializeField] BoxCollider2D collider;

    protected override void Start()
    {
        float size = Settings.sizeOfWind;
        SetSize(size);
        collider.size = new Vector2(size, size);

        base.Start();
    }
}
