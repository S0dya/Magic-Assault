using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Spell
{
    [Header("This spell")]
    [SerializeField] CircleCollider2D col;

    void Start()
    {
        col.radius = size/2;
    }


}
