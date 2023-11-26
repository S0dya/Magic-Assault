using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Settings")]
    public float maxHp;
    public float movementSpeed;

    public float pushMultiplier;
    public int averageTimeOfBurning;

    [Header("Coroutines time values")]
    public float takingDamageVisualisationTime;

    [Header("Health restoring")]
    public bool canRestoreHp;
    public float amountOfTimeBeforeRestoringHp;
    public float amountOfTimeForRestoringHp;
    public float amountOfRestoringHp;

    [Header("0 - fire, 1 - water, 2 - earth, 3 - air")]
    public float[] elementalDamageMultipliers;

    [Header("Fire")]
    public float inLavaSpeed;
    public bool burningDealsDamage;
    public float damageOfBurning;

    [Header("Water")]
    public float inWaterSpeed;
    public bool waterDealsDamage;
    public float durationOfDrying;
    public float damageOnWet;

    [Header("Air")]
    public float timeForPush;

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
    [HideInInspector] public bool isLookingOnRight;

    //elemenatal bools
    bool isBurning;
    [HideInInspector] public bool isPushed;
    bool isWet;
    [HideInInspector] public bool isStunned;

    //fire
    int amountOfTriggeredLava;
    int curTimeOfBuring;

    //water
    int amountOfTriggeredWater;

    //wind
    int amountOfTriggeredTornado;
    Vector2 curPosOfCenterOfTornado;

    //cor
    Coroutine restoreHpCor;
    Coroutine visualiseDamageCor;

    //fire
    Coroutine burningCor;
    Coroutine burningInLavaCor;
    //water
    Coroutine dryCor;
    Coroutine damageOnWaterCor;
    //push
    Coroutine pushCor;
    Coroutine pushToCenterOfTornadoCor;
    //stun
    Coroutine stunCor;

    protected virtual void Awake()
    {
        if (waterDealsDamage || Settings.allWaterIsPoisened) handleWaterEncounter = OnWaterDealsDamage;
        else handleWaterEncounter = OnWater;
    }

    protected virtual void Start()
    {
        curHp = maxHp;
        curMovementSpeed = movementSpeed;
    }

    //movement
    protected virtual void Update()
    {
        if (canMove && !isStunned && !isPushed)
        {
            rb.velocity = directionOfMovement * curMovementSpeed * movementMultiplier;
        }
    }

    public void ChangeLookingDirection()
    {
        sr.flipX = isLookingOnRight;
        isLookingOnRight = !isLookingOnRight;
    }


    //burning
    public virtual void Burn()
    {
        //if creature is in water they cant burn
        if (!burningDealsDamage || amountOfTriggeredWater > 0) return;

        //dont burn if creature is wet
        if (isWet)
        {
            StopBeingWet();
            return;
        }
        curTimeOfBuring += averageTimeOfBurning;
        //set burning time to zero, so creature can burn, or will "start burning" again 
        //return if already burning
        if (isBurning) return;

        //play effects for visualisation
        burningEffect.Play();

        //stop restoring hp and start burning
        isBurning = true;
        if (restoreHpCor != null) StopCoroutine(restoreHpCor);
        burningCor = StartCoroutine(BurningCor(damageOfBurning * elementalDamageMultipliers[0]));
    }
    IEnumerator BurningCor(float damage)
    {
        int curTime = 0;
        //burn untill cur burning time is less than duration of burning. change hp while burning

        while (curTime < curTimeOfBuring)
        {
            curTime++;
            ChangeHP(damage, 0);

            yield return new WaitForSeconds(2f);
        }

        StopBurning();
    }
    public virtual void StopBurning()
    {
        //stop visualising burning
        burningEffect.Stop();
        curTimeOfBuring = 0;

        if (burningCor != null) StopCoroutine(burningCor);
        isBurning = false;
    }

    //lava
    public void EnterLava()
    {
        amountOfTriggeredLava++;

        if (amountOfTriggeredLava == 1)
        {
            burningInLavaCor = StartCoroutine(BurningInLavaCor());
            
            //creature moves slower in lava or faster
            movementMultiplier = inLavaSpeed;
        }
    }
    IEnumerator BurningInLavaCor()
    {
        float curTimeOfGettingDamageFromLava = 1.5f;

        while (true)
        {
            Burn();

            yield return new WaitForSeconds(curTimeOfGettingDamageFromLava);
            curTimeOfGettingDamageFromLava = Mathf.Max(curTimeOfGettingDamageFromLava - 0.25f, 0.25f);
        }
    }
    public void ExitLava()
    {
        amountOfTriggeredLava--;

        if (amountOfTriggeredLava == 0)
        {
            if (burningInLavaCor != null) StopCoroutine(burningInLavaCor);
            movementMultiplier = 1f;
        }
    }

    //push
    public virtual void Push(Vector2 directionOfPush, float powerOfPush)
    {
        //push creature in a specified direction with all needed multipliers
        rb.AddForce(directionOfPush * pushMultiplier * movementMultiplier * powerOfPush * elementalDamageMultipliers[3], ForceMode2D.Impulse);

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

    //tornado
    public void EnterTornado(Vector2 centerPosOfTornado)
    {
        amountOfTriggeredTornado++;
        curPosOfCenterOfTornado = centerPosOfTornado;

        if (amountOfTriggeredTornado == 1)
        {
            pushToCenterOfTornadoCor = StartCoroutine(PushToCenterOfTornadoCor());
        }
    }
    IEnumerator PushToCenterOfTornadoCor()
    {
        while (true)
        {
            Vector2 direction = (curPosOfCenterOfTornado - (Vector2)transform.position).normalized;
            Push(direction, Vector2.Distance(curPosOfCenterOfTornado, (Vector2)transform.position)/2f);

            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        }
    }
    public void ExitTornado()
    {
        amountOfTriggeredTornado--;

        if (amountOfTriggeredTornado == 0)
        {
            if (pushToCenterOfTornadoCor != null) StopCoroutine(pushToCenterOfTornadoCor);
        }
    }

    //stun
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
        if (amountOfTriggeredWater == 1)
        {
            handleWaterEncounter.Invoke();
            
            //creature moves slower in water or faster
            movementMultiplier = inWaterSpeed;
        }

    }
    void OnWater()
    {
        //creature is wet if its in water
        if (!isWet) StartBeingWet();
        else if (dryCor != null) StopCoroutine(dryCor);

        //water makes creature stop burning
        if (isBurning) StopBurning();
    }
    void StartBeingWet()
    {
        wetEffect.Play();
        isWet = true;
    }
    void OnWaterDealsDamage() //since some creatures might get damage from water, or if water is poisened
    {
        //call base logic
        OnWater();
        //start dealing damage while creature is wet
        if (damageOnWaterCor == null) damageOnWaterCor = StartCoroutine(DamageOnWaterCor());
    }
    IEnumerator DamageOnWaterCor()
    {
        while (isWet)
        {
            ChangeHP(damageOnWet, 1);

            yield return new WaitForSeconds(2f);
        }

        damageOnWaterCor = null;
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

    public void HandleWater()//method is called without creature being in water already, so we creature dryes
    {
        handleWaterEncounter.Invoke();

        if (amountOfTriggeredWater == 0)
        {
            if (dryCor != null) StopCoroutine(dryCor);
            dryCor = StartCoroutine(DryCor());
        }

    }

    //damage
    public void VisualiseDamage()//script is called from inheriting scripts
    {
        if (visualiseDamageCor == null) visualiseDamageCor = StartCoroutine(VisualiseDamageCor());
    }
    IEnumerator VisualiseDamageCor()
    {
        sr.color = damageColor;

        yield return new WaitForSeconds(takingDamageVisualisationTime);

        sr.color = normalColor;
        visualiseDamageCor = null;
    }

    //health 
    public virtual void ChangeHP(float val, int type)
    {
        if (type != -1) val *= elementalDamageMultipliers[type];
        curHp = ChangeStat(val, curHp, maxHp);

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
