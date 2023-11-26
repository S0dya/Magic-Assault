using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    Transform playerTransform;
    
    [Header("Enemy settings")]
    public float speedOnTriggerWithPlayer;
    public float damageOnTriggerWithPlayer;
    public int typeOfDamageOnTriggerWithPlayer;

    [SerializeField] GameObject expPrefab;

    //local
    Transform expParent;
    
    //cors
    Coroutine burningCor;
    Coroutine waitForPushEndCor;
    Coroutine visualiseDamage;

    protected override void Awake()
    {
        base.Awake();

        expParent = GameObject.FindGameObjectWithTag("ExpParent").GetComponent<Transform>();
    }

    protected override void Start()
    {
        base.Start();

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    protected override void Update()
    {
        DirectionOfMovement = (playerTransform.position - transform.position).normalized;

        base.Update();
    }

    public void ToggleMovement(bool val)
    {
        CanMove = val;
        if (!val) Rb.velocity = Vector2.zero;
    }

    public void ToggleMovementSpeedOnPlayerTrigger(bool val)
    {
        if (val) CurMovementSpeed = speedOnTriggerWithPlayer;
        else CurMovementSpeed = movementSpeed;
    }

    //health
    public override void ChangeHP(float val, int typeOfDamage)
    {
        base.ChangeHP(val * Settings.damageMultipliers[typeOfDamage], typeOfDamage);

        if (curHp == 0) Die();
        else VisualiseDamage();
    }

    public void Die()
    {
        ToggleMovement(false);
        Push((transform.position - playerTransform.position).normalized, 0.5f);

        LeanTween.alpha(gameObject, 0f, 0.75f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
        {
            UIInGame.I.AddKill();
            InstantiateExp();
            Destroy(gameObject);
        });
    }
    public void InstantiateExp() => Instantiate(expPrefab, transform.position, Quaternion.identity, expParent);
}
