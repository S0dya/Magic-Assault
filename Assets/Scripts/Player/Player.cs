using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Creature
{
    [Header("Player's settings")]
    public float speedOnDamage;
    [SerializeField] float maxMana;
    [SerializeField] float amountOfTimeBeforeRestoringMana;
    [SerializeField] float amountOfTimeForRestoringMana;
    [SerializeField] float amountOfRestoringMana;

    [Header("Animation")]
    [SerializeField] Animator animator;

    [Header("UI")]
    [SerializeField] Image[] statsImages; //0 - hp, 1 - mana

    [HideInInspector] public float curMana;
    [HideInInspector] public Joystick joystick;
    [HideInInspector] public bool joystickInput;

    //local
    [HideInInspector] public Vector2 lastJoystickDirection;

    float xOfMove;

    int curAmountOfEnemies;

    //cors
    Coroutine restoreManaCor;
    Coroutine visualiseDamage;

    protected override void Start()
    {
        base.Start();

        curMana = maxMana;
    }

    protected override void Update()
    {
        DirectionOfMovement = joystick.Direction;
        animator.speed = directionOfMovement.magnitude;//set animator for walking visualisation
        
        xOfMove = directionOfMovement.x;
        if ((xOfMove < 0 && !isLookingOnRight) || (xOfMove > 0 && isLookingOnRight)) 
            ChangeLookingDirection();//rotate player if they go in another direction

        base.Update();
    }

    //joystick input
    public void ToggleJoystickInput(bool val)
    {
        joystickInput = val;
        if (!val)
        {
            if (IsJoystickDirectionNotZero()) lastJoystickDirection = directionOfMovement;

            Rb.velocity = Vector2.zero;

            StartCoroutine(UnSetCheckCor());//skip this frame for proper value set
        }
    }
    IEnumerator UnSetCheckCor()
    {
        yield return null;

        DrawManager.I.inputChecked = false;
    }

    public bool IsJoystickDirectionNotZero()
    {
        return directionOfMovement != Vector2.zero;
    }

    //UI
    public override void ChangeHP(float val, int typeOfDamage)
    {
        base.ChangeHP(val, typeOfDamage);

        if (curHp == 0)
        {
            Debug.Log("die");
        }
        else if (val < 0) VisualiseDamage(); //visualise hp damage

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

    //on enemies trigger
    public void EnemyTriggerEnter()
    {
        curAmountOfEnemies++;

        if (curAmountOfEnemies == 1) DamageMultiplierOnDamage = speedOnDamage;
    }

    public void EnemyTriggerExit()
    {
        curAmountOfEnemies--;

        if (curAmountOfEnemies == 0) DamageMultiplierOnDamage = 1;
    }

}
