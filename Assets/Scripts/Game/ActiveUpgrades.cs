using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUpgrades : SingletonMonobehaviour<ActiveUpgrades>
{
    [SerializeField] EnemiesTrigger enemiesTrigger;
    [SerializeField] Player player;

    //shoot 
    [Header("Spells")]
    [SerializeField] GameObject[] dotSpells;
    [SerializeField] GameObject[] circleSpells;

    //local
    //shoot dot spell
    int[] dotSpellsIndex = new int[4] { 3, 1, 2, 0 }; // 0 - nearest. 1 - random enemy. 2 - random position. 3 - direction of movement
    float[] dotSpellsSize = new float[4] { 0.5f, 0.5f, 0.5f, 0.25f};
    float[] dotSpellsTime = new float[4] { 3, 4, 2, 1f };

    //instantiate circle spell
    int[] circleSpellsIndex = new int[1] { 0 };
    float[] circleSpellsSize = new float[1] { 2 };
    float[] circleSpellsTime = new float[1] { 1 };

    protected override void Awake()
    {
        base.Awake();

        
    }

    void Start() //test
    {
        //EnableDotInNearestEnemy(0);
        //EnableDotInRandomEnemy();
        //EnableDotInRandomPosition();
        //EnableDotInMovementDirection(0);
        EnableCircleInRandomPosition(1);
    }

    //enabling active upgrades
    //dot 
    public void EnableDotInNearestEnemy(int indexOfSpell)
    {
        dotSpellsIndex[0] = indexOfSpell;
        StartCoroutine(DotInNearestEnemyCor());
    }
    public void EnableDotInRandomEnemy(int indexOfSpell)
    {
        dotSpellsIndex[1] = indexOfSpell;
        StartCoroutine(DotInRandomEnemyCor());
    }
    public void EnableDotInRandomPosition(int indexOfSpell)
    {
        dotSpellsIndex[2] = indexOfSpell;
        StartCoroutine(DotInRandomPositionCor());
    }
    public void EnableDotInMovementDirection(int indexOfSpell)
    {
        dotSpellsIndex[3] = indexOfSpell;
        StartCoroutine(DotInMovementDirectionCor());
    }

    //circle
    public void EnableCircleInRandomPosition(int indexOfSpell)
    {
        circleSpellsIndex[0] = indexOfSpell;
        StartCoroutine(CircleInRandomPositionCor());
    }

    // main coroutines
    //dot
    IEnumerator DotInNearestEnemyCor()// instantiate if there are enemies around player
    {
        GameObject spellObj = dotSpells[dotSpellsIndex[0]];

        while (true)
        {
            if (enemiesTrigger.HasEnemiesNearPlayer()) InstantiateSpellToPosition(spellObj, enemiesTrigger.GetNearestEnemyPosition(), dotSpellsSize[0]);

            yield return new WaitForSeconds(dotSpellsTime[0]);
        }
    }

    IEnumerator DotInRandomEnemyCor()//instantiate on position of a random enemy around player
    {
        GameObject spellObj = dotSpells[dotSpellsIndex[1]];

        while (true)
        {
            if (enemiesTrigger.HasEnemiesNearPlayer()) InstantiateSpellToPosition(spellObj, enemiesTrigger.GetRandomEnemyPosition(), dotSpellsSize[1]);

            yield return new WaitForSeconds(dotSpellsTime[1]);
        }
    }

    
    IEnumerator DotInRandomPositionCor()//instantiate in random pos
    {
        GameObject spellObj = dotSpells[dotSpellsIndex[2]];

        while (true)
        {
            InstantiateSpellToPosition(spellObj, enemiesTrigger.GetRandomPosition(1), dotSpellsSize[2]);

            yield return new WaitForSeconds(dotSpellsTime[2]);
        }
    }
    
    IEnumerator DotInMovementDirectionCor()//instantiate in direction
    {
        GameObject spellObj = dotSpells[dotSpellsIndex[3]];

        while (true)
        {
            //if player isn't moving get last direction of movement. if player is moving but direction of movement is zero shoot last movement direction
            Vector2 direction = (player.joystickInput && player.IsJoystickDirectionNotZero() ? player.directionOfMovement : player.lastJoystickDirection);

            InstantiateSpellToDirection(spellObj, direction, dotSpellsSize[3]);

            yield return new WaitForSeconds(dotSpellsTime[3]);
        }
    }

    //circle
    IEnumerator CircleInRandomPositionCor()
    {
        GameObject spellObj = circleSpells[circleSpellsIndex[0]];
        float distance = Settings.worldWidth;

        while (true)
        {
            InstantiateSpellAtPosition(spellObj, enemiesTrigger.GetRandomPosition(distance), circleSpellsSize[0]);

            yield return new WaitForSeconds(dotSpellsTime[2]);
        }
    }

    


    //other
    void InstantiateSpellToPosition(GameObject spellObj, Vector2 pos, float size)//dot logic
    {
        Vector2 startPos = transform.position;
        Vector2 direction = (pos - startPos).normalized;
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        SpellsManager.I.InstantiateEffect(spellObj, startPos, size, direction, rotation);
    }

    void InstantiateSpellToDirection(GameObject spellObj, Vector2 direction, float size)
    {
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        SpellsManager.I.InstantiateEffect(spellObj, transform.position, size, direction, rotation);
    }

    void InstantiateSpellAtPosition(GameObject spellObj, Vector2 pos, float size)// circle logic
    {
        SpellsManager.I.InstantiateEffect(spellObj, pos, size);
    }
}
