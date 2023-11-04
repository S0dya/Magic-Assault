using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Spell
{
    [Header("This spell")]
    [SerializeField] BoxCollider2D collider;


    void Start()
    {
        collider.size = new Vector2(size, size);

    }
}
