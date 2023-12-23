using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : SingletonMonobehaviour<Map>
{
    [Header("Other scripts")]
    [SerializeField] LevelManager levelManager;

    [Header("level consistency")]
    [SerializeField] GameObject[] platforms;
    [SerializeField] GameObject[] decor;

    [Header("Waves")]
    [SerializeField][TextArea(30, 10)] string DescriptionOfWavesOfAMap;
    [SerializeField] MapWave[] mapWaves;

    //local
    GameObject firstElemental;

    //treshhold
    MapWave curWave;
    MapWaveEnemies curWaveEnemies;
    MapWaveChordEnemies curWaveChordEnemies;
    MapWaveCircleCrowdWaveEnemies curWaveCircleCrowdEnemies;
    MapWaveMiniBoss curWaveMiniBoss;

    int curWaveIndex = 25;

    protected override void Awake()
    {
        base.Awake();

        levelManager.SetPlatformsAndDecor(platforms, decor);
    }

    void Start()
    {
        Wave();
    }

    /*
    IEnumerator WavesCor() // each wave spawn enemies, crowds, circle crowds, mini-bosses and/or bosses
    {
        for (int i = 0; i < 30; i++)//iterate throught each wave, each wave is one minute - iterate each minute
        {
            curWave = mapWaves[i];//set current wave 

            //check additioanal conditions of a wave
            if (curWave.hasCrowd)
            {
                curWaveChordEnemies = curWave.waveCrowd;//set cur wave enemies
                //start coroutine of spawining enemies. assign time, amonut and enemy object for spawn. if there is an elemental type enemy - choose 1 of 4 of them
                StartCoroutine(levelManager.SpawnCrowdsEnemiesCor(curWaveChordEnemies.time, curWaveChordEnemies.amount, curWaveChordEnemies.AmountOfChords, 
                    (curWaveChordEnemies.setsEnemyOnChoose ? curWaveChordEnemies.enemyChoose.enemiesChoose[Random.Range(0, curWaveChordEnemies.enemyChoose.enemiesChoose.Length)] : curWaveChordEnemies.enemy)));
            }

            if (curWave.hasCircleCrowd)
            {
                curWaveCircleCrowdEnemies = curWave.waveCircleCrowd;
                levelManager.SpawnCircleCrowdEnemies(curWaveCircleCrowdEnemies.time, curWaveCircleCrowdEnemies.amount, curWaveCircleCrowdEnemies.radius, 
                    (curWaveCircleCrowdEnemies.setsEnemyOnChoose ? curWaveCircleCrowdEnemies.enemyChoose.enemiesChoose[Random.Range(0, curWaveCircleCrowdEnemies.enemyChoose.enemiesChoose.Length)] : curWaveCircleCrowdEnemies.enemy));
            }

            if (curWave.hasMiniBoss)
            {
                curWaveMiniBoss = curWave.waveMiniBoss;
                levelManager.SpawnMiniBoss(curWaveMiniBoss.sizeMultiplier, 
                    (curWaveMiniBoss.setsEnemyOnChoose ? curWaveMiniBoss.enemyChoose.enemiesChoose[Random.Range(0, curWaveMiniBoss.enemyChoose.enemiesChoose.Length)] : curWaveMiniBoss.enemy));
            }

            curWaveEnemies = curWave.wave;
            if (curWaveEnemies.setsEnemiesOnChoose) AddEnemiesOnChoose();
            levelManager.StartSpawningEnemies(curWaveEnemies.time, curWaveEnemies.amount, curWaveEnemies.enemies.ToArray());
        }
    }
    */

    public void NextWave()
    {
        curWaveIndex++;

        Wave();
    }

    //main method
    void Wave()
    {
        curWave = mapWaves[curWaveIndex];//set current wave 

        //check additioanal conditions of a wave
        if (curWave.hasCrowd)
        {
            curWaveChordEnemies = curWave.waveCrowd;//set cur wave enemies
            //start coroutine of spawining enemies. assign time, amonut and enemy object for spawn. if there is a choose type of enemies - choose 1 of n
            StartCoroutine(levelManager.SpawnCrowdsEnemiesCor(curWaveChordEnemies.time, curWaveChordEnemies.amount, curWaveChordEnemies.AmountOfChords,
                (curWaveChordEnemies.setsEnemyOnChoose ? curWaveChordEnemies.enemyChoose.enemiesChoose[Random.Range(0, curWaveChordEnemies.enemyChoose.enemiesChoose.Length)] : curWaveChordEnemies.enemy)));
        }

        if (curWave.hasCircleCrowd)
        {
            curWaveCircleCrowdEnemies = curWave.waveCircleCrowd;
            levelManager.SpawnCircleCrowdEnemies(curWaveCircleCrowdEnemies.time, curWaveCircleCrowdEnemies.amount, curWaveCircleCrowdEnemies.radius,
                (curWaveCircleCrowdEnemies.setsEnemyOnChoose ? curWaveCircleCrowdEnemies.enemyChoose.enemiesChoose[Random.Range(0, curWaveCircleCrowdEnemies.enemyChoose.enemiesChoose.Length)] : curWaveCircleCrowdEnemies.enemy));
        }

        if (curWave.hasMiniBoss)
        {
            curWaveMiniBoss = curWave.waveMiniBoss;
            levelManager.SpawnMiniBoss(curWaveMiniBoss.sizeMultiplier,
                (curWaveMiniBoss.setsEnemyOnChoose ? curWaveMiniBoss.enemyChoose.enemiesChoose[Random.Range(0, curWaveMiniBoss.enemyChoose.enemiesChoose.Length)] : curWaveMiniBoss.enemy));
        }

        curWaveEnemies = curWave.wave;
        if (curWaveEnemies.setsEnemiesOnChoose) AddEnemiesOnChoose();
        levelManager.StartSpawningEnemies(curWaveEnemies.time, curWaveEnemies.amount, curWaveEnemies.enemies.ToArray());
    }

    void AddEnemiesOnChoose()//add additional elementals or other enemies on choose
    {
        int n = curWaveEnemies.enemiesChoose.Length;

        for (int i = 0; i < n; i++)
        {
            curWaveEnemies.enemies.Add(curWaveEnemies.enemiesChoose[i].enemiesChoose[Random.Range(0, curWaveEnemies.enemiesChoose[i].enemiesChoose.Length)]);
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
    [SerializeField] public MapWaveChordEnemies waveCrowd;

    [Header("Circle crowd wave")]
    [SerializeField] public bool hasCircleCrowd;
    [SerializeField] public MapWaveCircleCrowdWaveEnemies waveCircleCrowd;

    [Header("Mini boss wave")]
    [SerializeField] public bool hasMiniBoss;
    [SerializeField] public MapWaveMiniBoss waveMiniBoss;
}

//types of waves
[System.Serializable]
public class MapWaveEnemies : MapWaveManyEnemiesTimeAmount
{
}

[System.Serializable]
public class MapWaveChordEnemies : MapWaveSingleEnemyTimeAmount
{
    [SerializeField] public int AmountOfChords = 5;
}

[System.Serializable]
public class MapWaveCircleCrowdWaveEnemies : MapWaveSingleEnemyTimeAmount
{
    [SerializeField] public float radius = 10;
}

[System.Serializable]
public class MapWaveMiniBoss : MapWaveSingleEnemy
{
    [SerializeField] public float sizeMultiplier = 2;
}

//base scripts
[System.Serializable]
public class MapWaveManyEnemiesTimeAmount : MapWaveManyEnemies
{
    [SerializeField] public float time = 60;
    [SerializeField] public int amount = 60;
}


[System.Serializable]
public class MapWaveSingleEnemyTimeAmount : MapWaveSingleEnemy
{
    [SerializeField] public float time = 60;
    [SerializeField] public int amount = 60;
}

[System.Serializable]
public class MapWaveManyEnemies
{
    [SerializeField] public List<GameObject> enemies;

    [Header("Choose logic")]
    [SerializeField] public bool setsEnemiesOnChoose;
    [SerializeField] public WaveEnemiesChoose[] enemiesChoose;
}

[System.Serializable]
public class MapWaveSingleEnemy
{
    [SerializeField] public GameObject enemy;

    [Header("Choose logic")]
    [SerializeField] public bool setsEnemyOnChoose;
    [SerializeField] public WaveEnemiesChoose enemyChoose;
}

[System.Serializable]
public class WaveEnemiesChoose
{
    [SerializeField] public GameObject[] enemiesChoose;
}