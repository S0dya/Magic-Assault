using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveUpgrades : SingletonMonobehaviour<ActiveUpgrades>
{
    [Header("Other scripts")]
    [SerializeField] Player player;

    [Header("Spells")]
    [SerializeField] Spells[] spells;

    [Header("Upgrades")]
    [SerializeField] ActiveUpgrade[] upgrades;

    //local
    SpellsManager spellsManager;
    LevelManager levelManager;

    //other
    float distanceBetweenSpells = 10;

    int upgradesN;

    //enemies search
    List<Transform> enemiesTransforms = new List<Transform>();
    Vector2 nearestPos;

    //cors
    Coroutine[] upgradesCors;

    protected override void Awake()
    {
        base.Awake();

        spellsManager = GameObject.FindGameObjectWithTag("SpellsManager").GetComponent<SpellsManager>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        upgradesN = upgrades.Length;
        upgradesCors = new Coroutine[upgradesN];
        for (int i = 0; i < upgradesN; i++) upgrades[i].upgradeIndex = i;
    }

    //main methods
    public void PerformActiveUpgrade(UpgradeType upgradeType, int typeOfDamage)
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.upgradeType == upgradeType)
            {
                upgrade.typeOfDaamge = typeOfDamage;
                SetActiveUpgrade(upgrade);

                break;
            }
        }
    }

    public void SetActiveUpgrade(ActiveUpgrade upgrade)
    {
        int i = upgrade.upgradeIndex;
        Debug.Log(i);

        if (upgradesCors[i] != null) StopCoroutine(upgradesCors[i]);

        upgradesCors[i] = StartCoroutine(UpgradeCor(spells[upgrade.spellI].spells[upgrade.typeOfDaamge], 
            upgrade.amountOfSpells, upgrade.reloadTime, upgrade.timeBeforeNextSpells, upgrade.size, upgrade.upgradeEvent));
    }

    IEnumerator UpgradeCor(GameObject effectPrefab, int amountOfSpells, float reloadTime, float timeBeforeNextSpells, float size, UpgradeEvent action)
    {
        while (true)
        {
            for (int i = 0; i < amountOfSpells; i++)
            {
                action.Invoke(effectPrefab, i, size);

                yield return new WaitForSeconds(reloadTime);
            }

            yield return new WaitForSeconds(timeBeforeNextSpells);
        }
    }

    //outside methods for upgrading
    void ResetUpgrade(int i)
    {
        if (upgradesCors[i] != null) SetActiveUpgrade(upgrades[i]);
    }

    public void IncreaseAmount()
    {
        for (int i = 0; i < upgradesN; i++) IncreaseAmount(i);
    }
    public void IncreaseAmount(UpgradeType upgradeType)
    {
        for (int i = 0; i < upgradesN; i++)
        {
            if (upgrades[i].upgradeType == upgradeType)
            {
                IncreaseAmount(i);

                break;
            }
        }
    }
    public void IncreaseAmount(int i)
    {
        upgrades[i].amountOfSpells++;
        ResetUpgrade(i);
    }


    //dot active upgrades
    public void DotInNearestEnemy(GameObject effectPrefab, int i, float size)
    {
        InstantiateSpellToPosition(effectPrefab, GetNearestEnemyPosition(), size);
    }

    public void DotInRandomEnemy(GameObject effectPrefab, int i, float size)
    {
        InstantiateSpellToPosition(effectPrefab, GetRandomEnemyPosition(), size);
    }
    
    public void DotInRandomPosition(GameObject effectPrefab, int i, float size)
    {
        InstantiateSpellToPosition(effectPrefab, GetRandomDistancePos(1), size);
    }

    public void DonInMovementDirection(GameObject effectPrefab, int i, float size)
    {
        //if player isn't moving get last direction of movement. if player is moving but direction of movement is zero shoot last movement direction
        Vector2 direction = (player.joystickInput && player.IsJoystickDirectionNotZero() ? player.directionOfMovement : player.lastJoystickDirection);
        float distance = (float)i * 0.02f;
        direction += levelManager.GetRandomPos(distance, distance);

        InstantiateSpellToDirection(effectPrefab, direction, size);
    }

    //circle active upgrades
    public void CircleInRandomPosition(GameObject effectPrefab, int i, float size)
    {
        InstantiateSpellAtPosition(effectPrefab, GetRandomDistancePos(distanceBetweenSpells), size);
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

    Vector2 GetRandomDistancePos(float distance)
    {
        return transform.TransformPoint(levelManager.GetRandomPos(distance, distance));
    }

    //methods for finding enmies
    public Vector2 GetNearestEnemyPosition()
    {
        //set private values
        float nearestDistance = float.MaxValue;

        //check nearest transform based on distance
        foreach (Transform enemyTransform in enemiesTransforms)
        {
            Vector2 curPos = enemyTransform.position;
            float curDistance = Vector2.Distance(curPos, transform.position);
            
            if (nearestDistance > curDistance)
            {
                nearestDistance = curDistance;
                nearestPos = curPos;
            }
        }

        return nearestPos;
    }

    public Vector2 GetRandomEnemyPosition()
    {
        //get random enemy near player
        return enemiesTransforms.Count != 0 ? enemiesTransforms[Random.Range(0, enemiesTransforms.Count)].position : levelManager.GetRandomPos(1, 1);
    }

    //other
    public void AddEnemy(Transform enemyTransform) => enemiesTransforms.Add(enemyTransform);
    public void RemoveEnemy(Transform enemyTransform) => enemiesTransforms.Remove(enemyTransform);
}


[System.Serializable]
public class ActiveUpgrade
{
    [Header("Upgrade info")]
    [SerializeField] public UpgradeType upgradeType;
    [SerializeField] public UpgradeEvent upgradeEvent;

    [Header("Settings")]
    [SerializeField] public int amountOfSpells;

    [SerializeField] public float reloadTime;
    [SerializeField] public float timeBeforeNextSpells;
    
    [SerializeField] public float size;
    [SerializeField] public int spellI;

    //Other
    [HideInInspector] public int typeOfDaamge;

    [HideInInspector] public int upgradeIndex { get; set; }

}

[System.Serializable]
public class UpgradeEvent : UnityEvent<GameObject, int, float>{}