using UnityEngine;

public class GameData : SingletonMonobehaviour<GameData>
{
    [HideInInspector] public int timeInGame;
    [HideInInspector] public int damageDone;
    [HideInInspector] public int[] elementalDamageDone = new int[4];

    //earth
    [HideInInspector] public int amountOfAdditionalForceEffects = 1;
    //wind
    [HideInInspector] public int amountOfPassTroughTriggers = 4;
}
