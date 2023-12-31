using UnityEngine;

public class GameData : SingletonMonobehaviour<GameData>
{
    [HideInInspector] public int timeInGame;
    [HideInInspector] public int damageDone;
    [HideInInspector] public int[] elementalDamageDone = new int[4];

    //upgrardes
    [HideInInspector] public float power = 1f;
    [HideInInspector] public float cooldown = 1f;
    [HideInInspector] public float area = 1;
    [HideInInspector] public int amount;

    [HideInInspector] public float luck;
    [HideInInspector] public float lifetimeMultiplier = 1;
    [HideInInspector] public float growth = 1;
    [HideInInspector] public float greed = 1;

    //envrenment
    [HideInInspector] public float freezeTime = 7;

    //elementals
    [HideInInspector] public float[] elementalLifeTimeMultipliers = new float[4] { 1, 1, 1, 1 };

    //fire
    [HideInInspector] public float explosionDamageMultiplier = 1;
    //water
    [HideInInspector] public bool isWaterPoisened;
    [HideInInspector] public float poisonDamageMultiplier = 1;
    //earth
    [HideInInspector] public int amountOfAdditionalForceEffects = 1;
    //wind
    [HideInInspector] public int amountOfPassTroughTriggers = 1;
}
