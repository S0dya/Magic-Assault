using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    [Header("Enemy settings")]
    public float speedOnTriggerWithPlayer;
    public float damageOnTriggerWithPlayer;
    public int typeOfDamageOnTriggerWithPlayer;

    //spawn
    public float coinSpawnChance;
    public bool spawnsUpgradeItem;

    [Header("Other")]
    [SerializeField] GameObject expPrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject littleBagOfGoldPrefab;

    [SerializeField] GameObject[] upgradePrefabs;

    [Header("Colliders")]
    [SerializeField] BoxCollider2D enemyBoxCollider;
    [SerializeField] CircleCollider2D enemyCircleCollider;
    [SerializeField] BoxCollider2D damageTriggerMelee;
    [SerializeField] BoxCollider2D damageTriggerDistance;

    //local
    [HideInInspector] public Transform playerTransform;
    Transform expParent;
    Transform coinParent;
    Transform upgradesParent;

    //looking direction
    float xOfMove;

    //cors
    Coroutine burningCor;
    Coroutine waitForPushEndCor;

    protected override void Awake()
    {
        base.Awake();

        expParent = GameObject.FindGameObjectWithTag("ExpParent").GetComponent<Transform>();
        coinParent = GameObject.FindGameObjectWithTag("CoinParent").GetComponent<Transform>();
        upgradesParent = GameObject.FindGameObjectWithTag("UpgradesParent").GetComponent<Transform>();
    }

    protected override void Start()
    {
        base.Start();

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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

        VisualiseDamage();
        UIInGame.I.InstantiateTextOnDamage(transform.position, (int)-val, typeOfDamage);

        if (curHp == 0) Kill();
    }

    //killed
    public void Kill() //visualise death, instantiate items on death and destroy object 
    {
        DisableColldiers();//for better visualisation

        ToggleMovement(false);
        Push((transform.position - playerTransform.position).normalized, 0.5f);

        LeanTween.alpha(gameObject, 0f, 0.75f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
        {
            Die();
            UIInGame.I.AddKill();

            //instantiate items
            InstantiateAfterDeath(expPrefab, expParent);
            if (Random.Range(0, 15) <= coinSpawnChance) InstantiateAfterDeath((Random.Range(0, 15) < 12 ? coinPrefab : littleBagOfGoldPrefab), coinParent);
            if (spawnsUpgradeItem) InstantiateAfterDeath(upgradePrefabs[Random.Range(0, upgradePrefabs.Length)], coinParent);
        });
    }
    public void InstantiateAfterDeath(GameObject prefab, Transform parent) => Instantiate(prefab, transform.position, Quaternion.identity, parent);
    
    public void Die() => Destroy(gameObject);
    
    void DisableColldiers()
    {
        enemyBoxCollider.enabled = false;
        enemyCircleCollider.enabled = false;

        damageTriggerMelee.enabled = false;
        if (damageTriggerDistance != null) damageTriggerDistance.enabled = false;
    }
}
