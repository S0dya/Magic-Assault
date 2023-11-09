using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Creature
{
    [Header("Player's settings")]
    [SerializeField] float maxMana;
    [SerializeField] float amountOfTimeBeforeRestoringMana;
    [SerializeField] float amountOfTimeForRestoringMana;
    [SerializeField] float amountOfRestoringMana;
    
    [Header("UI")]
    [SerializeField] FloatingJoystick flJoystick;
    [SerializeField] FixedJoystick fxJoystick;
    Joystick joystick;

    [SerializeField] Image[] statsImages; //0 - hp, 1 - mana

    //UI
    [HideInInspector] public float curMana;

    //local 
    [HideInInspector] public bool joystickInput;

    //cors
    Coroutine restoreManaCor;


    protected override void Awake()
    {
        base.Awake();

        SetJoystick();
    }

    protected override void Start()
    {
        base.Start();

        curMana = maxMana;
    }

    protected override void Update()
    {
        DirectionOfMovement = joystick.Direction;

        base.Update();
    }


    //joystick input
    public void ToggleJoystickInput(bool val)
    {
        joystickInput = val;
        if (!val)
        {
            Rb.velocity = Vector2.zero;
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

    //UI
    public override void ChangeHP(float val, int typeOfDamage)
    {
        base.ChangeHP(val, typeOfDamage);

        if (curHp == 0)
        {
            Debug.Log("die");
        }

        //set stats from 0 to 1
        statsImages[0].fillAmount = curHp / maxHp;
    }
    public void ChangeMana(float val)
    {
        curMana = ChangeStat(val, curMana, maxMana);
        statsImages[1].fillAmount = curMana / maxMana;

        //visualise mana usage
        //else if (i == 1) play spell animation;

        if (restoreManaCor != null) StopCoroutine(restoreManaCor);
        restoreManaCor = StartCoroutine(RestoreManaCor());
    }
    IEnumerator RestoreManaCor()
    {
        yield return new WaitForSeconds(amountOfTimeBeforeRestoringMana);

        while (curMana < maxMana)
        {
            yield return new WaitForSeconds(amountOfTimeForRestoringMana);

            ChangeMana(amountOfRestoringMana);
        }

        restoreManaCor = null;
    }
}
