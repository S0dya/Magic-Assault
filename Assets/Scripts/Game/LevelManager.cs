using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonobehaviour<LevelManager>
{
    [Header("Settings")]
    public Vector2 levelSize;
    public Vector2 platformSize;

    [Header("Other")]
    [SerializeField] Transform levelParent;
    [SerializeField] GameObject levelPrefab;
    [SerializeField] GameObject[] platforms;

    //local
    int platformsLength;

    List<Vector2> allPositions = new List<Vector2>();

    protected override void Awake()
    {
        base.Awake();


    }

    void Start()
    {
        platformsLength = platforms.Length;

        GenerateLevel(Vector2.zero);
    }

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
}
