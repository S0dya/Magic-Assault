using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// waves in min : 
/// 0 1 wave enemies
/// 1 half little enemies, mini boss, half 2 wave enemies, 
/// 2 half first wave enemies, half little enemies, crowd enemies (3 (20 secs))
/// 3 many 3 wave enemies, mini boss
/// 4 half 4 wave (fast enemies), half 3 wave enemies
/// 5 boss, half 5 wave enemies, circle crowd
/// 6 half 2 wave enmies, half 5 wave enmemies
/// 7 mini boss, half 1 wave enemies, half 6 wave enemies, crowd enemies (5 (30 secs))
/// 8 many 3 wave enemies, crowd enemies (3 (1 min))
/// 9 mini boss, half 7 wave enemies, half 3 wave enemies crowd enemies (5)
/// 10 boss, circle crowd enemies, half 6 wave enemies, half 5 wave enemies
/// 11 many 3 wave enemies
/// 12 half 4 wave enemies, half 8 wave enemies, some 3 wave enemies
/// 13 mini boss, many 4 wave crowd enemies, half 8 wave enemies, half 4 wave enemies
/// 14 mini boss, half 7 wave enemies, half 8 wave enemies 
/// 15 circle crowd enemies, 5, 6, 7, 8 wave enemies
/// 16 mini boss, half 5, 6 wave enemies, half 9 wave enemies
/// 17 half (slow) 10 wave enemies
/// 
/// </summary>

public class GreenFieldMap : MonoBehaviour
{
    [Header("Other scripts")]
    [SerializeField] LevelManager levelManager;
    
    [Header("platforms")]
    [SerializeField] GameObject[] platforms;

    [Header("Waves")]
    [SerializeField][TextArea(30, 10)] string DescriptionOfWavesOfAMap;
    [SerializeField] MapWave[] mapWaves;

    [Header("Enemies")]
    [SerializeField] GameObject[] wave1;
    [SerializeField] GameObject[] wave2;
    [SerializeField] GameObject[] wave3;
    [SerializeField] GameObject[] wave4;
    [SerializeField] GameObject[] wave5;
    [SerializeField] GameObject[] wave6;
    [SerializeField] GameObject[] wave7;
    [SerializeField] GameObject[] wave8;
    [SerializeField] GameObject[] wave9;
    [SerializeField] GameObject[] wave10;
    [SerializeField] GameObject[] wave11;
    [SerializeField] GameObject[] wave12;
    [SerializeField] GameObject[] wave13;
    [SerializeField] GameObject[] wave14;
    [SerializeField] GameObject[] wave15;
    [SerializeField] GameObject[] wave16;
    [SerializeField] GameObject[] wave17;
    [SerializeField] GameObject[] wave18;
    [SerializeField] GameObject[] wave19;
    [SerializeField] GameObject[] wave20;
    [SerializeField] GameObject[] wave21;
    [SerializeField] GameObject[] wave22;
    [SerializeField] GameObject[] wave23;
    [SerializeField] GameObject[] wave24;
    [SerializeField] GameObject[] wave25;
    [SerializeField] GameObject[] wave26;
    [SerializeField] GameObject[] wave27;
    [SerializeField] GameObject[] wave28;
    [SerializeField] GameObject[] wave29;
    [SerializeField] GameObject[] wave30;

    [Header("Crowd enemies")]
    [SerializeField] GameObject[] crowdEnemies;

    [Header("Circle crowd enemies")]
    [SerializeField] GameObject[] circleCrowdEnemies;

    [Header("Different elementals")]
    [SerializeField] GameObject[] firstElementals;

    [Header("Bosses")]
    [SerializeField] GameObject[] bosses;

    [SerializeField] int[][] a;

    //local
    GameObject firstElemental;

    //treshhold
    MapWave curMapWave;
    MapWaveEnemies curMapWaveEnemies;
    MapWaveMiniBoss curWaveMiniBoss;

    //time
    float shortWave = 10;
    float middleWave = 30;
    float longWave = 60;

    //Ns

    void Awake()
    {
        levelManager.platforms = platforms;
    }

    void Start()
    {
        //assign random elementals
        firstElemental = firstElementals[Random.Range(0, 4)];

        StartCoroutine(WavesCor());
    }

    IEnumerator WavesCor() // each wave spawn enemies, crowds, or circle crowds of enemies
    {
        for (int i = 0; i < 30; i++)
        {
            curMapWave = mapWaves[i];

            if (curMapWave.hasCrowd)
            {
                curMapWaveEnemies = curMapWave.waveCrowd;
                StartCoroutine(levelManager.SpawnCrowdsEnemiesCor(curMapWaveEnemies.time, curMapWaveEnemies.amount, curMapWaveEnemies.enemies[0]));
            }

            if (curMapWave.hasCircleCrowd)
            {
                curMapWaveEnemies = curMapWave.waveCircleCrowd;
                levelManager.SpawnCircleCrowdEnemies(curMapWaveEnemies.time, curMapWaveEnemies.amount, curMapWaveEnemies.enemies[0]);
            }

            if (curMapWave.hasMiniBoss)
            {
                curWaveMiniBoss = curMapWave.waveMiniBoss;
                levelManager.SpawnMiniBoss(curWaveMiniBoss.enemy, curWaveMiniBoss.sizeMultiplier);
            }

            curMapWaveEnemies = curMapWave.wave;
            yield return StartCoroutine(levelManager.SpawnEnemiesWaveCor(curMapWaveEnemies.time, curMapWaveEnemies.amount, curMapWaveEnemies.enemies));
        }

        /*
        levelManager.SpawnCircleCrowdEnemies(35, circleCrowdEnemies[0]);
        StartCoroutine(levelManager.SpawnCrowdsEnemiesCor(longWave, 15, crowdEnemies[1]));
        levelManager.SpawnMiniBoss(firstElemental, 2);
        //00
        yield return StartCoroutine(levelManager.SpawnEnemiesWaveCor(longWave, 60, wave1));

        //01
        levelManager.SpawnMiniBoss(wave1[0], 2);
        yield return StartCoroutine(levelManager.SpawnEnemiesWaveCor(longWave, 60, wave2));

        //02
        StartCoroutine(levelManager.SpawnCrowdsEnemiesCor(shortWave, 15, crowdEnemies[0]));
        yield return StartCoroutine(levelManager.SpawnEnemiesWaveCor(longWave, 90, wave3));
        
        //03
        levelManager.SpawnMiniBoss(wave3[0], 2);
        yield return StartCoroutine(levelManager.SpawnEnemiesWaveCor(longWave, 150, wave4));

        //04
        yield return StartCoroutine(levelManager.SpawnEnemiesWaveCor(longWave, 120, wave5));

        //05
        levelManager.SpawnCircleCrowdEnemies(35, circleCrowdEnemies[0]);
        levelManager.SpawnMiniBoss(firstElemental, 2);
        yield return StartCoroutine(levelManager.SpawnEnemiesWaveCor(longWave, 30, wave6));

        //06
        yield return StartCoroutine(levelManager.SpawnEnemiesWaveCor(longWave, 90, wave7));

        //07

        while (true)
        {

            levelManager.SpawnMiniBoss(wave1[0], 2);
            yield return StartCoroutine(levelManager.SpawnEnemiesWaveCor(longWave, 120, wave1));

        }

        while (true)
        {
            StartCoroutine(levelManager.SpawnCrowdsEnemiesCor(middleWave, 10, crowdEnemies[0]));
            yield return new WaitForSeconds(shortWave);

        }
        */
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
}

[System.Serializable]
public class MapWaveMiniBoss
{
    [SerializeField] public GameObject enemy;

    [SerializeField] public float sizeMultiplier = 2;
}