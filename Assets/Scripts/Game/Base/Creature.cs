using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Settings")]
    public float maxHp;
    public float movementSpeed;
    public float inWaterSpeed;

    public float pushMultiplier;

    [Header("Coroutines time values")]
    public int durationOfBurning;
    public float durationOfDrying;
    public float takingDamageVisualisationTime;
    public float timeForPush;

    [Header("Health restoring")]
    public bool canRestoreHp;
    public float amountOfTimeBeforeRestoringHp;
    public float amountOfTimeForRestoringHp;
    public float amountOfRestoringHp;

    [Header("0 - fire, 1 - water, 2 - earth, 3 - air")]
    public float[] elementalDamageMultipliers; 
    public bool burningDealsDamage;
    public bool waterDealsDamage;

    [Header("Other")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sr;

    [SerializeField] ParticleSystem burningEffect;
    [SerializeField] ParticleSystem wetEffect;
    [SerializeField] ParticleSystem stunnedEffect;

    [SerializeField] Color normalColor;
    [SerializeField] Color damageColor;

    //inheriting scripts
    float curMovementSpeed;
    Vector2 directionOfMovement;

    protected Rigidbody2D Rb { get { return rb; } set { rb = value; } }
    protected float CurMovementSpeed { get { return curMovementSpeed; } set { curMovementSpeed = value; } }
    protected Vector2 DirectionOfMovement { get { return directionOfMovement; } set { directionOfMovement = value; } }
    protected bool CanMove { get { return canMove; } set { canMove = value; } }


    //local
    System.Action handleWaterEncounter;

    //health
    [HideInInspector] public float curHp;

    //movement
    bool canMove = true;
    float movementMultiplier = 1;

    //elemenatal bools
    bool isBurning;
    [HideInInspector] public bool isPushed;
    bool isWet;
    [HideInInspector] public bool isStunned;

    //fire
    float curBurningTime;

    //water
    int amountOfTriggeredWater;

    //cor
    Coroutine restoreHpCor;

    //fire
    Coroutine burningCor;
    //water
    Coroutine dryCor;
    Coroutine damageOnWaterCor;
    //push
    Coroutine pushCor;
    //stun
    Coroutine stunCor;

    protected virtual void Awake()
    {
        handleWaterEncounter = (waterDealsDamage ? OnWaterDealsDamage : OnWater);
    }

    protected virtual void Start()
    {
        curHp = maxHp;
        curMovementSpeed = movementSpeed;
    }

    protected virtual void Update()
    {
        if (canMove && !isStunned && !isPushed)
        {
            rb.velocity = directionOfMovement * curMovementSpeed * movementMultiplier;
        }
    }


    //burning
    public virtual void Burn(float damage, int timeOfBurning)
    {
        //if creature is in water they cant burn
        if (!burningDealsDamage || amountOfTriggeredWater > 0) return;
        //dont burn if creature is wet
        if (isWet)
        {
            StopBeingWet();
            return;
        }
        //set burning time to zero, so creature can burn, or will "start burning" again 
        curBurningTime = 0;
        //return if already burning
        if (isBurning) return;

        //play effects for visualisation
        burningEffect.Play();

        //stop restoring hp and start burning
        isBurning = true;
        if (restoreHpCor != null) StopCoroutine(restoreHpCor);
        burningCor = StartCoroutine(BurningCor(damage * elementalDamageMultipliers[0], timeOfBurning));
    }
    IEnumerator BurningCor(float damage, int timeOfBurning)
    {
        //burn untill cur burning time is less than duration of burning. change hp while burning
        while (curBurningTime < timeOfBurning)
        {
            curBurningTime++;
            ChangeHP(damage, 0);

            yield return new WaitForSeconds(2f);
        }

        StopBurning();
    }
    public virtual void StopBurning()
    {
        //stop visualising burning
        burningEffect.Stop();

        if (burningCor != null) StopCoroutine(burningCor);
        isBurning = false;
    }

    //push
    public virtual void Push(Vector2 posOfPush, float powerOfPush)
    {
        //push creature in a specified direction with all needed multipliers
        rb.AddForce(posOfPush * pushMultiplier * movementMultiplier * powerOfPush * elementalDamageMultipliers[3], ForceMode2D.Impulse);

        if (pushCor != null) StopCoroutine(pushCor);
        pushCor = StartCoroutine(PushCor());
    }
    IEnumerator PushCor()
    {
        isPushed = true;
        
        yield return new WaitForSeconds(timeForPush);

        isPushed = false;
        pushCor = null;
    }

    //stan
    public virtual void Stun(float timeOfStun)
    {
        //stop creature at one place and play stunned effect for visualisation
        if (!isPushed) stunnedEffect.Play();
        rb.velocity = Vector2.zero;

        //start or restart stun coroutine
        if (stunCor != null) StopCoroutine(stunCor);
        stunCor = StartCoroutine(StunCor(timeOfStun * elementalDamageMultipliers[2]));
    }
    IEnumerator StunCor(float timeOfStun)
    {
        isStunned = true;

        yield return new WaitForSeconds(timeOfStun);

        stunnedEffect.Stop();
        isStunned = false;
        stunCor = null;
    }

    //water
    public virtual void EnterWater()
    {
        //since creature can be in different amounts of water at the same time we add this water encounter
        amountOfTriggeredWater++;

        //start handling logic of water encounter
        if (amountOfTriggeredWater == 1) handleWaterEncounter.Invoke();
    }
    void OnWater()
    {
        //creature is wet if its in water
        if (!isWet) StartBeingWet();
        else if (dryCor != null) StopCoroutine(dryCor);

        //water makes creature stop burning
        if (isBurning) StopBurning();
        //creature moves slower in water or faster
        movementMultiplier = inWaterSpeed;
    }
    void StartBeingWet()
    {
        wetEffect.Play();
        isWet = true;
    }
    void OnWaterDealsDamage() //since some creatures might get damage from water
    {
        //call base logic
        OnWater();
        //start dealing damage while creature is wet
        StartCoroutine(DamageOnWaterCor());
    }
    IEnumerator DamageOnWaterCor()
    {
        while (isWet)
        {
            ChangeHP(-1, 1);

            yield return new WaitForSeconds(2f);
        }
    }
    public virtual void ExitWater()
    {
        amountOfTriggeredWater--;

        //set movement multiplier back to one if creature left water
        if (amountOfTriggeredWater == 0)
        {
            movementMultiplier = 1f;
            //creature dries after specified time
            dryCor = StartCoroutine(DryCor());
        }
    }
    IEnumerator DryCor()
    {
        yield return new WaitForSeconds(durationOfDrying);

        StopBeingWet();
    }
    
    void StopBeingWet()
    {
        wetEffect.Stop();
        if (dryCor != null) StopCoroutine(dryCor);
        isWet = false;
    }

    IEnumerator VisualiseDamage()
    {
        sr.color = damageColor;

        yield return new WaitForSeconds(takingDamageVisualisationTime);

        sr.color = normalColor;
    }

    //health 
    public virtual void ChangeHP(float val, int type)
    {
        if (type != -1) val *= elementalDamageMultipliers[type];
        curHp = ChangeStat(val, curHp, maxHp);
        
        //visualise hp damage
        if (val < 0) StartCoroutine(VisualiseDamage());

        //start coroutines to restore stats
        if (canRestoreHp && !isBurning)//creature cant restore hp while burning
        {
            if (restoreHpCor != null) StopCoroutine(restoreHpCor);
            restoreHpCor = StartCoroutine(RestoreHpCor());
        }
    }
    public float ChangeStat(float val, float curStat, float maxStat)
    {
        //get new stats from 0 to 100 
        float newStat = curStat + val;
        //set new stats from 0 to 100. depending on whether val is - or + check if its less or more than 100 or 0 
        return newStat = (val > 0 ? Mathf.Min(newStat, maxStat) : Mathf.Max(0, newStat));
    }

    IEnumerator RestoreHpCor()
    {
        yield return new WaitForSeconds(amountOfTimeBeforeRestoringHp);

        while (curHp < maxHp)
        {
            yield return new WaitForSeconds(amountOfTimeForRestoringHp);

            ChangeHP(amountOfRestoringHp, -1);
        }

        restoreHpCor = null;
    }
}
