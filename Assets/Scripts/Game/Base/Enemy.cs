using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    [Header("Enemy settings")]
    public float speedOnTriggerWithPlayer;
    public float damageOnTriggerWithPlayer;
    public int typeOfDamageOnTriggerWithPlayer;

    [Header("Skin")]
    [SerializeField] Sprite[] skins;
    [SerializeField] int skinsN;

    [Header("Colliders")]
    [SerializeField] BoxCollider2D enemyBoxCollider;
    [SerializeField] CircleCollider2D enemyCircleCollider;

    [SerializeField] GameObject damageCollidersObj;

    //local
    [HideInInspector] public Transform playerTransform;

    //looking direction
    float xOfMove;

    //for damage trigger
    [HideInInspector] public bool canDealDamage = true;

    //cors
    Coroutine burningCor;
    Coroutine waitForPushEndCor;

    protected override void Start()
    {
        base.Start();

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Sr.sprite = skins[Random.Range(0, skinsN)]; 
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
        val *= Settings.damageMultipliers[typeOfDamage];
        base.ChangeHP(val, typeOfDamage);

        int valForVisual = (int)-val;
        GameData.I.damageDone += valForVisual;//add damage amount to settings' data 
        if (typeOfDamage != -1) GameData.I.elementalDamageDone[typeOfDamage] += valForVisual;

        UIInGame.I.InstantiateTextOnDamage(transform.position, valForVisual, typeOfDamage);

        VisualiseDamage();
        if (curHp == 0) Kill();
    }

    //killed
    public void Kill() //visualise death, instantiate items on death and destroy object 
    {
        DisableColliders();//for better visualisation

        ToggleMovement(false);
        Push((transform.position - playerTransform.position).normalized, 0.5f);
        UIInGame.I.AddKill();
     
        Die();
    }
    public void Die() => LeanTween.alpha(gameObject, 0f, 0.6f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => Destroy(gameObject));

    void DisableColliders()
    {
        enemyBoxCollider.enabled = false;
        enemyCircleCollider.enabled = false;

        ToggleDamageColliders(false);
    }

    public void ToggleDamageColliders(bool toggle)
    {
        canDealDamage = toggle;
        damageCollidersObj.SetActive(toggle);
    }
}
