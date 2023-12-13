using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// base script for UI interaction
/// </summary>

public class UIPanel : MonoBehaviour 
{
    [Header("This panel")]
    public RectTransform panelTransform;
    public CanvasGroup panelCG;

    //local
    float[] startEndX;
    float[] startEndY;
    
    //inheriting
    protected float[] StartEndX { get { return startEndX; } set { startEndX = value; } }
    protected float[] StartEndY { get { return startEndY; } set { startEndY = value; } }

    protected virtual void Start() => panelTransform.anchoredPosition = new Vector2(startEndX[0], startEndY[0]); //hide panel 

    public void MoveTab(int i, float speed) => GameManager.I.MoveTransform(panelTransform, startEndX[i], startEndY[i], speed);

    public void SetString(int val, TextMeshProUGUI text) => text.text = val.ToString();
    public void SetString(float val, TextMeshProUGUI text) => text.text = val.ToString();
}
