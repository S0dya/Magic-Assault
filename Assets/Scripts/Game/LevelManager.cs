using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonobehaviour<LevelManager>
{
    [Header("Settings")]
    public Vector2 levelSize;
    public Vector2 platformSize;

    public float offestForSpawn;

    [Header("Level generation")]
    [SerializeField] Transform levelParent;
    [SerializeField] GameObject levelPrefab;
    [SerializeField] GameObject[] platforms;

    [Header("Other")]
    [SerializeField] Transform enemyParent;

    //local
    Transform playerTransform;

    //level generation
    int platformsLength;

    List<Vector2> allPositions = new List<Vector2>();

    //enemy spawn 
    float worldWidth;
    float worldHeight;

    protected override void Awake()
    {
        base.Awake();

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        worldHeight = Camera.main.orthographicSize * 4.5f;
        worldWidth = Settings.width / Settings.height * worldHeight;
    }

    void Start()
    {
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

    //enemy
    public void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, GetRandomPos(), Quaternion.identity, enemyParent);
    }

    public void SpawnCrowdOfEnemies(GameObject enemyPrefab, int amountOfEnemies)
    {
        Vector2 pos = GetRandomPos();
        Vector2 directionToPlayer = ((Vector2)playerTransform.position - pos).normalized;

        for (int i = 0; i < amountOfEnemies; i++)
        {
            EnemyCrowd enemyCrowd = Instantiate(enemyPrefab, pos, Quaternion.identity, enemyParent).GetComponent<EnemyCrowd>();
            enemyCrowd.directionOfMove = directionToPlayer;
        }
    }

    Vector2 GetRandomPos()
    {
        Vector2 result = new Vector2();

        switch (Random.Range(0, 4))
        {
            case 0:
                result = new Vector2(Random.Range(-worldWidth, worldWidth), Random.Range(worldHeight, worldHeight + offestForSpawn));
                break;
            case 1:
                result = new Vector2(Random.Range(worldWidth, worldWidth + offestForSpawn), Random.Range(-worldHeight, worldHeight));
                break;
            case 2:
                result = new Vector2(Random.Range(-worldWidth, worldWidth), Random.Range(-worldHeight - offestForSpawn, -worldHeight));
                break;
            case 3:
                result = new Vector2(Random.Range(-worldWidth - offestForSpawn, -worldWidth), Random.Range(-worldHeight, worldHeight));
                break;
            default: break;
        }

        return playerTransform.TransformPoint(result);
    }
}
