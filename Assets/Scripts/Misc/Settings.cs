using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings 
{


    //menu
    public static int money;

    //game
    

    //dont clear 
    //spells

    //dot

    //fire 
    public static float damageOfFireball = -5f;

    //earth
    public static float damageOfStone = -10f;

    public static int amountOfAdditionalForceEffects = 1;
    
    //wind
    public static float damageOfWind = -5f;

    //fire
    public static float damageOfFire = -5f;
    public static float damageOfBurning = -2f;
    public static float sizeOfFireMultiplier = 1f;

    //circle
    public static float maxSizeOfCircleSpell = 3.5f;

    //arrow
    public static bool additionalEffectsOfArrow = true;

    



    //settings
    public static bool isFloatingJoystick = false;
    public static bool firstTime;
    public static bool isMusicOn;

    //game
    public static float[] startingSpellsDamage = new float[4] { damageOfStone, 0, damageOfFire, damageOfWind };
    public static int[] startingSpells = new int[4];
    public static float[] startingSpellsManaUsage = new float[4] { 15f, 15f, 3f, 10f };

    public static float timeForCheckTransforms = 4;


    public static float[] damageMultipliers = new float[4] { 1, 1, 1, 1 };

    public static bool allWaterIsPoisened;

    public static void Clear()
    {

    }
}
