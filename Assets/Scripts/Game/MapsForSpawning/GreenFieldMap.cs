using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFieldMap : MonoBehaviour
{
    [Header("platforms")]
    [SerializeField] GameObject[] platforms;

    [Header("Enemies")]
    [SerializeField] GameObject[] firstWaveEnemies;

    [Header("Crowd enemies")]
    [SerializeField] GameObject firstWaveCrowdEnemy;

    [Header("Circle crowd enemies")]
    [SerializeField] GameObject firstWaveCircleCrowdEnemy;


    [Header("Other")]
    [SerializeField] LevelManager levelManager;

    //local
    [HideInInspector] public int curDeadEnemiesAmount;

    //time
    float shortWave = 10;
    float middleWave = 60;
    float longWave = 90;

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
        //levelManager.SpawnCircleCrowdEnemies(firstWaveCircleCrowdEnemy, 25);
        //levelManager.SpawnCrowdOfEnemies(firstWaveCrowdEnemy, 10);
        //StartCoroutine(SpawnEnemiesCor(shortWave, 60, firstWaveEnemies, firstWaveEnemiesN));

        //yield return new WaitForSeconds(shortWave);

        while (true)
        {
            StartCoroutine(SpawnEnemiesCor(shortWave, 60, firstWaveEnemies, firstWaveEnemiesN));
            yield return new WaitForSeconds(shortWave);

        }

        while (true)
        {
            levelManager.SpawnCrowdOfEnemies(firstWaveCrowdEnemy, 10);
            yield return new WaitForSeconds(shortWave);

        }

    }

    IEnumerator SpawnEnemiesCor(float totalTime, float amountOfEnemies, GameObject[] enemies, int n)
    {
        //get average time of spawning one enemy
        float time = totalTime / amountOfEnemies;

        while (totalTime > 0)// iterate while there is time
        {
            levelManager.SpawnEnemy(enemies[Random.Range(0, n)]);

            totalTime -= time;

            yield return new WaitForSeconds(time);
        }
    }
}
