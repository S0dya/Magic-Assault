using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFieldMap : MonoBehaviour
{
    [Header("Settings")]
    public int maxEnemiesAmount;

    [Header("Enemies")]
    [SerializeField] GameObject[] firstWaveEnemies;

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
        firstWaveEnemiesN = firstWaveEnemies.Length;
    }

    void Start()
    {
        StartCoroutine(WavesCor());
    }

    IEnumerator WavesCor()
    {
        StartCoroutine(SpawnEnemiesCor(shortWave, 60, firstWaveEnemies, firstWaveEnemiesN));

        yield return new WaitForSeconds(middleWave);

        levelManager.SpawnCrowdOfEnemies(firstWaveEnemies[0], 10);

    }

    IEnumerator SpawnEnemiesCor(float totalTime, float amountOfEnemies, GameObject[] enemies, int n)
    {
        float time = totalTime / amountOfEnemies;

        while (totalTime > 0)
        {
            levelManager.SpawnEnemy(enemies[Random.Range(0, n)]);
            
            totalTime -= time;

            yield return new WaitForSeconds(time);
        }
    }
}
