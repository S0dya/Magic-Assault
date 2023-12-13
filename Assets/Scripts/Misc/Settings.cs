using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    /// <summary>
    /// main settings
    /// </summary>
    public static bool firstTime;
    public static bool firstTimeInGame;

    public static bool[] unlockedCharacters = new bool[5] { true, false, true, false, false};
    public static int[] charactersPrices = new int[5] { 0, 200, 300, 400, 500, };

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
    public static bool additionalParticles = false;
    public static bool bordersTransparent = false;
    public static bool isQualityMedium = true;

    /// <summary>
    /// variables
    /// </summary>
    //menu
    public static int money = 5000;

    public static float width = Screen.width;
    public static float height = Screen.height;

    //get height and width in world space
    public static float worldHeight = Camera.main.orthographicSize * 4f;
    public static float worldWidth = width / height* worldHeight;

    /// <summary>
    /// in game changing settings
    /// </summary>
    //earth
    public static int amountOfAdditionalForceEffects = 1;
    //wind
    public static int amountOfPassTroughTriggers = 4;

    //other
    public static int[] startingSpells = new int[4] { 0, 1, 0, 3 };

    public static float timeForCheckTransforms = 4;

    public static float[] damageMultipliers = new float[4] { 1, 1, 1, 1 };
    public static float[] damageMultipliersMins = new float[4] { 0.6f, 0.6f, 0.6f, 0.6f };
    public static float[] lifeTimeMultipliers = new float[4] { 1, 1, 1, 1 };

    public static bool allWaterIsPoisened;

    public static void Clear()
    {
        amountOfPassTroughTriggers = 0;
        amountOfAdditionalForceEffects = 0;
    }
}
