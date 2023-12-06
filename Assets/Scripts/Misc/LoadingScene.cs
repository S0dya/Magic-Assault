using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class LoadingScene : SingletonMonobehaviour<LoadingScene>
{
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] Image LoadingBarFill;

    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(LoadGame());
    }

    public IEnumerator LoadGame()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progression = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progression;

            yield return null;
        }

        LoadingScreen.SetActive(false);
    }

    public IEnumerator LoadMenuAsync(int sceneToClose)
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneToClose);
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        LoadingScreen.SetActive(true);

        /*
        else if (sceneToClose > 1)
        {
            AudioManager.Instance.ToggleMusicRain(false);
        }
        */

        while (!operation.isDone)
        {
            float progression = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progression;

            yield return null;
        }

        LoadingScreen.SetActive(false);
    }

    public IEnumerator LoadSceneAsync(int sceneId, int sceneToClose)
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneToClose);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);

        /*
        else if (sceneToClose > 1)
        {
            AudioManager.Instance.ToggleMusicRain(false);
        }
        */

        LoadingScreen.SetActive(true);

        while (!unloadOperation.isDone)
        {
            yield return null;
        }

        while (!operation.isDone)
        {
            float progression = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progression;
            yield return null;
        }
    }
}
