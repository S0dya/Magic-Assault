using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : SingletonMonobehaviour<Map>
{
    [Header("Other scripts")]
    [SerializeField] LevelManager levelManager;
    
    [Header("platforms")]
    [SerializeField] GameObject[] platforms;

    [Header("Waves")]
    [SerializeField][TextArea(30, 10)] string DescriptionOfWavesOfAMap;
    [SerializeField] MapWave[] mapWaves;

    //local
    GameObject firstElemental;

    //treshhold
    MapWave curMapWave;
    MapWaveEnemies curMapWaveEnemies;
    MapWaveMiniBoss curWaveMiniBoss;

    protected override void Awake()
    {
        base.Awake();

        levelManager.platforms = platforms;
    }

    void Start()
    {
        StartCoroutine(WavesCor());
    }

    IEnumerator WavesCor() // each wave spawn enemies, crowds, circle crowds, mini-bosses and/or bosses
    {
        for (int i = 0; i < 30; i++)//iterate throught each wave, each wave is one minute - iterate each minute
        {
            curMapWave = mapWaves[i];//set current wave 

            //check additioanal conditions of a wave
            if (curMapWave.hasCrowd)
            {
                curMapWaveEnemies = curMapWave.waveCrowd;//set cur wave enemies
                //start coroutine of spawining enemies. assign time, amonut and enemy object for spawn. if there is an elemental type enemy - choose 1 of 4 of them
                StartCoroutine(levelManager.SpawnCrowdsEnemiesCor(curMapWaveEnemies.time, curMapWaveEnemies.amount, (curMapWaveEnemies.setsEnemyOnChoose ? curMapWaveEnemies.enemyChoose[Random.Range(0, 4)] : curMapWaveEnemies.enemies[0])));
            }

            if (curMapWave.hasCircleCrowd)
            {
                curMapWaveEnemies = curMapWave.waveCircleCrowd;
                levelManager.SpawnCircleCrowdEnemies(curMapWaveEnemies.time, curMapWaveEnemies.amount, (curMapWaveEnemies.setsEnemyOnChoose ? curMapWaveEnemies.enemyChoose[Random.Range(0, 4)] : curMapWaveEnemies.enemies[0]));
            }

            if (curMapWave.hasMiniBoss)
            {
                curWaveMiniBoss = curMapWave.waveMiniBoss;
                levelManager.SpawnMiniBoss((curWaveMiniBoss.setsEnemyOnChoose ? curWaveMiniBoss.enemyChoose[Random.Range(0, 4)] : curWaveMiniBoss.enemy), curWaveMiniBoss.sizeMultiplier);
            }

            curMapWaveEnemies = curMapWave.wave;
            if (curMapWaveEnemies.setsEnemyOnChoose) curMapWaveEnemies.enemies[^1] = curMapWaveEnemies.enemyChoose[Random.Range(0, 4)];//add additional elemental enemy (last index of an array must be empty)
            yield return StartCoroutine(levelManager.SpawnEnemiesWaveCor(curMapWaveEnemies.time, curMapWaveEnemies.amount, curMapWaveEnemies.enemies));
        }
    }

    //DELATER
    public void ToggleTime()
    {
        Time.timeScale = (Time.timeScale == 1 ? 5 : 1);
    }
}

[System.Serializable]
public class MapWave
{
    [Header("Main wave")]
    [SerializeField] public MapWaveEnemies wave;

    //additional
    [Header("Crowd wave")]
    [SerializeField] public bool hasCrowd;
    [SerializeField] public MapWaveEnemies waveCrowd;

    [Header("Circle crowd wave")]
    [SerializeField] public bool hasCircleCrowd;
    [SerializeField] public MapWaveEnemies waveCircleCrowd;

    [Header("Mini boss wave")]
    [SerializeField] public bool hasMiniBoss;
    [SerializeField] public MapWaveMiniBoss waveMiniBoss;
}

[System.Serializable]
public class MapWaveEnemies
{
    [SerializeField] public GameObject[] enemies;

    [SerializeField] public float time = 60;
    [SerializeField] public int amount = 60;

    [Header("elemntals logic")]
    [SerializeField] public bool setsEnemyOnChoose;
    [SerializeField] public GameObject[] enemyChoose;
}

[System.Serializable]
public class MapWaveMiniBoss
{
    [SerializeField] public GameObject enemy;

    [SerializeField] public float sizeMultiplier = 2;

    [Header("elemntals logic")]
    [SerializeField] public bool setsEnemyOnChoose;
    [SerializeField] public GameObject[] enemyChoose;
}
