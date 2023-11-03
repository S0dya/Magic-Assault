using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Spell
{
    [Header("This spell")]
    [SerializeField] CircleCollider2D collider;

    //[HideInInspector] 
    public float size;

    protected override void Start()
    {
        SetSize(size);
        collider.radius = size;

        base.Start();
    }
}
