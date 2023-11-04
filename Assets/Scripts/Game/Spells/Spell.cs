using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [Header("Settings")]
    public ParticleSystem ps;

    //main
    public float size;
    public float rotation;
    
    //shape
    public float angle;


    //before play all needed parametrs are set inside other spells scripts
    public void Play()
    {
        ps.Play();
    }

    //main module
    public void SetSize(float s)
    {
        size = s;
        var main = GetMainModule();
        main.startSize = size;
    }

    public void SetRotation(float r)
    {
        rotation = r;
        var main = GetMainModule();
        main.startRotation = -rotation * Mathf.Deg2Rad;
    }

    //since its only can be used as instance
    ParticleSystem.MainModule GetMainModule()
    {
        return ps.main;
    }

    //shape 
    public void SetAngle(float a)
    {
        angle = a;
        var shape = GetShape();
        shape.rotation = new Vector3(0f, 0f, angle);
    }

    ParticleSystem.ShapeModule GetShape()
    {
        return ps.shape;
    }
}
