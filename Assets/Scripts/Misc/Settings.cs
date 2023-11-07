using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings 
{


    //menu
    public static int money;

    //shop

    //game
    public static float takingDamageVisualisationTime = 0.15f;

    //player 
    public static float[] amountOfTimeBeforeRestoringStats = new float[2] { 1, 1 };
    public static float[] amountOfTimeForRestoringStats = new float[2] { 0.2f, 0.2f };
    public static float[] amountOfRestoringStats = new float[2] { 5, 5 };

    public static bool canRestoreHp = false;

    public static int durationOfBurning = 10;

    public static float timeForPushEnd = 0.5f;

    //spells
    public static int[] startingSpells = new int[4];
    public static float[] startingSpellsManaUsage = new float[4] { 5f, 15f, 3f, 10f };

    //dot

    //stone
    public static float size = 1f;

    public static float damageOfStone = 7f;

    //line

    //fire
    public static float damageOfFire = 5f;
    public static float damageOfBurning = 2f;
    public static float sizeOfFireMultiplier = 1f;
    
    //circle

    //water
    public static float maxSizeOfWater = 3.5f;
    
    //arrow
    public static bool additionalEffects = true;

    //air
    public static float damageOfWind = 5f;


    //wind
    public static float sizeOfWind = 1;//change to multiplier

    //settings
    public static bool isFloatingJoystick = false;
    public static bool firstTime;
    public static bool isMusicOn;
}
