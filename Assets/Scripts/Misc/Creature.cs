using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Settings")]
    public float maxHp;
    public float movementSpeed;

    public float pushMultiplier;
    public int durationOfBurning;
    public float takingDamageVisualisationTime;
    public float timeForPushEnd;

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
    protected Rigidbody2D Rb { get { return rb; } set { rb = value; } }
    Vector2 directionOfMovement;
    protected Vector2 DirectionOfMovement { get { return directionOfMovement; } set { directionOfMovement = value; } }

    //local

    //health
    [HideInInspector] public float curHp;

    //movement
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
    Coroutine waitForPushEndCor;

    protected virtual void Start()
    {
        curHp = maxHp;
    }

    protected virtual void Update()
    {
        if (!isPushed)
        {
            rb.velocity = directionOfMovement * movementSpeed * movementMultiplier;
        }
    }


    //burning
    public virtual void StartBurning(float damage)
    {
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
        rb.AddForce(posOfPush * pushMultiplier * movementMultiplier * powerOfPush, ForceMode2D.Impulse);

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
        amountOfTriggeredWater++;
        if (amountOfTriggeredWater == 1)
        {
            if (isBurning) StopBurning();
            movementMultiplier = 0.6f;
        }
    }
    public virtual void ExitWater()
    {
        amountOfTriggeredWater--;
        if (amountOfTriggeredWater == 0) movementMultiplier = 1f;
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
