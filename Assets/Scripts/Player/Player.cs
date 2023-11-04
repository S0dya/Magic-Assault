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
    float[] curStats;

    float[] amountOfTimeBeforeRestoringStats;
    float[] amountOfTimeForRestoringStats;
    float[] amountOfRestoringStats;

    bool canRestoreHp;

    //cor
    Coroutine restoreHpCor;
    Coroutine restoreManaCor;

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
        bool isFJ = Settings.isFloatingJoystick;

        if (isFJ) flJoystick.gameObject.SetActive(true);
        else fxJoystick.gameObject.SetActive(true);

        joystick = isFJ ? flJoystick : fxJoystick;
    }

    //UI
    void ChangeStats(float val, int i)
    {
        //make stats be between 0 to 1
        float newStat = Mathf.Lerp(0, 1, curStats[i] + val);
        statsImages[i].fillAmount = newStat;

        if (i == 1)
        {
            if (restoreManaCor != null) StopCoroutine(restoreManaCor);
            restoreManaCor = StartCoroutine(RestoreStatsCor(i, restoreManaCor));
        }
        else if (canRestoreHp)
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
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Fire"))
        {
            Debug.Log("555");
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Fire"))
        {
            Debug.Log("555");
        }
    }
}
