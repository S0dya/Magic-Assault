using UnityEngine;

public class GameData : SingletonMonobehaviour<GameData>
{
    [HideInInspector] public int timeInGame;
    [HideInInspector] public int damageDone;
    [HideInInspector] public int[] elementalDamageDone = new int[4];

    //upgrardes
    [HideInInspector] public float power = 1f;
    [HideInInspector] public float cooldown = 1f;

    [HideInInspector] public float lifetimeMultiplier = 1;


    //elementals
    [HideInInspector] public float[] elementalLifeTimeMultipliers = new float[4] { 1, 1, 1, 1 };

    //earth
    [HideInInspector] public int amountOfAdditionalForceEffects = 1;
    //wind
    [HideInInspector] public int amountOfPassTroughTriggers = 4;
}
