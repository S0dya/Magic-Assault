using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : SingletonMonobehaviour<Player>
{
    [Header("Settings")]
    public float movementSpeed;

    [Header("SerializeFields")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer sr;

    [Header("UI")]
    [SerializeField] FloatingJoystick flJoystick;
    [SerializeField] FixedJoystick fxJoystick;
    Joystick joystick;

    [SerializeField] Image[] statsImages; //0 - hp, 1 - mana

    //UI
    float[] maxStats;
    [HideInInspector] public float[] curStats = { 100, 100 };

    float[] amountOfTimeBeforeRestoringStats;
    float[] amountOfTimeForRestoringStats;
    float[] amountOfRestoringStats;
    
    //local 
    [HideInInspector] public bool joystickInput;

    float movementMultiplier = 1;
    float pushMultiplier = 4;

    int durationOfBurning;
    float takingDamageVisualisationTime;
    float timeForPushEnd;

    bool canRestoreHp;

    bool isBurning;
    bool isPushed;
    bool isWet;

    float burningTime;

    //cor
    Coroutine restoreHpCor;
    Coroutine restoreManaCor;

    Coroutine burningCor;
    Coroutine waitForPushEndCor;

    protected override void Awake()
    {
        base.Awake();

        SetJoystick();
    }

    void Start()
    {
        amountOfTimeBeforeRestoringStats = Settings.amountOfTimeBeforeRestoringStats;
        amountOfTimeForRestoringStats = Settings.amountOfTimeForRestoringStats;
        amountOfRestoringStats = Settings.amountOfRestoringStats;

        durationOfBurning = Settings.durationOfBurning;
        takingDamageVisualisationTime = Settings.takingDamageVisualisationTime;
        timeForPushEnd = Settings.timeForPushEnd;

        canRestoreHp = Settings.canRestoreHp;
    }

    void Update()
    {
        if (joystickInput && !isPushed)
        {
            rb.velocity = joystick.Direction * movementSpeed * movementMultiplier;
        }
    }


    //joystick input
    public void ToggleJoystickInput(bool val)
    {
        joystickInput = val;
        if (!val)
        {
            rb.velocity = Vector2.zero;
            //skip this frame for proper value set
            StartCoroutine(UnSetCheckCor());
        }
    }
    IEnumerator UnSetCheckCor()
    {
        yield return null;

        DrawManager.I.inputChecked = false;
    }

    //other methods
    void SetJoystick()
    {
        //set joystick's type
        bool isFJ = Settings.isFloatingJoystick;

        if (isFJ) flJoystick.gameObject.SetActive(true);
        else fxJoystick.gameObject.SetActive(true);

        //set this joystick
        joystick = isFJ ? flJoystick : fxJoystick;
    }

    public void StartBurning(float damage)
    {
        //set burning time to zero, so player can burn, or will "start burning" again 
        burningTime = 0;
        //return if already burning
        if (isBurning) return;

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
    public void StopBurning()
    {
        if (burningCor != null) StopCoroutine(burningCor);
        isBurning = false;
    }

    public void Push(Vector2 posOfPush)
    {
        Vector2 direction = (Vector2)transform.position - posOfPush;
        rb.AddForce(direction * pushMultiplier * movementMultiplier, ForceMode2D.Impulse);
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

    //UI
    public void ChangeHP(float val)
    {
        ChangeStats(val, 0);

        if (curStats[0] == 0)
        {
            Debug.Log("die");
        }

        //start coroutines to restore stats
        if (canRestoreHp && !isBurning)//player cant restore hp while burning
        {
            if (restoreHpCor != null) StopCoroutine(restoreHpCor);
            restoreHpCor = StartCoroutine(RestoreHpCor());
        }
    }
    public void ChangeMana(float val)
    {
        ChangeStats(val, 1);

        if (restoreManaCor != null) StopCoroutine(restoreManaCor);
        restoreManaCor = StartCoroutine(RestoreManaCor());
    }
    void ChangeStats(float val, int i)
    {
        //get new stats from 0 to 100 
        float newStat = curStats[i] + val;
        //set new stats from 0 to 100. depending on whether val is - or + check if its less or more than 100 or 0 
        if (val > 0) curStats[i] = Mathf.Min(newStat, 100);
        else
        {
            //visualise hp damage
            if (i == 0) StartCoroutine(VisualiseDamage());
            //visualise mana usage
            //else if (i == 1) play spell animation;

            curStats[i] = Mathf.Max(0, newStat);
        }
        //set stats from 0 to 1
        statsImages[i].fillAmount = curStats[i] / 100;
    }

    IEnumerator RestoreHpCor()
    {
        yield return new WaitForSeconds(amountOfTimeBeforeRestoringStats[0]);

        while (curStats[0] < 100)
        {
            yield return new WaitForSeconds(amountOfTimeForRestoringStats[0]);

            ChangeHP(amountOfRestoringStats[0]);
        }

        restoreHpCor = null;
    }
    IEnumerator RestoreManaCor()
    {
        yield return new WaitForSeconds(amountOfTimeBeforeRestoringStats[1]);

        while (curStats[1] < 100)
        {
            yield return new WaitForSeconds(amountOfTimeForRestoringStats[1]);

            ChangeMana(amountOfRestoringStats[1]);
        }

        restoreManaCor = null;
    }


    //trigger
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
