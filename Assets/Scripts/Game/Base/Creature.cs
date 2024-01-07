using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Creature : MonoBehaviour
{
    [Header("Settings")]
    public float maxHp;
    public float movementSpeed;

    [Header("0 - fire, 1 - water, 2 - earth, 3 - air")]
    public float[] elementalDamageMultipliers;

    [Header("Fire")]
    public float inLavaSpeed;
    public bool burningDealsDamage;
    public int averageTimeOfBurning;
    public float damageOfBurning;

    [Header("Water")]
    public float inWaterSpeed;
    public bool waterDealsDamage;
    public float durationOfDrying;
    public float damageOnWet;

    [Header("Air")]
    public float timeForPush;

    [Header("Earth")]
    public float stunMultiplier = 1;

    [Header("Other")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sr;

    [SerializeField] ParticleSystem burningEffect;
    [SerializeField] ParticleSystem wetEffect;
    [SerializeField] ParticleSystem stunnedEffect;

    //inheriting scripts
    [HideInInspector] public float curMovementSpeed;
    [HideInInspector] public Vector2 directionOfMovement;

    protected float MaxHp { get { return maxHp; } set { maxHp = value; } }
    protected Rigidbody2D Rb { get { return rb; } set { rb = value; } }
    protected SpriteRenderer Sr { get { return sr; } set { sr = value; } }
    protected float CurMovementSpeed { get { return curMovementSpeed; } set { curMovementSpeed = value; } }
    protected float SpeedMultiplierOnDamage { get { return speedMultiplierOnDamage; } set { speedMultiplierOnDamage = value; } }
    protected Vector2 DirectionOfMovement { get { return directionOfMovement; } set { directionOfMovement = value; } }
    protected bool CanMove { get { return canMove; } set { canMove = value; } }

    public event Action HealthChanged;

    public float CurHp
    {
        get { return curHp; }
        set
        {
            if (curHp != value)
            {
                curHp = value;
                OnHealthChanged();
            }
        }
    }

    //local
    System.Action handleWaterEncounter;

    //health
    [HideInInspector] public float curHp;

    //movement
    bool canMove = true;
    float movementMultiplier = 1;
    float speedMultiplierOnDamage = 1;
    [HideInInspector] public bool isLookingOnRight;

    //elemenatal bools
    [HideInInspector] public bool isBurning;
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

    //visualisation of damage
    Color normalColor = new Color(255, 255, 255);
    Color damageColor = new Color(0, 0, 0);

    //cor
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

    protected virtual void Start()
    {
        if (waterDealsDamage || GameData.I.isWaterPoisened) SetWaterDealsDamage();
        else handleWaterEncounter = OnWater;
        
        CurHp = maxHp;
        curMovementSpeed = movementSpeed;
    }

    //movement
    protected virtual void Update()
    {
        if (canMove && !isStunned && !isPushed)
        {
            rb.velocity = directionOfMovement * curMovementSpeed * movementMultiplier * speedMultiplierOnDamage; 
        }
    }

    public void ChangeLookingDirection()
    {
        sr.flipX = !sr.flipX;
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
        burningCor = StartCoroutine(BurningCor(damageOfBurning * elementalDamageMultipliers[0]));
    }
    IEnumerator BurningCor(float damage)
    {
        int curTime = 0;
        //burn untill cur burning time is less than duration of burning. change hp while burning

        while (curTime < curTimeOfBuring)
        {
            curTime++;
            ChangeHPWithoutSound(damage, 0);

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
        rb.AddForce(directionOfPush * movementMultiplier * powerOfPush * elementalDamageMultipliers[3], ForceMode2D.Impulse);

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

            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.5f));
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
        stunCor = StartCoroutine(StunCor(timeOfStun * elementalDamageMultipliers[2] * stunMultiplier));
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
            ChangeHPWithoutSound(damageOnWet, 1);

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
        ChangeSrColor(damageColor);

        yield return new WaitForSeconds(0.15f);

        ChangeSrColor(normalColor);
        visualiseDamageCor = null;
    }
    public void ChangeSrColor(Color color) => sr.color = color;
    public void SetNormalColor() => ChangeSrColor(normalColor);//after freeze
    
    //health 
    public void ChangeHPWithoutSound(float val, int type)
    {
        if (type != -1) val *= elementalDamageMultipliers[type];
        CurHp = ChangeStat(val, CurHp, maxHp);
    }
    public virtual void ChangeHP(float val, int type)
    {
        if (type != -1) AudioManager.I.PlayOneShot("elemental", type);
        else AudioManager.I.PlayOneShot("damage");
        ChangeHPWithoutSound(val, type);
    }
    public float ChangeStat(float val, float curStat, float maxStat)
    {
        //get new stats from 0 to 100 
        float newStat = curStat + val;
        //set new stats from 0 to 100. depending on whether val is - or + check if its less or more than 100 or 0 
        return newStat = (val > 0 ? Mathf.Min(newStat, maxStat) : Mathf.Max(0, newStat));
    }

    //outside
    public void SetWaterDealsDamage()
    {
        handleWaterEncounter = OnWaterDealsDamage;
        damageOnWet *= GameData.I.poisonDamageMultiplier;
    }

    //events
    protected virtual void OnHealthChanged()
    {
        HealthChanged?.Invoke();
    }
}
