using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuMapDescription : UIPanel
{
    [Header("Maps")]
    [SerializeField] SO_Map[] maps;

    [Header("Map Description")]
    [SerializeField] Image mapImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;

    [Header("Stats panel")]
    [SerializeField] TextMeshProUGUI timeLimitText;
    [SerializeField] TextMeshProUGUI clockSpeedText;
    [SerializeField] TextMeshProUGUI moveSpeedText;

    [SerializeField] TextMeshProUGUI goldBonusText;
    [SerializeField] TextMeshProUGUI luckBonusText;
    [SerializeField] TextMeshProUGUI expBonusText;

    //local
    [HideInInspector] public int curMapI;

    void Awake()
    {
        StartEndX = new float[2] { 0, 0 };
        StartEndY = new float[2] { -Settings.height, 0 };
    }

    protected override void Start()
    {
        base.Start();

        OnMapClicked(0);
    }

    //buttons
    public void OnMapClicked(int i)
    {
        curMapI = i;

        SetInfo(maps[i]);
    }

    //main methods
    void SetInfo(SO_Map item)
    {
        mapImage.sprite = item.ItemImage;
        nameText.text = item.Name;
        descriptionText.text = item.Description;

        timeLimitText.text = item.timeLimit;
        SetString(item.clockSpeed, clockSpeedText);
        SetString(item.moveSpeed, moveSpeedText);

        SetString(item.goldBonus, goldBonusText);
        SetString(item.luckBonus, luckBonusText);
        SetString(item.expBonus, expBonusText);
    }
}
