using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Creature
{
    [Header("Player's settings")]
    public float speedOnDamage;
    public float maxMana;
    public float shields;
    [SerializeField] float amountOfTimeBeforeRestoringMana;
    [SerializeField] float amountOfTimeForRestoringMana;
    [SerializeField] float amountOfRestoringMana;

    [Header("Health restoring")]
    public bool canRestoreHp;
    public float amountOfTimeBeforeRestoringHp;
    public float amountOfTimeForRestoringHp;
    public float amountOfRestoringHp;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [Header("Shadow effect (0 - left, 1 - right)")]
    [SerializeField] GameObject[] shadowPrefabs;

    [Header("Damage Visualisation")]
    [SerializeField] GameObject BloodEffectObj;

    [Header("UI")]
    [SerializeField] Image[] statsImages; //0 - hp, 1 - mana

    [HideInInspector] public float curMana;
    [HideInInspector] public Joystick joystick;
    [HideInInspector] public bool joystickInput;

    //local
    [HideInInspector] public Vector2 lastJoystickDirection;

    //movement
    float xOfMove;

    //enemies trigger
    int curAmountOfEnemies;

    float shieldProtection;

    //shadow animation
    Transform shadowEffectsParent;
    int curShadowDirection = 1; 
    float shadowTimeReload = 0.025f;
    float curShadowTime;

    //cors
    Coroutine restoreHpCor;
    Coroutine restoreManaCor;
    Coroutine visualiseDamage;

    protected override void Awake()
    {
        base.Awake();

        shadowEffectsParent = GameObject.FindGameObjectWithTag("ShadowEffectsParent").transform;
    }

    protected override void Start()
    {
        base.Start();

        curMana = maxMana;
        shieldProtection = (float)shields * 0.1f;
    }

    protected override void Update()
    {
        DirectionOfMovement = joystick.Direction;
        animator.speed = directionOfMovement.magnitude;//set animator for walking visualisation
        
        xOfMove = directionOfMovement.x;
        if ((xOfMove < 0 && !isLookingOnRight) || (xOfMove > 0 && isLookingOnRight))
        {
            ChangeLookingDirection();//rotate player if they go in another direction
            curShadowDirection = (isLookingOnRight ? 0 : 1);
        }

        if (joystickInput) ShadowAnimation();

        base.Update();
    }

    void ShadowAnimation()// instnatiate shadow effect (character's sprite) each specific amount of time behind player to visualise spead
    {
        if (curShadowTime > shadowTimeReload)
        {
            Instantiate(shadowPrefabs[curShadowDirection], transform.position, Quaternion.identity, shadowEffectsParent);
            curShadowTime = 0;
        }
        else curShadowTime += Time.deltaTime;
    }

    //joystick input
    public void StartJoystickInput()
    {
        ToggleJoystickInput(true);
    }
    public void StopJoystickInput()
    {
        ToggleJoystickInput(false);

        if (IsJoystickDirectionNotZero()) lastJoystickDirection = directionOfMovement;

        Rb.velocity = Vector2.zero;

        StartCoroutine(UnSetCheckCor());//skip this frame for proper value set
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

    //elemtals
    public override void Burn()
    {
        base.Burn();

        if (restoreHpCor != null) StopCoroutine(restoreHpCor);
    }

    //UI
    public override void ChangeHP(float val, int typeOfDamage)
    {
        if (val < 0) val -= val * shieldProtection;
        base.ChangeHP(val, typeOfDamage);

        if (curHp == 0) 
        {
            //UIResults.I.SetTransparentBg();
        }
        else if (val < 0)
        {
            if (Settings.showBlood) Instantiate(BloodEffectObj, transform);
            VisualiseDamage(); //visualise hp damage
            //start coroutines to restore stats
            if (canRestoreHp && !isBurning)//creature cant restore hp while burning
            {
                if (restoreHpCor != null) StopCoroutine(restoreHpCor);
                restoreHpCor = StartCoroutine(RestoreHpCor());
            }
        }
        
        //set stats from 0 to 1
        statsImages[0].fillAmount = curHp / maxHp;
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

    //other
    public void AddShield()
    {
        Debug.Log("asd");
        shieldProtection += 0.1f;
    }

    void ToggleJoystickInput(bool val) => joystickInput = val;
}
