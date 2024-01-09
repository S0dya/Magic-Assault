using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    /// <summary>
    /// main settings
    /// </summary>
    public static bool firstTime;
    public static bool showTraining = true;

    public static int[] charactersPrices = new int[5] { 0, 903, 425, 500, 850, };
    public static bool[] unlockedMaps = new bool[5] { true, false, false, false, false };

    /// <summary>
    /// options
    /// </summary>
    //gameOptions
    public static bool showDamageNumbers = true;
    public static bool showBlood = true;
    public static bool isFloatingJoystick = true;
    public static float[] musicStats = new float[2] { 0.5f, 0.5f };
    //menuOptions
    public static bool additionalParticles = true;
    public static bool bordersTransparent = true;
    public static bool isQualityMedium = true;

    /// <summary>
    /// variables
    /// </summary>
    //menu
    public static int money = 0;

    public static float width = Screen.width;
    public static float height = Screen.height;

    //get height and width in world space
    public static float worldHeight = Camera.main.orthographicSize * 3f;
    public static float worldWidth = width / height* worldHeight;

    /// <summary>
    /// in game changing settings
    /// </summary>

    //other
    public static int[] startingSpells = new int[4] { 0, -1, -1, -1 };

    public static float[] damageMultipliers = new float[4] { 1, 1, 1, 1 };
    public static float[] damageMultipliersMins = new float[4] { 0.6f, 0.6f, 0.6f, 0.6f };


    /// <summary>
    /// Game data
    /// </summary>
    public static int totalTimeInGame;
    public static int totalKilledEnemies;
    public static int totalGoldEarned;
    public static int totalLevelReached;
    public static int maxTotalLevelReached;
    public static int totalDamageDone;
    public static int[] totalElementalDamageDone = new int[4];

    //other data
    public static Sprite characterSprite;
    public static Sprite characterSpriteInGame;
    public static string characterName;
    public static int characterI;
    public static int curSceneId;
}
