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


    protected virtual void Start()//hide panel
    {
        panelTransform.anchoredPosition = new Vector2(startEndX[0], startEndY[0]);
    }

    //panel
    public virtual void OpenTab()
    {
        MoveTab(startEndX[1], startEndY[1], 0.5f);
        GameManager.I.Open(panelCG, 0.75f);
    }

    public virtual void CloseTab()
    {
        MoveTab(startEndX[0], startEndY[0], 0.2f);
        GameManager.I.Close(panelCG, 0.25f);
    }

    //panel in menu
    public virtual void OpenTabInMenu()
    {
        MoveTab(startEndX[1], startEndY[1], 0.35f);
        ToggleCGRaycast(true);
    }

    public virtual void CloseTabInMenu()
    {
        MoveTab(startEndX[0], startEndY[0], 0.1f);
        ToggleCGRaycast(false);
    }

    //panel in game
    public virtual void OpenTabInGame()
    {
        OpenTab();
     
        ToggleTimeScale(false);
        drawManager.StopCreatingSpell();
    }

    public virtual void CloseTabInGame()
    {
        CloseTab();
        
        ToggleTimeScale(true);
    }

    //other methods
    void MoveTab(float x, float y, float speed)
    {
        GameManager.I.MoveTransform(panelTransform, x, y, speed);
    }
    void ToggleCGRaycast(bool val) => panelCG.blocksRaycasts = val;

    void ToggleTimeScale(bool val)
    {
        float floatVal = val ? 1 : 0;
        StartCoroutine(ToggleOnUICor(!val));
        Time.timeScale = floatVal;
        UIInGame.I.ToggleJoystickVisibility(floatVal);
    }

    //coroutine will make sure ui interaction is not considered in-game interaction
    IEnumerator ToggleOnUICor(bool val)
    {
        yield return null;

        drawManager.isOnUI = val;
        if (!val) drawManager.StopCreatingSpell();
    }
}
