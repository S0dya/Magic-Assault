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

    [Header("Magnet")]
    [SerializeField] CircleCollider2D pickingTrigger;

    [Header("UI")]
    [SerializeField] Image[] statsImages; //0 - hp, 1 - mana

    [HideInInspector] public float curMana;
    Joystick joystick;
    [HideInInspector] public bool joystickInput;

    //local
    [HideInInspector] public Vector2 lastJoystickDirection;

    System.Action movementAction;

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

    void Awake()
    {
        HealthChanged += UpdateHealthBar;
        shadowEffectsParent = GameObject.FindGameObjectWithTag("ShadowEffectsParent").transform;

        if (Settings.isQualityMedium) movementAction = ShadowMovement;
        else movementAction = NoShadowMovement;
    }

    protected override void Start()
    {
        base.Start();

        curMana = maxMana;
        shieldProtection = (float)shields * 0.1f;
    }

    public void SetJoystick(Joystick js) => joystick = js;

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

        movementAction.Invoke();

        base.Update();
    }

    //shadow annimation
    void ShadowMovement()
    {
        if (joystickInput) ShadowAnimation();
    }
    void NoShadowMovement(){}

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
    public void DecreaseHP(float val, int typeOfDamage)
    {
        val -= val * shieldProtection;
        ChangeHP(val, typeOfDamage);

        if (curHp == 0)
        {
            //UIResults.I.SetTransparentBg();
            return;
        }

        if (Settings.showBlood) Instantiate(BloodEffectObj, transform);
        VisualiseDamage(); //visualise hp damage
        //start coroutines to restore stats
        if (canRestoreHp && !isBurning)//creature cant restore hp while burning
        {
            if (restoreHpCor != null) StopCoroutine(restoreHpCor);
            restoreHpCor = StartCoroutine(RestoreHPCor());
        }
    }
    public void RestoreHPWithItem(float val)
    {
        UIInGame.I.InstantiateTextOnHPRestore(transform.position, (int)val);
        RestoreHP(val);
    }
    void RestoreHP(float val) => ChangeHP(val, -1);
    IEnumerator RestoreHPCor()
    {
        yield return new WaitForSeconds(amountOfTimeBeforeRestoringHp);

        while (curHp < maxHp)
        {
            yield return new WaitForSeconds(amountOfTimeForRestoringHp);

            RestoreHP(amountOfRestoringHp);
        }

        restoreHpCor = null;
    }
    void SetHPText() => statsImages[0].fillAmount = curHp / maxHp;//set stats from 0 to 1

    public void RestoreManaWithItem(float val)
    {
        if (val > 0) UIInGame.I.InstantiateTextOnManaRestore(transform.position, (int)val);
        RestoreMana(val);
    }
    public void RestoreMana(float val) => ChangeMana(val);
    public void DecreaseMana(float val)
    {
        ChangeMana(val);

        if (restoreManaCor != null) StopCoroutine(restoreManaCor);
        restoreManaCor = StartCoroutine(RestoreManaCor());
    }
    public void ChangeMana(float val)
    {
        curMana = ChangeStat(val, curMana, maxMana);
        statsImages[1].fillAmount = curMana / maxMana;
    }
    IEnumerator RestoreManaCor()
    {
        yield return new WaitForSeconds(amountOfTimeBeforeRestoringMana);

        while (curMana < maxMana)
        {
            yield return new WaitForSeconds(amountOfTimeForRestoringMana);

            RestoreMana(amountOfRestoringMana);
        }

        restoreManaCor = null;
    }

    //on enemies trigger
    public void EnemyTriggerEnter()
    {
        curAmountOfEnemies++;

        if (curAmountOfEnemies == 1) SpeedMultiplierOnDamage = speedOnDamage;
    }
    public void EnemyTriggerExit()
    {
        curAmountOfEnemies--;

        if (curAmountOfEnemies == 0) SpeedMultiplierOnDamage = 1;
    }

    //events
    void UpdateHealthBar() => SetHPText();

    //upgrades
    public void IncreaseMaxHp() => MaxHp *= 1.1f;
    public void AddShield() => shieldProtection += 0.1f;
    public void IncreaseMagnet() => pickingTrigger.radius *= 1.25f;
    public void StartRestoringHp()
    {
        canRestoreHp = true;
        restoreHpCor = StartCoroutine(RestoreHPCor());
    }
    public void IncreaseHealthRecovery()
    {
        amountOfTimeBeforeRestoringHp *= 0.9f;
        amountOfRestoringHp *= 1.1f;
    }

    //other
    void ToggleJoystickInput(bool val) => joystickInput = val;
}
