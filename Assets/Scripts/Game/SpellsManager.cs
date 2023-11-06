using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : SingletonMonobehaviour<SpellsManager>
{
    [Header("Settings")]
    public float distanceForFire;

    [Header("SerializeFields")]
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

        curEffectManaUsage = Settings.startingSpellsManaUsage;
    }


    //Spells
    public void useDot()
    {
        if (PlayerHasEnoughMana(0)) return;

        //get angle from player to draw position
        Vector2 direction = (drawPoints[^1] - (Vector2)playerTransform.position).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;
        
        InstantiateEffect(curEffect[0], playerTransform.position, size, angle, 0);
        UseMana(0, 1);
    }

    public void useCircle(float distance)
    {
        if (PlayerHasEnoughMana(1)) return;

        //get radius of circle
        Vector2 pos = Vector2.Lerp(drawPoints[0], drawPoints[drawPoints.Count / 2], 0.5f);
        InstantiateEffect(curEffect[1], pos, size * distance, 0, 0);
        UseMana(1, 1);
    }

    public void useLine()
    {
        if (PlayerHasEnoughMana(2)) return;

        //find total number of objects that we need spawn 
        float distance = Vector2.Distance(drawPoints[0], drawPoints[^1]);
        float numObjects = Mathf.CeilToInt(distance / distanceForFire);

        //check if players mana allows to spawn as many objects as needed
        float manaNeeded = Mathf.Min(player.curStats[1], numObjects * curEffectManaUsage[2]);
        float totalN = manaNeeded / curEffectManaUsage[2];

        //instantiate effect from first draw position to last 
        for (int i = 0; i < totalN; i++)
        {
            InstantiateEffect(curEffect[2], Vector2.Lerp(drawPoints[0], drawPoints[^1], i / numObjects), size, 0, 0);
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

        InstantiateEffect(curEffect[3], middleElementPos, size, 0, rotation);

        if (Settings.additionalEffects)
        {
            InstantiateEffect(curEffect[3], drawPoints[0], size, 0, rotation);
            InstantiateEffect(curEffect[3], drawPoints[^1], size, 0, rotation);
        }

        UseMana(3, 1);
    }
    void InstantiateEffect(GameObject prefab, Vector2 pos, float size, float angle, float rotation)
    {
        //instantiate object and get spells script
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity, effectsParent);
        Spell spell = obj.GetComponent<Spell>();

        //set all needed variables of isntantiated object
        spell.SetSize(size);
        if (angle != 0) spell.SetAngle(angle);
        if (rotation != 0) spell.SetRotation(rotation);

        spell.Play();
    }

    //check if player has enough mana to do spells
    bool PlayerHasEnoughMana(int i)
    {
        return player.curStats[1] <= curEffectManaUsage[i];
    }
    //subtract mana
    void UseMana(int i, int amount) => player.ChangeMana(-curEffectManaUsage[i] * size * amount);

}
