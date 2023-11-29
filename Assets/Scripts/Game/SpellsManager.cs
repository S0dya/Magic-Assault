using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : SingletonMonobehaviour<SpellsManager>
{
    [Header("Serialize fields")]
    [SerializeField] Player player;
    Transform playerTransform;
    [SerializeField] DrawManager drawManager;

    [SerializeField] Transform effectsParent;

    //dot
    [SerializeField] GameObject[] dotEffects;
    
    //circle
    [SerializeField] GameObject[] circleEffects;

    //line
    [SerializeField] GameObject[] lineEffects;

    //arrow
    [SerializeField] GameObject[] arrowEffects;

    [Header("Improved spells")]
    [SerializeField] GameObject[] improvedDotEffects;
    [SerializeField] GameObject[] improvedCircleEffects;
    [SerializeField] GameObject[] improvedLineEffects;
    [SerializeField] GameObject[] improvedArrowEffects;

    [Header("Additional effects")]
    [SerializeField] GameObject[] fadeEffects;

    //local
    System.Action<Vector2> handleArrow;

    [HideInInspector] public float size;
    [HideInInspector] public List<Vector2> drawPoints;


    [HideInInspector] public int[] curTypeOfSpell;// 0 - dot 1 - circle 2 - line 3 - arrow
    float[] curEffectManaUsage = new float[4];

    protected override void Awake()
    {
        base.Awake();

        playerTransform = player.transform;

        //set values of current spells and mana usage
        curTypeOfSpell = Settings.startingSpells;
    }

    void Start()
    {
        curEffectManaUsage = Settings.startingSpellsManaUsage;
    }

    //Spells
    public void useDot()
    {
        if (PlayerHasEnoughMana(0)) return;

        //get angle from player to draw position
        Vector2 direction = (drawPoints[^1] - (Vector2)playerTransform.position).normalized;
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        InstantiateEffect(dotEffects[curTypeOfSpell[0]], (Vector2)playerTransform.position + direction, size, direction, rotation);
        UseMana(0, 1);
    }

    public void useCircle(float distance)
    {
        if (PlayerHasEnoughMana(1)) return;

        //get radius of circle
        Vector2 pos = Vector2.Lerp(drawPoints[0], drawPoints[drawPoints.Count / 2], 0.5f);
        InstantiateEffect(circleEffects[curTypeOfSpell[1]], pos, size * distance);
        UseMana(1, 1);
    }

    public void useLine()
    {
        if (PlayerHasEnoughMana(2)) return;

        //find total number of objects that needs to be spawn 
        float distance = Vector2.Distance(drawPoints[0], drawPoints[^1]);
        float numObjects = Mathf.CeilToInt(distance / size);

        //check if players mana allows to spawn as many objects as needed
        float manaNeeded = Mathf.Min(player.curMana, numObjects * curEffectManaUsage[2]);
        float totalN = manaNeeded / curEffectManaUsage[2];

        //instantiate effect from first draw position to last 
        for (int i = 0; i < totalN; i++)
        {
            InstantiateEffect(lineEffects[curTypeOfSpell[2]], Vector2.Lerp(drawPoints[0], drawPoints[^1], i / numObjects), size);
        }

        UseMana(2, (int)manaNeeded);
    }

    public void useArrow(Vector2 middleElementPos)
    {
        if (PlayerHasEnoughMana(3)) return;

        Vector2 arrowStartPos = Vector2.Lerp(drawPoints[0], drawPoints[^1], 0.5f);
        Vector2 direction = (middleElementPos - arrowStartPos).normalized;
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        Vector2[] posOfSpells = new Vector2[] { drawPoints[0], middleElementPos, drawPoints[^1] };

        for (int i = 0; i < (Settings.additionalEffectsOfArrow ? 3 : 1); i++)
        {
            InstantiateEffect(arrowEffects[curTypeOfSpell[3]], posOfSpells[i], size, direction, rotation);
            UseMana(3, 1);
            if (PlayerHasEnoughMana(3)) break;
        }
    }

    public Spell InstantiateEffect(GameObject prefab, Vector2 pos, float size)
    {
        //instantiate object and get spells script
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity, effectsParent);
        Spell spell = obj.GetComponent<Spell>();

        //set all needed variables of isntantiated object
        spell.SetSize(size);
        spell.Play();

        return spell;
    }
    //additional logics for instantiating
    //instantiate with settings direction of force for rigidbody
    public void InstantiateEffect(GameObject prefab, Vector2 pos, float size, Vector2 direction, float rotation)
    {
        Spell spell = InstantiateEffect(prefab, pos, size);
        spell.ApplyForce(direction.normalized);
        spell.SetRotation(rotation);
    }
    //instantiate with setting rotation 
    void InstantiateEffect(GameObject prefab, Vector2 pos, float size, float rotation)
    {
        Spell spell = InstantiateEffect(prefab, pos, size);
        spell.SetRotation(rotation);
    }

    //instantiate fade effect to visualise destroying of spell
    public ParticleSystem InstantiateFadeEffect(Vector2 pos, int i)
    {
        GameObject obj = Instantiate(fadeEffects[i], pos, Quaternion.identity, effectsParent);

        return obj.GetComponent<ParticleSystem>();
    }

    //check if player has enough mana to do spells
    bool PlayerHasEnoughMana(int i)
    {
        return player.curMana <= curEffectManaUsage[i];
    }
    //subtract mana
    void UseMana(int i, int amount) => player.ChangeMana(-curEffectManaUsage[i] * size * amount);

    //Improve first 3 spells (air cant be improved by this logic)
    public void ImproveSpells(int typeOfDamage)
    {
        //set general spells 
        dotEffects[typeOfDamage] = improvedDotEffects[typeOfDamage];
        circleEffects[typeOfDamage] = improvedCircleEffects[typeOfDamage];
        lineEffects[typeOfDamage] = improvedLineEffects[typeOfDamage];

        if (typeOfDamage == 2) arrowEffects[typeOfDamage]= improvedArrowEffects[typeOfDamage]; //set arrow for earth
    }
}
