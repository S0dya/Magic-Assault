using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [Header("Settings")]
    public ParticleSystem ps;
    public Rigidbody2D rb;

    public int typeOfDamage;

    public bool worksForPlayer;
    public bool worksForEnemy;

    //main
    [HideInInspector] public float size;
    [HideInInspector] public float rotation;

    //rb
    [HideInInspector] public Vector2 directionOfPush;
    public float speed;

    //vars for inheriting scripts
    public float damage;
    [HideInInspector] public float damageMultiplier;



    public void Play() //before play all needed parametrs are set inside other spells scripts
    {
        damageMultiplier = size + 0.25f;
        damage *= damageMultiplier;

        ps.Play();
    }

    //instantiate additional effects when spell is destroyed
    protected virtual void OnDisable()
    {
        ParticleSystem fadePs = SpellsManager.I.InstantiateFadeEffect(transform.position, typeOfDamage);

        SetStartSize(fadePs, size);
    }

    //main module
    public void SetSize(float s)
    {
        size = s;

        SetStartSize(ps, size);
    }
    void SetStartSize(ParticleSystem thisPs, float size)
    {
        var main = GetMainModule(thisPs);
        main.startSize = size;
    }

    public void SetRotation(float r)
    {
        rotation = r;
        var main = GetMainModule(ps);
        main.startRotation = -rotation * Mathf.Deg2Rad;
    }

    //since its only can be used as instance
    ParticleSystem.MainModule GetMainModule(ParticleSystem thisPs)
    {
        return thisPs.main;
    }

    //rigidbody
    public void ApplyForce(Vector2 direction)
    {
        directionOfPush = direction;
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}
