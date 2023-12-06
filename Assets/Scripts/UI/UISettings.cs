using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : UIPanel
{
    [Header("Music")]
    [SerializeField] Slider[] sliders;

    void Awake()
    {
        StartEndX = new float[2] { Settings.width, 0 };
        StartEndY = new float[2] { 0, 0 };
    }

    //buttons
    public void MusicChange(int index)
    {
        AudioManager.I.SetVolume(index, sliders[index].value);
    }
    

}
