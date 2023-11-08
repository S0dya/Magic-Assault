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
    public float timeForPushEnd;

    [Header("Health restoring")]
    public bool canRestoreHp;
    public float amountOfTimeBeforeRestoringHp;
    public float amountOfTimeForRestoringHp;
    public float amountOfRestoringHp;

    [Header("Other")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sr;

    [SerializeField] ParticleSystem burningEffect;
    [SerializeField] ParticleSystem wetEffect;

    //inheriting scripts
    float curMovementSpeed;
    Vector2 directionOfMovement;

    protected Rigidbody2D Rb { get { return rb; } set { rb = value; } }
    protected float CurMovementSpeed { get { return curMovementSpeed; } set { curMovementSpeed = value; } }
    protected Vector2 DirectionOfMovement { get { return directionOfMovement; } set { directionOfMovement = value; } }
    protected bool CanMove { get { return canMove; } set { canMove = value; } }


    //local

    //health
    [HideInInspector] public float curHp;

    //movement
    bool canMove = true;
    float movementMultiplier = 1;

    //elemenatal bools
    bool isBurning;
    bool isPushed;
    bool isWet;

    //fire
    float burningTime;

    //water
    int amountOfTriggeredWater;

    //cor
    Coroutine restoreHpCor;

    Coroutine burningCor;
    Coroutine dryCor;
    Coroutine waitForPushEndCor;

    protected virtual void Start()
    {
        curHp = maxHp;
        curMovementSpeed = movementSpeed;
    }

    protected virtual void Update()
    {
        if (canMove && !isPushed)
        {
            rb.velocity = directionOfMovement * curMovementSpeed * movementMultiplier;
        }
    }


    //burning
    public virtual void Burn(float damage)
    {
        //dont burn if creature is wet
        if (isWet)
        {
            StopBeingWet();
            return;
        }
        //set burning time to zero, so creature can burn, or will "start burning" again 
        burningTime = 0;
        //return if already burning
        if (isBurning) return;

        //play effects for visualisation
        burningEffect.Play();

        //stop restoring hp and start burning
        isBurning = true;
        if (restoreHpCor != null) StopCoroutine(restoreHpCor);
        burningCor = StartCoroutine(BurningCor(damage));
    }
    IEnumerator BurningCor(float damage)
    {
        //burn untill cur burning time is less than duration of burning. change hp while burning
        while (burningTime < durationOfBurning)
        {
            burningTime++;
            ChangeHP(damage);

            yield return new WaitForSeconds(2f);
        }

        StopBurning();
    }
    public virtual void StopBurning()
    {
        burningEffect.Stop();
        if (burningCor != null) StopCoroutine(burningCor);
        isBurning = false;
    }

    //push
    public virtual void Push(Vector2 posOfPush, float powerOfPush)
    {
        //push creature in a specified direction with all needed multipliers
        rb.AddForce(posOfPush * pushMultiplier * movementMultiplier * powerOfPush, ForceMode2D.Impulse);

        //creature might cant walk when pushed
        if (waitForPushEndCor != null) StopCoroutine(waitForPushEndCor);
        waitForPushEndCor = StartCoroutine(WaitForPushEndCor());
    }
    IEnumerator WaitForPushEndCor()
    {
        isPushed = true;
        yield return new WaitForSeconds(timeForPushEnd);
        isPushed = false;
    }

    IEnumerator VisualiseDamage()
    {
        sr.color = new Color(0, 0, 0);
        yield return new WaitForSeconds(takingDamageVisualisationTime);
        sr.color = new Color(255, 255, 255);
    }

    //water
    public virtual void EnterWater()
    {
        //since creature can be in different amounts of water at the same time we add this water encounter
        amountOfTriggeredWater++;

        if (amountOfTriggeredWater == 1)
        {
            //creature is wet if its in water
            if (!isWet) StartBeingWet();
            else if (dryCor != null) StopCoroutine(dryCor);

            //water makes creature stop burning
            if (isBurning) StopBurning();
            //creature moves slower in water or faster
            movementMultiplier = inWaterSpeed;
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
    void StartBeingWet()
    {
        wetEffect.Play();
        isWet = true;
    }
    void StopBeingWet()
    {
        wetEffect.Stop();
        if (dryCor != null) StopCoroutine(dryCor);
        isWet = false;
    }


    //health 
    public virtual void ChangeHP(float val)
    {
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

            ChangeHP(amountOfRestoringHp);
        }

        restoreHpCor = null;
    }
}
