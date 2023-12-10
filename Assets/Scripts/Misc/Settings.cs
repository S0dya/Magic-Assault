using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings 
{
    //settings
    public static bool firstTime;
    public static bool firstTimeInGame;
    //gameSettings
    public static bool showDamageNumbers = true;
    public static bool showBlood = true;
    public static bool isFloatingJoystick = true;
    public static float[] musicStats = new float[2] { 0.5f, 0.5f };
    //menuSettings
    public static bool additionalParticles = true;
    public static bool bordersTransparent = true;


    //menu
    public static int money;

    public static float width = Screen.width;
    public static float height = Screen.height;

        //get height and width in world space
    public static float worldHeight = Camera.main.orthographicSize * 4f;
    public static float worldWidth = width / height* worldHeight;

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

    
    //in game changing settings
    public static int[] startingSpells = new int[4] { 0, 1, 0, 3 };
    public static float[] startingSpellsManaUsage = new float[4] { 15f, 15f, 3f, 10f };

    public static float timeForCheckTransforms = 4;

    public static float[] damageMultipliers = new float[4] { 1, 1, 1, 1 };
    public static float[] damageMultipliersMins = new float[4] { 0.6f, 0.6f, 0.6f, 0.6f };
    public static float[] lifeTimeMultipliers = new float[4] { 1, 10, 1, 1 };

    public static bool allWaterIsPoisened;

    public static void Clear()
    {

    }
}
