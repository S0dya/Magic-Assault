using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Spell
{
    [Header("This spell")]
    [SerializeField] CircleCollider2D collider;

    void Start()
    {
        collider.radius = size;
    }
}
