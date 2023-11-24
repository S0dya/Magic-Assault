using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base script for UI interaction of Upgrades, Multipliers, Spells
/// </summary>

public class UIPanel : MonoBehaviour 
{
    [Header("Other scripts base")]
    [SerializeField] DrawManager drawManager;

    [Header("This panel")]
    [SerializeField] RectTransform panelTransform;
    [SerializeField] CanvasGroup panelCG;

    //local
    float[] startEndX;
    float[] startEndY;

    //inheriting
    protected float[] StartEndX { get { return startEndX; } set { startEndX = value; } }
    protected float[] StartEndY { get { return startEndY; } set { startEndY = value; } }


    public virtual void Start()//hide panel
    {
        panelTransform.anchoredPosition = new Vector2(startEndX[0], startEndY[0]);
    }

    //panel
    public virtual void OpenTab()
    {
        GameManager.I.MoveTransform(panelTransform, startEndX[1], startEndY[1], 0.75f);
        GameManager.I.Open(panelCG, 0.75f);

        ToggleTimeScale(false);
        drawManager.StopCreatingSpell();
    }

    public virtual void CloseTab()
    {
        GameManager.I.MoveTransform(panelTransform, startEndX[0], startEndY[0], 0.25f);
        GameManager.I.Close(panelCG, 0.25f);

        ToggleTimeScale(true);
    }

    //other methods
    void ToggleTimeScale(bool val)
    {
        StartCoroutine(ToggleOnUICor(!val));
        Time.timeScale = val ? 1 : 0;
    }
    
    //coroutine will make sure ui interaction is not considered in-game interaction
    IEnumerator ToggleOnUICor(bool val)
    {
        yield return null;

        drawManager.isOnUI = val;
        if (!val) drawManager.StopCreatingSpell();
    }

}
