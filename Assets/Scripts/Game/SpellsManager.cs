using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : SingletonMonobehaviour<SpellsManager>
{
    [Header("Settings")]
    public float distanceForFire;

    [Header("Other")]
    [SerializeField] Player player;
    Transform playerTransform;
    [SerializeField] DrawManager drawManager;

    [SerializeField] Transform effectsParent;

    //dot
    [SerializeField] GameObject[] dotEffect;
    
    //circle
    [SerializeField] GameObject[] circleEffect;
    
    //line
    [SerializeField] GameObject[] lineEffect;
    
    //arrow
    [SerializeField] GameObject[] arrowEffect;

    
    //local
    [HideInInspector] public float size;
    [HideInInspector] public List<Vector2> drawPoints;

    GameObject[] curEffect = new GameObject[4];// 1 - dot. 2 - circle. 3 - line. 4 - arrow
    float[] curEffectManaUsage = new float[4];
    float[] curSpellsDamage = new float[4];

    protected override void Awake()
    {
        base.Awake();

        playerTransform = player.transform;
    }

    void Start()
    {
        //set values of current spells and mana usage
        curEffect[0] = dotEffect[Settings.startingSpells[0]];
        curEffect[1] = circleEffect[Settings.startingSpells[1]];
        curEffect[2] = lineEffect[Settings.startingSpells[2]];
        curEffect[3] = arrowEffect[Settings.startingSpells[3]];

        curSpellsDamage = Settings.startingSpellsDamage;
        curEffectManaUsage = Settings.startingSpellsManaUsage;
    }


    //Spells
    public void useDot()
    {
        if (PlayerHasEnoughMana(0)) return;

        //get angle from player to draw position
        Vector2 direction = (drawPoints[^1] - (Vector2)playerTransform.position).normalized;
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        InstantiateEffect(curEffect[0], (Vector2)playerTransform.position + direction, size, curSpellsDamage[0], direction, rotation);
        UseMana(0, 1);
    }

    public void useCircle(float distance)
    {
        if (PlayerHasEnoughMana(1)) return;

        //get radius of circle
        Vector2 pos = Vector2.Lerp(drawPoints[0], drawPoints[drawPoints.Count / 2], 0.5f);
        InstantiateEffect(curEffect[1], pos, size * distance, curSpellsDamage[1]);
        UseMana(1, 1);
    }

    public void useLine()
    {
        if (PlayerHasEnoughMana(2)) return;

        //find total number of objects that we need spawn 
        float distance = Vector2.Distance(drawPoints[0], drawPoints[^1]);
        float numObjects = Mathf.CeilToInt(distance / distanceForFire);

        //check if players mana allows to spawn as many objects as needed
        float manaNeeded = Mathf.Min(player.curMana, numObjects * curEffectManaUsage[2]);
        float totalN = manaNeeded / curEffectManaUsage[2];

        //instantiate effect from first draw position to last 
        for (int i = 0; i < totalN; i++)
        {
            InstantiateEffect(curEffect[2], Vector2.Lerp(drawPoints[0], drawPoints[^1], i / numObjects), size, curSpellsDamage[2]);
        }

        UseMana(2, (int)manaNeeded);
    }

    public void useArrow(Vector2 middleElementPos)
    {
        if (PlayerHasEnoughMana(3)) return;

        //get angle from first and last dots to dot with highest angle
        Vector2 arrowStartPos = Vector2.Lerp(drawPoints[0], drawPoints[^1], 0.5f);
        Vector2 direction = (middleElementPos - arrowStartPos).normalized;
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        Vector2[] posOfSpells = new Vector2[] { drawPoints[0], middleElementPos, drawPoints[^1] };

        for (int i = 0; i < (Settings.additionalEffects ? 3 : 1); i++)
        {
            InstantiateEffect(curEffect[3], posOfSpells[i], size, curSpellsDamage[3], rotation);
            UseMana(3, 1);
            if (PlayerHasEnoughMana(3)) break;
        }
    }
    Spell InstantiateEffect(GameObject prefab, Vector2 pos, float size, float damage)
    {
        //instantiate object and get spells script
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity, effectsParent);
        Spell spell = obj.GetComponent<Spell>();

        //set all needed variables of isntantiated object
        spell.SetSize(size);
        spell.damage = damage;
        spell.Play();

        return spell;
    }
    //additional logics for instantiating
    //instantiate with settings direction of force for rigidbody
    public void InstantiateEffect(GameObject prefab, Vector2 pos, float size, float damage, Vector2 direction, float rotation)
    {
        Spell spell = InstantiateEffect(prefab, pos, size, damage);
        spell.ApplyForce(direction/2);
        spell.SetRotation(rotation);
    }
    //instantiate with setting rotation 
    void InstantiateEffect(GameObject prefab, Vector2 pos, float size, float damage, float rotation)
    {
        Spell spell = InstantiateEffect(prefab, pos, size, damage);
        spell.SetRotation(rotation);
    }

    //check if player has enough mana to do spells
    bool PlayerHasEnoughMana(int i)
    {
        return player.curMana <= curEffectManaUsage[i];
    }
    //subtract mana
    void UseMana(int i, int amount) => player.ChangeMana(-curEffectManaUsage[i] * size * amount);

}
