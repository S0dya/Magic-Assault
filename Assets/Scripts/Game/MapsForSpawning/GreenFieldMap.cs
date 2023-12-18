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

    [Header("Enemies")]
    [SerializeField] GameObject[] zeroWaveEnemies;
    [SerializeField] GameObject[] firstWaveEnemies;
    [SerializeField] GameObject[] secondWaveEnemies;
    [SerializeField] GameObject[] thirdWaveEnemies;
    [SerializeField] GameObject[] forthWaveEnemies;
    [SerializeField] GameObject[] fifthWaveEnemies;
    [SerializeField] GameObject[] sixthWaveEnemies;
    [SerializeField] GameObject[] seventhWaveEnemies;
    [SerializeField] GameObject[] eighthWaveEnemies;
    [SerializeField] GameObject[] ninthWaveEnemies;

    [Header("Crowd enemies")]
    [SerializeField] GameObject firstWaveCrowdEnemy;

    [Header("Circle crowd enemies")]
    [SerializeField] GameObject firstWaveCircleCrowdEnemy;

    //time
    float shortWave = 10;
    float middleWave = 30;
    float longWave = 60;

    //Ns
    int firstWaveEnemiesN;

    void Awake()
    {
        //sign Ns for future using 
        firstWaveEnemiesN = firstWaveEnemies.Length;
        levelManager.platforms = platforms;
    }

    void Start()
    {
        StartCoroutine(WavesCor());
    }

    IEnumerator WavesCor() // each wave spawn enemies, crowds, or circle crowds of enemies
    {
        yield return StartCoroutine(levelManager.SpawnEnemiesCor(longWave, 30, firstWaveEnemies));

        //levelManager.SpawnCircleCrowdEnemies(firstWaveCircleCrowdEnemy, 25);
        //levelManager.SpawnCrowdOfEnemies(firstWaveCrowdEnemy, 10);
        //StartCoroutine(SpawnEnemiesCor(shortWave, 60, firstWaveEnemies, firstWaveEnemiesN));

        //yield return new WaitForSeconds(shortWave);

        while (true)
        {

            levelManager.SpawnMiniBoss(firstWaveEnemies[0], 2);
            yield return StartCoroutine(levelManager.SpawnEnemiesCor(shortWave, 30, firstWaveEnemies));

        }

        while (true)
        {
            levelManager.SpawnCrowdOfEnemies(firstWaveCrowdEnemy, 10);
            yield return new WaitForSeconds(shortWave);

        }
    }
}
