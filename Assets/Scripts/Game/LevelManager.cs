using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonobehaviour<LevelManager>
{
    [Header("Settings")]
    public Vector2 levelSize;
    public Vector2 platformSize;

    public float offestForSpawn;

    public float distanceBetweenCrowdEnemies;

    [Header("Level generation")]
    [SerializeField] Transform levelParent;
    [SerializeField] GameObject levelPrefab;

    [Header("Other")]
    [SerializeField] Transform enemyParent;
    [SerializeField] Transform expParent;
    [SerializeField] Transform coinParent;
    [SerializeField] Transform upgradeParent;

    [Header("Player characters")]
    [SerializeField] GameObject[] playerCharacters;
    [SerializeField] Transform playerParent;

    [Header("Mini boss")]
    [SerializeField] GameObject SpawnOnDestroyEnemyBossObj;

    //local
    Transform playerTransform;

    GameObject[] platforms;
    GameObject[] decor;

    //level generation
    int platformsLength;
    int decorLength;

    List<Vector2> allPositions = new List<Vector2>();

    Vector2[][] platformsPositions;
    Vector2[][] envirenmentPositions;

    //enemy spawn 
    float worldWidth;
    float worldHeight;

    //cor
    Coroutine spawnEnemiesWaveCor;

    protected override void Awake()
    {
        base.Awake();

        playerTransform = Instantiate(playerCharacters[Settings.characterI], playerParent).GetComponent<Transform>();

        worldHeight = Settings.worldHeight;
        worldWidth = Settings.worldWidth;
    }

    void Start()
    {
        //sign Ns for future using 
        platformsLength = platforms.Length;
        decorLength = decor.Length;

        platformsPositions = GetPositionsArray(5, platformSize);
        envirenmentPositions = GetPositionsArray(4, platformSize / 4);

        GenerateLevel(Vector2.zero);
    }

    //level generation
    public void GenerateLevel(Vector2 position)
    {
        if (allPositions.Contains(position)) return; //no need to create level if it already exists 

        allPositions.Add(position);

        //instantiate this level on new position and get all needed components
        GameObject levelObj = Instantiate(levelPrefab, position * levelSize, Quaternion.identity, levelParent);
        Level level = levelObj.GetComponent<Level>();
        level.position = position;

        Transform levelTransform = levelObj.GetComponent<Transform>();

        //instantiate 25 platforms of level randomly in local scale of level's transform
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                //instantiate new platform in level transform and get platforms transform
                Transform platformTransform = Instantiate(platforms[Random.Range(0, platformsLength)], GetPointPos(levelTransform, platformsPositions[i][j]), Quaternion.identity, levelTransform).transform;

                //Instantiate differtent objects
                GenerateEnvirenment(platformTransform);
            }
        }
    }
    void GenerateEnvirenment(Transform platformTransform)
    {
        //instantiate new objects on level 
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (Random.Range(0, 10) == 1)
                    Instantiate(decor[Random.Range(0, decorLength)],
                        GetPointPos(platformTransform, GetRandomPos(envirenmentPositions[i][j], envirenmentPositions[i + 1][j + 1])), Quaternion.identity, platformTransform);
    }

    //level generation other
    Vector2[][] GetPositionsArray(int n, Vector2 sidesLength)// create 2d array of all positions based on length
    {
        Vector2[][] result = new Vector2[n][];
        for (int i = 0; i < n; i++) result[i] = new Vector2[n];

        int start = -n / 2;

        for (int i = 0, x = start; i < n; i++, x++)
            for (int j = 0, y = start; j < n; j++, y++)
                result[i][j] = sidesLength * new Vector2(x, y);

        return result;
    }

    Vector2 GetPointPos(Transform transform, Vector2 pos)
    {
        return transform.TransformPoint(pos);
    }

    public void SetPlatformsAndDecor(GameObject[] platformsObjs, GameObject[] decorObjs)
    {
        platforms = platformsObjs;
        decor = decorObjs;
    }



    //enemy spawn cor
    public void StartSpawningEnemies(float totalTime, int amountOfEnemies, GameObject[] enemies)
    {
        StopLastWave();
        spawnEnemiesWaveCor = StartCoroutine(SpawnEnemiesWaveCor(totalTime, amountOfEnemies, enemies));
    }
    public void StopLastWave()
    {
        if (spawnEnemiesWaveCor != null) StopCoroutine(spawnEnemiesWaveCor);
    }

    public IEnumerator SpawnEnemiesWaveCor(float totalTime, int amountOfEnemies, GameObject[] enemies)
    {
        float time = GetAverageTime(totalTime, (float)amountOfEnemies);
        int n = enemies.Length;

        for (int i = 0; i < amountOfEnemies; i++)
        {
            SpawnEnemy(enemies[Random.Range(0, n)]);

            yield return new WaitForSeconds(time);
        }
    }

    public IEnumerator SpawnCrowdsEnemiesCor(float totalTime, int amountOfEnemies, int amountOfChords, GameObject enemyPrefab)
    {
        float time = GetAverageTime(totalTime, (float)amountOfChords);

        for (int i = 0; i < amountOfChords; i++)
        {
            SpawnCrowdEnemies(amountOfEnemies, enemyPrefab);

            yield return new WaitForSeconds(time);
        }
    }

    //spawn enemy
    public void SpawnEnemy(GameObject enemyPrefab)
    {
        InstantiateObject(enemyPrefab, GetRandomOffsetPos(), enemyParent);// spawn enemy around player
    }

    public void SpawnCrowdEnemies(int amountOfEnemies, GameObject enemyPrefab)// create many enemies in one place with one direction
    {
        Vector2 pos = GetRandomOffsetPos();
        Vector2 directionToPlayer = ((Vector2)playerTransform.position - pos).normalized;

        for (int i = 0; i < amountOfEnemies; i++)
        {
            pos += GetRandomPos(distanceBetweenCrowdEnemies, distanceBetweenCrowdEnemies);// different pos at one place for crowd

            InstantiateCrowdEnemy(enemyPrefab, pos, directionToPlayer, 7);
        }
    }

    public void SpawnCircleCrowdEnemies(float lifeTime, int amountOfEnemies, float radius, GameObject enemyPrefab)// create enemies around player
    {
        float deltaTheta = (2f * Mathf.PI) / (float)amountOfEnemies;// perform some smart trigonometry things 
        float theta = 0f;

        for (int i = 0; i < amountOfEnemies; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector2 pos = new Vector2(x, y);// position around player

            Vector2 direction = ((Vector2)playerTransform.position - pos).normalized;
            pos = playerTransform.TransformPoint(pos);
            InstantiateCrowdEnemy(enemyPrefab, pos, direction, lifeTime);

            theta += deltaTheta;
        }
    }

    public void SpawnMiniBoss(float sizeMultiplier, GameObject enemyPrefab)
    {
        GameObject enemyObj = InstantiateObject(enemyPrefab, GetRandomOffsetPos(), enemyParent);
        float size = enemyObj.transform.localScale.x * sizeMultiplier;
        enemyObj.transform.localScale = new Vector2(size, size);

        var rb = enemyObj.GetComponent<Rigidbody2D>();
        rb.mass *= sizeMultiplier;

        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.maxHp *= sizeMultiplier * 1.5f;

        SpawnOnDestroyEnemy spawnOnDestroyEnemy = enemyObj.GetComponentInChildren<SpawnOnDestroyEnemy>();
        Destroy(spawnOnDestroyEnemy.gameObject);

        var spawnOnDestroy = Instantiate(SpawnOnDestroyEnemyBossObj, enemyObj.transform).GetComponent<SpawnOnDestroyEnemyBoss>();
        spawnOnDestroy.SetEnemy();
    }

    void InstantiateCrowdEnemy(GameObject enemy, Vector2 pos, Vector2 direction, float lifeTime)
    {
        EnemyCrowd enemyCrowd = InstantiateObject(enemy, pos, enemyParent).GetComponent<EnemyCrowd>();
        enemyCrowd.directionOfMove = direction;
        enemyCrowd.Invoke("Die", lifeTime);// enemy dies after life time
    }

    //other of enemy spawn
    float GetAverageTime(float time, float amount)
    {
        //get average time of spawning one enemy
        return time / amount;
    }

    //in game
    public Vector2 GetRandomPos(float x, float y)
    {
        return new Vector2(Random.Range(-x, x), Random.Range(-y, y));
    }
    Vector2 GetRandomPos(Vector2 pos)
    {
        return GetRandomPos(pos.x, pos.y);
    }
    Vector2 GetRandomPos(Vector2 start, Vector2 end)
    {
        return new Vector2(Random.Range(start.x, end.x), Random.Range(start.y, end.y));
    }

    Vector2 GetRandomOffsetPos()// get random position further than player's screen
    {
        return GetRandomOffsetPos(worldWidth, worldHeight, offestForSpawn);
    }
    public Vector2 GetRandomOffsetPos(float x, float y, float offset)
    {
        Vector2 result = new Vector2();

        switch (Random.Range(0, 4))
        {
            case 0: result = new Vector2(Random.Range(-x, x), Random.Range(y, y + offset)); break;
            case 1: result = new Vector2(Random.Range(x, x + offset), Random.Range(-y, y)); break;
            case 2: result = new Vector2(Random.Range(-x, x), Random.Range(-y - offset, -y)); break;
            case 3: result = new Vector2(Random.Range(-x - offset, -x), Random.Range(-y, y)); break;
            default: break;
        }

        return playerTransform.TransformPoint(result);//return pos to point of player position
    }

    //other

    //get exps and magnet it to player
    public void MagnetExps() => LoopThroughTransforms(expParent, MagnetExp);
    void MagnetExp(Transform transform) => transform.gameObject.GetComponent<FollowingObjectExp>().StartFollowingPlayer();

    //get enemy and freeze them
    public void FreezeEnemies() => LoopThroughTransforms(enemyParent, FreezeEnemy);
    void FreezeEnemy(Transform transform) => transform.gameObject.GetComponent<EnemyFreeze>().Freeze();

    //get enemy's transform and kill
    public void KillEnemies() => LoopThroughTransforms(enemyParent, KillEnemy);
    void KillEnemy(Transform transform) => GetEenemy(transform).Kill();

    //same as above, but is used on last time limit reached
    public void WapeOutEnemies()
    {
        LoopThroughTransforms(enemyParent, WapeOutEnemy);
        StopLastWave();
    }
    void WapeOutEnemy(Transform transform) => GetEenemy(transform).Die();

    //on improved water set enemies to recceive damage from water
    public void SetWaterDealsDamageForEnemies() => LoopThroughTransforms(enemyParent, SetWaterDealsDamageForEnemy);
    void SetWaterDealsDamageForEnemy(Transform transform) => GetEenemy(transform).SetWaterDealsDamage();

    //other base
    void LoopThroughTransforms(Transform parent, System.Action<Transform> action)//loop thorught enemies and invoke action
    {
        foreach (Transform transform in parent) action.Invoke(transform);
    }
    Enemy GetEenemy(Transform transform)
    {
        return transform.gameObject.GetComponent<Enemy>();
    }

    //on enemy death
    public void InstantiateExp(GameObject prefab, Vector2 pos) => InstantiateObject(prefab, pos, expParent);
    public void InstantiateCoin(GameObject prefab, Vector2 pos) => InstantiateObject(prefab, pos, coinParent);
    public void InstantiateUpgrade(GameObject prefab, Vector2 pos) => InstantiateObject(prefab, pos, upgradeParent);

    //other
    public void MoveEnemy(Transform enemyTransform) => enemyTransform.position = GetRandomOffsetPos();//move enemy closer to player when player gets far from enemy
 
    GameObject InstantiateObject(GameObject prefab, Vector2 pos, Transform parent)
    {
        return Instantiate(prefab, pos, Quaternion.identity, parent);
    }
}