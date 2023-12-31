using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsManager : SingletonMonobehaviour<SpellsManager>
{
    [Header("Other scripts")]
    [SerializeField] DrawManager drawManager;
    [SerializeField] LevelManager levelManager;

    [SerializeField] Transform effectsParent;

    public Spells[] spells= new Spells[4];
    
    [Header("Improved spells")]
    public Spells[] improvedSpells = new Spells[4];

    [Header("Additional effects")]
    [SerializeField] GameObject[] fadeEffects;

    //local
    Player player;
    Transform playerTransform;
    GameData gameData;

    System.Action<Vector2> handleArrow;

    float size;
    Vector2[] drawPoints;

    [HideInInspector] public int[] curTypeOfSpell;// 0 - dot 1 - circle 2 - line 3 - arrow
    public float[] effectManaUsage = new float[4] { 15f, 25f, 3f, 10f };

    int[] amountOfSpells = new int[4] { 1, 1, 1, 1 };
    int arrowsAmount = 1; //3 - max

    //spells threshold
    GameObject[] effectsThreshold = new GameObject[4];


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerTransform = player.transform;
        gameData = GameData.I;

        //set values of current spells and mana usage
        curTypeOfSpell = Settings.startingSpells;
    }
    //Spells
    public void UseDot()
    {
        if (HasMana(0)) StartCoroutine(UseDotCor());
    }
    IEnumerator UseDotCor()
    {
        //get angle from player to draw position
        Vector2 direction = (drawPoints[^1] - (Vector2)playerTransform.position).normalized;
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;
        effectsThreshold[0] = spells[0].spells[curTypeOfSpell[0]];

        UseMana(0, 1);

        float distance = 0;
        for (int i = 0; i < amountOfSpells[0]; i++)
        {
            distance = (float)i * 0.02f;
            direction += levelManager.GetRandomPos(distance, distance);

            InstantiateEffect(effectsThreshold[0], (Vector2)playerTransform.position + direction, size, direction, rotation);

            size *= 0.95f;
            yield return new WaitForSeconds(0.1f * gameData.cooldown);
        }
    }

    public void UseCircle(float distanceOfCircle)
    {
        if (HasMana(1)) StartCoroutine(UseCircleCor(distanceOfCircle));
    }
    IEnumerator UseCircleCor(float distanceOfCircle)
    {
        //get radius of circle
        Vector2 pos = Vector2.Lerp(drawPoints[0], drawPoints[drawPoints.Length / 2], 0.5f);
        effectsThreshold[1] = spells[1].spells[curTypeOfSpell[1]];
        size *= distanceOfCircle;

        UseMana(1, 1);

        float distance = 0;
        for (int i = 0; i < amountOfSpells[1]; i++)
        {
            distance = (float)i * 0.5f;
            pos += levelManager.GetRandomPos(distance, distance);

            InstantiateEffect(effectsThreshold[1], pos, size);

            size *= 0.85f;
            yield return new WaitForSeconds(1f * gameData.cooldown);
        }
    }

    public void UseLine()
    {
        if (HasMana(2)) StartCoroutine(UseLineCor());
    }
    IEnumerator UseLineCor()
    {
        //find total number of objects that needs to be spawn 
        float distance = Vector2.Distance(drawPoints[0], drawPoints[^1]);
        float numObjects = Mathf.CeilToInt(distance / size);

        //check if players mana allows to spawn as many objects as needed
        float manaNeeded = Mathf.Min(player.curMana, numObjects * effectManaUsage[2]);
        float totalN = manaNeeded / effectManaUsage[2];
        effectsThreshold[2] = spells[2].spells[curTypeOfSpell[2]];

        UseMana(2, (int)manaNeeded);

        for (int i = 0; i < amountOfSpells[2]; i++)
        {
            //instantiate effect from first draw position to last 
            for (int j = 0; j < totalN; j++)
            {
                InstantiateEffect(effectsThreshold[2], Vector2.Lerp(drawPoints[0], drawPoints[^1], j / numObjects), size);
            }

            size *= 0.95f;
            yield return new WaitForSeconds(2f * gameData.cooldown);
        }
    }

    public void UseArrow(Vector2 middleElementPos)
    {
        if (HasMana(3)) StartCoroutine(UseArrowCor(middleElementPos));
    }
    IEnumerator UseArrowCor(Vector2 middleElementPos)
    {
        Vector2 arrowStartPos = Vector2.Lerp(drawPoints[0], drawPoints[^1], 0.5f);
        Vector2 direction = (middleElementPos - arrowStartPos).normalized;
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        Vector2[] posOfSpells = new Vector2[] { middleElementPos, drawPoints[0],  drawPoints[^1] };
        effectsThreshold[3] = spells[3].spells[curTypeOfSpell[3]];

        bool hasMana = true;
        for (int i = 0; i < amountOfSpells[3]; i++)
        {
            for (int j = 0; j < arrowsAmount; j++)
            {
                InstantiateEffect(effectsThreshold[3], posOfSpells[j], size, direction, rotation);
                UseMana(3, 1);
                hasMana = HasMana(3);
                if (!hasMana) yield break;
            }

            if (!hasMana) yield break;
            size *= 0.95f;
            yield return new WaitForSeconds(1.2f * gameData.cooldown);
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

    //set draw points and size 
    public void SetDrawPointsAndSize(Vector2[] points, float spellSize)
    {
        drawPoints = points;
        size = spellSize;
    }

    //check if player has enough mana to use spells
    bool HasMana(int i)
    {
        return player.curMana > effectManaUsage[i];
    }
    //subtract mana
    void UseMana(int i, int amount) => player.DecreaseMana(-effectManaUsage[i] * size * amount);
    
    //passive upgrades logic
    public void IncreaseAmountOfSpells()
    {
        for (int i = 0; i < 4; i++) IncreaseAmountOfSpells(i);
    }
    public void IncreaseAmountOfSpells(int i) => amountOfSpells[i]++;

    public void IncreaseArrowAmount() => arrowsAmount++;

    //Improve first 3 spells (air cant be improved by this logic)
    public void ImproveSpells(int typeOfDamage)
    {
        //set general spells 
        for (int i = 0; i < 3; i++) spells[i].spells[typeOfDamage] = improvedSpells[i].spells[typeOfDamage];

        switch (typeOfDamage)
        {
            case 1:
                gameData.isWaterPoisened = true;
                LevelManager.I.SetWaterDealsDamageForEnemies();
                break;
            case 2:
                spells[3].spells[typeOfDamage] = improvedSpells[3].spells[typeOfDamage]; //set arrow for earth
                break;
            default: break;
        }
    }
}
