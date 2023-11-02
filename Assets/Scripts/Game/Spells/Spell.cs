using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [Header("Settings")]
    public ParticleSystem ps;
    
    ParticleSystem.MainModule mainModule;

    protected virtual void Awake()
    {
        mainModule = ps.main;
        ps.Play();
    }

    public void SetSize(float min, float max)
    {
        var main = GetMainModule();
        main.startSize = new ParticleSystem.MinMaxCurve(min, max);
    }

    ParticleSystem.MainModule GetMainModule()
    {
        return ps.main;
    }

    void OnParticleCollision(GameObject obj)
    {
        Debug.Log("4");
    }

    void OnTrigerEnter2D(Collider2D collision)
    {
        Debug.Log("c");
    }
}
