using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    [Header("Enemy settings")]
    public float speedOnTriggerWithPlayer;
    public float damageOnTriggerWithPlayer;
    public int typeOfDamageOnTriggerWithPlayer;
    public float coinSpawnChance;

    [Header("Animation")]
    public float[] rotation;

    [Header("Other")]
    [SerializeField] GameObject expPrefab;
    [SerializeField] GameObject coinPrefab;

    //local
    [HideInInspector] public Transform playerTransform;
    Transform expParent;
    Transform coinParent;

    //looking direction
    float xOfMove;

    //animation
    LTDescr rotationTween;

    //cors
    Coroutine burningCor;
    Coroutine waitForPushEndCor;

    protected override void Awake()
    {
        base.Awake();

        expParent = GameObject.FindGameObjectWithTag("ExpParent").GetComponent<Transform>();
        coinParent = GameObject.FindGameObjectWithTag("CoinParent").GetComponent<Transform>();
    }

    protected override void Start()
    {
        base.Start();

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (rotation.Length > 0) RotateBackwards();
    }

    public void CheckLookingDirection() => CheckLookingDirection(playerTransform.position.x);
    public void CheckLookingDirection(float targetPosX)
    {
        xOfMove = targetPosX - transform.position.x;
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
    public void Kill() //visualise death, instantiate items on death and destroy object 
    {
        ToggleMovement(false);
        Push((transform.position - playerTransform.position).normalized, 0.5f);

        LeanTween.alpha(gameObject, 0f, 0.75f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
        {
            Die();
            UIInGame.I.AddKill();

            InstantiateAfterDeath(expPrefab, expParent);
            if (Random.Range(0, 15) <= coinSpawnChance) InstantiateAfterDeath(coinPrefab, coinParent);
        });
    }
    public void InstantiateAfterDeath(GameObject prefab, Transform parent) => Instantiate(prefab, transform.position, Quaternion.identity, parent);
    
    public void Die() => Destroy(gameObject);
    
    //animation
    void RotateBackwards() => rotationTween = LeanTween.rotateZ(gameObject, rotation[0], curMovementSpeed).setEase(LeanTweenType.easeInOutSine).setOnComplete(RotateForwards);
    void RotateForwards() => rotationTween = LeanTween.rotateZ(gameObject, rotation[1], curMovementSpeed).setEase(LeanTweenType.easeInOutSine).setOnComplete(RotateBackwards);
}
