using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [Header("Settings")]
    public ParticleSystem ps;
    public Rigidbody2D rb;

    public bool worksForPlayer;
    public bool worksForEnemy;

    //main
    [HideInInspector] public float size;
    [HideInInspector] public float rotation;

    //rb
    [HideInInspector] public Vector2 directionOfPush;
    public float speed;

    //vars for inheriting scripts
    [HideInInspector] public float damageMultiplier;


    //before play all needed parametrs are set inside other spells scripts
    public void Play()
    {
        damageMultiplier = size + 0.25f;

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

    //rigidbody
    public void ApplyForce(Vector2 direction)
    {
        directionOfPush = direction;
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}
