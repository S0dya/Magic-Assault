using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Spell
{
    protected override void Awake()
    {
        SetSize(Settings.sizeOfFire[0], Settings.sizeOfFire[1]);
        base.Awake();
    }

    void OnParticleCollision(GameObject obj)
    {
        Debug.Log("123123");
    }
}
