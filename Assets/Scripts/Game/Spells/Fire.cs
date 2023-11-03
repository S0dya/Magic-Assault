using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Spell
{
    [Header("This spell")]
    [SerializeField] BoxCollider2D collider;

    protected override void Start()
    {
        float size = Random.Range(Settings.sizeOfFire[0], Settings.sizeOfFire[1]);
        SetSize(size);
        collider.size = new Vector2(size, size);

        base.Start();
    }
}
