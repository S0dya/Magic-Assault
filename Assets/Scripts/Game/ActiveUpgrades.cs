using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUpgrades : SingletonMonobehaviour<ActiveUpgrades>
{
    [SerializeField] EnemiesTrigger enemiesTrigger;
    [SerializeField] Player player;

    //local
    SpellsManager spellsManager;

    //shoot dot spell
    [HideInInspector] public int[] dotSpellsIndex = new int[4] { 3, 1, 2, 0 }; // 0 - nearest. 1 - random enemy. 2 - random position. 3 - direction of movement
    [HideInInspector] public float[] dotSpellsSize = new float[4] { 0.5f, 0.5f, 0.5f, 0.25f};
    [HideInInspector] public float[] dotSpellsTime = new float[4] { 3, 4, 2, 1f };
    [HideInInspector] public float[] dotSpellsAmountOnShoot = new float[4] { 10, 10, 10, 10 };
    [HideInInspector] public float[] dotReloadTime = new float[4] { 0.1f, 0.08f, 0.01f, 0.01f };

    //instantiate circle spell
    int[] circleSpellsIndex = new int[1] { 0 };
    [HideInInspector] public float[] circleSpellsSize = new float[1] { 2 };
    [HideInInspector] public float[] circleSpellsTime = new float[1] { 1 };
    [HideInInspector] public float[] circleSpellsAmountOnShoot = new float[1] { 10 };
    [HideInInspector] public float[] circleReloadTime = new float[1] { 0.1f };

    public float distanceBetweenSpells = 12;

    protected override void Awake()
    {
        base.Awake();

        spellsManager = GameObject.FindGameObjectWithTag("SpellsManager").GetComponent<SpellsManager>();
    }

    void Start() //test
    {
        EnableDotInNearestEnemy(0);
        EnableDotInRandomEnemy(1);
        EnableDotInRandomPosition(2);
        EnableDotInMovementDirection(3);
        EnableCircleInRandomPosition(0);
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
        while (true)
        {
            for (int i = 0; i < dotSpellsAmountOnShoot[0]; i++)
            {
                if (enemiesTrigger.HasEnemiesNearPlayer()) InstantiateSpellToPosition(spellsManager.dotEffects[dotSpellsIndex[0]], enemiesTrigger.GetNearestEnemyPosition(), dotSpellsSize[0]);
                else break;

                yield return new WaitForSeconds(dotReloadTime[0]);
            }

            yield return new WaitForSeconds(dotSpellsTime[0]);
        }
    }

    IEnumerator DotInRandomEnemyCor()//instantiate on position of a random enemy around player
    {
        while (true)
        {
            for (int i = 0; i < dotSpellsAmountOnShoot[1]; i++)
            {
                if (enemiesTrigger.HasEnemiesNearPlayer()) InstantiateSpellToPosition(spellsManager.dotEffects[dotSpellsIndex[1]], enemiesTrigger.GetRandomEnemyPosition(), dotSpellsSize[1]);
                else break;

                yield return new WaitForSeconds(dotReloadTime[1]);
            }

            yield return new WaitForSeconds(dotSpellsTime[1]);
        }
    }

    
    IEnumerator DotInRandomPositionCor()//instantiate in random pos
    {
        while (true)
        {
            for (int i = 0; i < dotSpellsAmountOnShoot[2]; i++)
            {
                InstantiateSpellToPosition(spellsManager.dotEffects[dotSpellsIndex[2]], enemiesTrigger.GetRandomOffsetPos(1), dotSpellsSize[2]);

                yield return new WaitForSeconds(dotReloadTime[2]);
            }

            yield return new WaitForSeconds(dotSpellsTime[2]);
        }
    }
    
    IEnumerator DotInMovementDirectionCor()//instantiate in direction
    {
        while (true)
        {
            for (int i = 0; i < dotSpellsAmountOnShoot[3]; i++)
            {
                //if player isn't moving get last direction of movement. if player is moving but direction of movement is zero shoot last movement direction
                Vector2 direction = (player.joystickInput && player.IsJoystickDirectionNotZero() ? player.directionOfMovement : player.lastJoystickDirection);
                float distance = (float)i / 50f;
                direction += LevelManager.I.GetRandomPos(distance, distance);

                InstantiateSpellToDirection(spellsManager.dotEffects[dotSpellsIndex[3]], direction, dotSpellsSize[3]);

                yield return new WaitForSeconds(dotReloadTime[3]);
            }
            
            yield return new WaitForSeconds(dotSpellsTime[3]);
        }
    }
    
    //circle
    IEnumerator CircleInRandomPositionCor()
    {
        while (true)
        {
            for (int i = 0; i < circleSpellsAmountOnShoot[0]; i++)
            {
                InstantiateSpellAtPosition(spellsManager.circleEffects[circleSpellsIndex[0]], enemiesTrigger.GetRandomOffsetPos(distanceBetweenSpells), circleSpellsSize[0]);

                yield return new WaitForSeconds(circleReloadTime[0]);
            }

            yield return new WaitForSeconds(dotSpellsTime[2]);
        }
    }

    


    //other
    void InstantiateSpellToPosition(GameObject spellObj, Vector2 pos, float size)//dot logic
    {
        Vector2 startPos = transform.position;
        Vector2 direction = (pos - startPos).normalized;
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        spellsManager.InstantiateEffect(spellObj, startPos, size, direction, rotation);
    }

    void InstantiateSpellToDirection(GameObject spellObj, Vector2 direction, float size)
    {
        float rotation = (Mathf.Atan2(direction.y, direction.x) - 1.5f) * Mathf.Rad2Deg;

        spellsManager.InstantiateEffect(spellObj, transform.position, size, direction, rotation);
    }

    void InstantiateSpellAtPosition(GameObject spellObj, Vector2 pos, float size)// circle logic
    {
        spellsManager.InstantiateEffect(spellObj, pos, size);
    }
}
