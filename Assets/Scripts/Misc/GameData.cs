using UnityEngine;

public class GameData : SingletonMonobehaviour<GameData>
{
    [HideInInspector] public int timeInGame;
    [HideInInspector] public int killedEnemies;
    [HideInInspector] public int goldEarned;
    [HideInInspector] public int levelReached;
    [HideInInspector] public int damageDone;
    [HideInInspector] public int[] elementalDamageDone = new int[4];

    //earth
    [HideInInspector] public int amountOfAdditionalForceEffects = 1;
    //wind
    [HideInInspector] public int amountOfPassTroughTriggers = 4;
}
