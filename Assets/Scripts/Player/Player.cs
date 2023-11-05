using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : SingletonMonobehaviour<Player>
{
    [Header("Settings")]
    public float mocementSpeed;

    [Header("SerializeFields")]
    [SerializeField] Rigidbody2D rb;
    [Header("UI")]
    [SerializeField] FloatingJoystick flJoystick;
    [SerializeField] FixedJoystick fxJoystick;
    Joystick joystick;

    [SerializeField] Image[] statsImages; //0 - hp, 1 - mana

    
    //local 
    [HideInInspector] public bool joystickInput;

    //UI
    float[] maxStats;
    [HideInInspector] public float[] curStats = { 100, 100 };

    float[] amountOfTimeBeforeRestoringStats;
    float[] amountOfTimeForRestoringStats;
    float[] amountOfRestoringStats;

    int durationOfBurning;

    bool canRestoreHp;
    bool isBurning;

    //cor
    Coroutine restoreHpCor;
    Coroutine restoreManaCor;

    Coroutine burningCor;

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

        canRestoreHp = Settings.canRestoreHp;
    }

    void Update()
    {
        if (joystickInput)
        {
            rb.velocity = joystick.Direction * mocementSpeed;
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
        //return if already burning
        if (isBurning) return;

        //stop restoring hp and start burning
        isBurning = true;
        if (restoreHpCor != null) StopCoroutine(restoreHpCor);
        burningCor = StartCoroutine(BurningCor(damage));
    }
    IEnumerator BurningCor(float damage)
    {
        int time = 0;

        while (time < durationOfBurning)
        {
            time++;
            ChangeStats(damage, 0);

            yield return new WaitForSeconds(2f);
        }

        StopBurning();
    }
    public void StopBurning()
    {
        if (burningCor != null) StopCoroutine(burningCor);
        isBurning = false;
    }

    //UI
    public void ChangeStats(float val, int i)
    {
        //get new stats from 0 to 100 
        float newStat = curStats[i] + val;
        //set new stats from 0 to 100. depending on whether val is - or + check if its less or more than 100 or 0 
        curStats[i] = (val > 0 ? Mathf.Min(newStat, 100) : Mathf.Max(0, newStat));
        //set stats from 0 to 1
        statsImages[i].fillAmount = curStats[i] / 100;

        //hp == 0
        if (curStats[0] == 0)
        {
            Debug.Log("die");
        }

        //start coroutines to restore stats
        if (i == 1)
        {
            if (restoreManaCor != null) StopCoroutine(restoreManaCor);
            restoreManaCor = StartCoroutine(RestoreStatsCor(i, restoreManaCor));
        }
        else if (canRestoreHp && !isBurning)//player cant restore hp while burning
        {
            if (restoreHpCor != null) StopCoroutine(restoreHpCor);
            restoreHpCor = StartCoroutine(RestoreStatsCor(i, restoreHpCor));
        }
    }
    IEnumerator RestoreStatsCor(int i, Coroutine thisCor)
    {
        yield return new WaitForSeconds(amountOfTimeBeforeRestoringStats[i]);

        while (curStats[i] < 100)
        {
            yield return new WaitForSeconds(amountOfTimeForRestoringStats[i]);

            ChangeStats(amountOfRestoringStats[i], i);
        }

        thisCor = null;
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
