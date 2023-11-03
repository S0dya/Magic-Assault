using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [Header("Settings")]
    public ParticleSystem ps;

    //before play all needed parametrs are set inside other spells scripts
    protected virtual void Start()
    {
        ps.Play();
    }

    //main module
    public void SetSize(float min, float max)
    {
        var main = GetMainModule();
        main.startSize = new ParticleSystem.MinMaxCurve(min, max);
    }
    public void SetSize(float size)
    {
        var main = GetMainModule();
        main.startSize = size;
    }



    //since its only can be used as instance
    ParticleSystem.MainModule GetMainModule()
    {
        return ps.main;
    }

    //shape 
    public void SetAngle(float angle)
    {
        var shape = GetShape();
        shape.rotation = new Vector3(0f, 0f, angle);
    }

    ParticleSystem.ShapeModule GetShape()
    {
        return ps.shape;
    }
}
