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
    
    public float radiusForCircleEnemy;

    [Header("Level generation")]
    [SerializeField] Transform levelParent;
    [SerializeField] GameObject levelPrefab;

    [Header("Other")]
    [SerializeField] Transform enemyParent;
    [SerializeField] Transform expParent;

    [Header("Player characters")]
    [SerializeField] GameObject[] playerCharacters;
    [SerializeField] Transform playerParent;

    //local
    Transform playerTransform;

    [HideInInspector] public GameObject[] platforms;

    //level generation
    int platformsLength;

    List<Vector2> allPositions = new List<Vector2>();

    //enemy spawn 
    float worldWidth;
    float worldHeight;


    protected override void Awake()
    {
        base.Awake();

        playerTransform = Instantiate(playerCharacters[GameManager.I.characterI], playerParent).GetComponent<Transform>(); 

        worldHeight = Settings.worldHeight;
        worldWidth = Settings.worldWidth;
    }

    void Start()
    {
        //sign Ns for future using 
        platformsLength = platforms.Length;

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
        for (int x = -2; x < 3; x++)
        {
            for (int y = -2; y < 3; y++)
            {
                Vector2 pos = levelTransform.TransformPoint(new Vector2(platformSize.x * x, platformSize.y * y));
                Instantiate(platforms[Random.Range(0, platformsLength)], pos, Quaternion.identity, levelTransform);
            }
        }
    }

    //spawn enemy
    public void SpawnEnemy(GameObject enemyPrefab)
    {
        InstantiateEnemy(enemyPrefab, GetRandomOffsetPos());// spawn enemy around player
    }

    public void SpawnCrowdOfEnemies(GameObject enemyPrefab, int amountOfEnemies)// create many enemies in one place with one direction
    {
        Vector2 pos = GetRandomOffsetPos();
        Vector2 directionToPlayer = ((Vector2)playerTransform.position - pos).normalized;

        for (int i = 0; i < amountOfEnemies; i++)
        {
            pos += GetRandomPos(distanceBetweenCrowdEnemies, distanceBetweenCrowdEnemies);// different pos at one place for crowd

            InstantiateCrowdEnemy(enemyPrefab, pos, directionToPlayer, 7);
        }
    }

    public void SpawnCircleCrowdEnemies(GameObject enemyPrefab, float amountOfEnemies)// create enemies around player
    {
        float deltaTheta = (2f * Mathf.PI) / amountOfEnemies;// perform some smart trigonometry things 
        float theta = 0f;

        for (int i = 0; i < amountOfEnemies; i++)
        {
            float x = radiusForCircleEnemy * Mathf.Cos(theta);
            float y = radiusForCircleEnemy * Mathf.Sin(theta);
            Vector2 pos = new Vector2(x, y);// position around player

            playerTransform.TransformPoint(pos);
            Vector2 direction = ((Vector2)playerTransform.position - pos).normalized;
            InstantiateCrowdEnemy(enemyPrefab, pos, direction, 30);

            theta += deltaTheta;
        }
    }


    GameObject InstantiateEnemy(GameObject enemy, Vector2 pos)
    {
        return Instantiate(enemy, pos, Quaternion.identity, enemyParent);
    }

    void InstantiateCrowdEnemy(GameObject enemy, Vector2 pos, Vector2 direction, float lifeTime)
    {
        EnemyCrowd enemyCrowd = InstantiateEnemy(enemy, pos).GetComponent<EnemyCrowd>();
        enemyCrowd.directionOfMove = direction;
        enemyCrowd.Invoke("Die", lifeTime);// enemy dies after life time
    }

    Vector2 GetRandomOffsetPos()// get random position further than player's screen
    {
        return GetRandomOffsetPos(worldWidth, worldHeight, offestForSpawn);
    }

    //in game
    public Vector2 GetRandomPos(float x, float y)
    {
        return new Vector2(Random.Range(-x, x), Random.Range(-y, y));
    }

    public Vector2 GetRandomOffsetPos(float x, float y, float offset)
    {
        Vector2 result = new Vector2();

        switch (Random.Range(0, 4))
        {
            case 0:
                result = new Vector2(Random.Range(-x, x), Random.Range(y, y + offset));
                break;
            case 1:
                result = new Vector2(Random.Range(x, x + offset), Random.Range(-y, y));
                break;
            case 2:
                result = new Vector2(Random.Range(-x, x), Random.Range(-y - offset, -y));
                break;
            case 3:
                result = new Vector2(Random.Range(-x - offset, -x), Random.Range(-y, y));
                break;
            default: break;
        }

        return playerTransform.TransformPoint(result);//return pos to point of player position
    }

    //other
    public void MagnetExp()
    {
        foreach (Transform transform in expParent)
        {
            ExpFollowingObject expFollowingObject = transform.gameObject.GetComponent<ExpFollowingObject>();
            expFollowingObject.StartFollowingPlayer();
        }
    }
    public void FreezeEnemies()
    {
        foreach (Transform transform in enemyParent)
        {
            FreezeEnemy freezeEnemy = transform.gameObject.GetComponent<FreezeEnemy>();
            freezeEnemy.Freeze();
        }
    }

    public void MoveEnemy(Transform  enemyTransform)//move enemy closer to player when player gets far from enemy
    {
        enemyTransform.position = GetRandomOffsetPos();
    }
}
