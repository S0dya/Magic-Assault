using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Spell
{
    [Header("This spell")]
    public float size;
    public float angle;

    protected override void Start()
    {
        SetSize(size);
        SetAngle(angle);

        base.Start();
    }

    void OnParticleCollider(GameObject obj)
    {
        Debug.Log("asd");
    }
}
