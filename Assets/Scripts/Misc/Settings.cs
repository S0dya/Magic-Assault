using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings 
{


    //menu
    public static int money;

    public static float height = Screen.height;

    //game


    //dont clear 
    //spells

    //dot

    //fire 

    //earth

    public static int amountOfAdditionalForceEffects = 1;

    //wind
    public static int amountOfPassTroughTriggers = 4;

    //fire

    //circle
    public static float maxSizeOfCircleSpell = 3.5f;

    //arrow
    public static bool additionalEffectsOfArrow = true;

    



    //settings
    public static bool isFloatingJoystick = false;
    public static bool firstTime;
    public static bool isMusicOn;

    //game
    public static int[] startingSpells = new int[4] { 0, 1, 2, 3 };
    public static float[] startingSpellsManaUsage = new float[4] { 15f, 15f, 3f, 10f };

    public static float timeForCheckTransforms = 4;

    public static float[] damageMultipliers = new float[4] { 1, 3, 1, 1 };
    public static float[] lifeTimeMultipliers = new float[4] { 1, 10, 1, 1 };

    public static bool allWaterIsPoisened;

    public static void Clear()
    {

    }
}
