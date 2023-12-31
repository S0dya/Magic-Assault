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
    ActiveUpgrades activeUpgrades;
    GameData gameData;

    //looking direction
    float xOfMove;

    //damage trigger
    [HideInInspector] public bool canDealDamage = true;

    //item spawn after death
    [HideInInspector] public bool isKilled;

    //cors
    Coroutine burningCor;
    Coroutine waitForPushEndCor;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        activeUpgrades = playerTransform.gameObject.GetComponent<ActiveUpgrades>();

        activeUpgrades.AddEnemy(transform);
    }

    protected override void Start()
    {
        base.Start();

        gameData = GameData.I;

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
        if (typeOfDamage != -1) val *= Settings.damageMultipliers[typeOfDamage] * gameData.power;
        base.ChangeHP(val, typeOfDamage);

        int valForVisual = (int)-val;
        gameData.damageDone += valForVisual;//add damage amount to settings' data 
        if (typeOfDamage != -1) gameData.elementalDamageDone[typeOfDamage] += valForVisual;

        UIInGame.I.InstantiateTextOnDamage(transform.position, valForVisual, typeOfDamage);

        VisualiseDamage();
        if (curHp == 0) Kill();
    }

    //killed
    public void Kill() //visualise death, instantiate items on death and destroy object 
    {
        isKilled = true;

        DisableColliders();//for better visualisation

        ToggleMovement(false);
        Push((transform.position - playerTransform.position).normalized, 0.5f);
        UIInGame.I.AddKill();
        
        Die();
    }
    public void Die()
    {
        LeanTween.alpha(gameObject, 0f, 0.6f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => Destroy(gameObject));
        activeUpgrades.RemoveEnemy(transform);
    }

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
