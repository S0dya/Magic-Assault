using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResults : SingletonMonobehaviour<UIResults>
{
    [Header("Other scripts")]
    [SerializeField] GameData gameData;
    [SerializeField] UIInGame uiInGame;

    [Header("Panel")]
    [SerializeField] CanvasGroup bgCg;
    [SerializeField] CanvasGroup panelCg;

    [Header("Stats")]
    //header
    [SerializeField] TextMeshProUGUI survivedAmountText;
    [SerializeField] TextMeshProUGUI goldAmountText;
    [SerializeField] TextMeshProUGUI levelAmountText;
    [SerializeField] TextMeshProUGUI killedAmountText;

    //body 
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI characterName;

    [SerializeField] UIResultStatsItem[] multipliersItems;
    [SerializeField] UIResultStatsItem[] spellsItems;

    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI[] elementalDamageTexts;



    public void SetTransparentBg()
    {
        GameManager.I.Open(bgCg, 0.2f, 0.4f);
    }

    public void OpenPanel()
    {
        GameManager.I.Open(bgCg, 0.3f);

        Invoke("OpenTab", 0.3f);
    }

    public void OpenTab()
    {
        GameManager.I.Open(panelCg, 0.4f);
         
    }

    //buttons
    public void OnQuit()
    {

    }

    public void OnWatchAd()
    {

    }

    public void OnDone()
    {

    }

    //main methods
    void SetStats()
    {
        SetString(gameData.timeInGame, survivedAmountText);
        SetString(gameData.goldEarned, goldAmountText);
        SetString(gameData.levelReached, levelAmountText);
        SetString(gameData.killedEnemies, killedAmountText);

        characterImage.sprite = Settings.characterSpriteInGame;
        characterName.text = Settings.characterName;

        for (int i = 0; i < 4; i++)
        {
            multipliersItems[i].SetAmountText((int)Settings.damageMultipliers[i] * 10);


            SetString(gameData.elementalDamageDone[i], elementalDamageTexts[i]);
        }

        SetString(gameData.damageDone, damageText);
    }

    //other methods
    public void SetString(int val, TextMeshProUGUI text) => text.text = val.ToString();//a bit of spaghetti
}
