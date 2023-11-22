using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : SingletonMonobehaviour<UIGameMenu>
{
    [SerializeField] RectTransform gameMenuTransform;
    [SerializeField] CanvasGroup gameMenuCG;

    protected override void Awake()
    {
        base.Awake();

    }

    public void Start()
    {
        gameMenuTransform.anchoredPosition = new Vector2(0, Settings.height);
    }

    //buttons 
    public void PauseButton()
    {
        OpenGameMenu();
    }

    public void ResumeButton()
    {
        CloseGameMenu();
    }

    //methods
    public void OpenGameMenu()
    {
        GameManager.I.MoveTransform(gameMenuTransform, 0, 0, 0.75f);
        GameManager.I.Open(gameMenuCG, 0.75f);

        ToggleTimeScale(false);
        DrawManager.I.StopCreatingSpell();
    }

    public void CloseGameMenu()
    {
        GameManager.I.MoveTransform(gameMenuTransform, 0, Settings.height, 0.25f);
        GameManager.I.Close(gameMenuCG, 0.25f);

        ToggleTimeScale(true);
    }

    //other methods
    public void ToggleTimeScale(bool val)
    {
        StartCoroutine(ToggleOnUICor(!val));
        Time.timeScale = val ? 1 : 0;
    }

    //coroutine will make sure ui interaction is not considered in-game interaction
    IEnumerator ToggleOnUICor(bool val)
    {
        yield return null;

        DrawManager.I.isOnUI = val;
        if (!val) DrawManager.I.StopCreatingSpell();
    }
}
