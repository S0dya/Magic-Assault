using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    [Header("Enemy settings")]
    public float speedOnTriggerWithPlayer;
    public float damageOnTriggerWithPlayer;
    public int typeOfDamageOnTriggerWithPlayer;

    [Header("Animation")]
    public float[] rotation;

    [Header("Other")]
    [SerializeField] GameObject expPrefab;

    //local
    [HideInInspector] public Transform playerTransform;
    Transform expParent;

    //looking direction
    float xOfMove;

    //animation
    LTDescr rotationTween;

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
        if (rotation.Length > 0) RotateBackwards();
        CheckLookingDirection();
    }

    public void CheckLookingDirection()
    {
        xOfMove = playerTransform.position.x - transform.position.x;
        if ((xOfMove < 0 && !isLookingOnRight) || (xOfMove > 0 && isLookingOnRight)) ChangeLookingDirection();
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

        if (curHp == 0) Kill();
        else VisualiseDamage();
    }

    //killed
    public void Kill()
    {
        ToggleMovement(false);
        Push((transform.position - playerTransform.position).normalized, 0.5f);

        LeanTween.alpha(gameObject, 0f, 0.75f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
        {
            Die();
            UIInGame.I.AddKill();
            InstantiateExp();
        });
    }
    public void InstantiateExp() => Instantiate(expPrefab, transform.position, Quaternion.identity, expParent);

    public void Die()
    {
        LevelManager.I.SpawnEnemy(gameObject);
        Destroy(gameObject);
        Debug.Log("asd");
    }

    //animation
    void RotateBackwards() => rotationTween = LeanTween.rotateZ(gameObject, rotation[0], curMovementSpeed).setEase(LeanTweenType.easeInOutSine).setOnComplete(RotateForwards);
    void RotateForwards() => rotationTween = LeanTween.rotateZ(gameObject, rotation[1], curMovementSpeed).setEase(LeanTweenType.easeInOutSine).setOnComplete(RotateBackwards);
}
