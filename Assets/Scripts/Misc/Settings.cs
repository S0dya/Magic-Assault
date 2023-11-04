using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings 
{


    //menu
    public static int money;

    //shop

    //game
    //player 
    public static float[] amountOfTimeBeforeRestoringStats = new float[2] { 1, 1 };
    public static float[] amountOfTimeForRestoringStats = new float[2] { 0.2f, 0.2f };
    public static float[] amountOfRestoringStats = new float[2] { 5, 5 };

    public static bool canRestoreHp;

    //spells
    //dot

    //line

    //circle

    //arrow
    public static bool additionalEffects = true;

    //fire
    public static float sizeOfFire = 1;

    //water
    public static float maxSizeOfWater = 3.5f;

    //stone
    public static float size = 1f;

    //wind
    public static float sizeOfWind = 1;//change to multiplier

    //settings
    public static bool isFloatingJoystick = false;
    public static bool firstTime;
    public static bool isMusicOn;
}
