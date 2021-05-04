using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    [Header("Async Loading Info")]
    public float loadingProgress = 0;
    public bool isDoneLoading = false;

    [Header("Transition")]
    public Animator fadeBlack;
    public float transitionDuration = 1f;

    // This script is inteded to  t e l e p o r t  the player across scenes
    public void sceneToIntro()
    {
        SceneManager.LoadScene("Introduction");
    }

    public void sceneTo(string sceneName)
    {
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void sceneExit()
    {
        Application.Quit();
    }

    public void FadeBlackIn()
    {
        fadeBlack.SetBool("isStart", true);
        fadeBlack.SetBool("isDone", false);
    }

    public void FadeBlackOut()
    {
        fadeBlack.SetBool("isStart", true);
        fadeBlack.SetBool("isDone", true);
    }

    public void FadeBlackReset()
    {
        fadeBlack.SetBool("isStart", false);
        fadeBlack.SetBool("isDone", false);
    }

    public void StartFading()
    {
        StartCoroutine(FadeBlackTransition());
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);

        loadingProgress = 0;
        isDoneLoading = false;

        if (fadeBlack != null)
            FadeBlackIn();

        while (!loading.isDone)
        {
            loadingProgress = loading.progress;
            yield return null;
        }

        isDoneLoading = loading.isDone;
    }

    public IEnumerator FadeBlackTransition()
    {
        fadeBlack.SetBool("isStart", true);
        fadeBlack.SetBool("isDone", false);

        yield return new WaitForSeconds(transitionDuration);

        fadeBlack.SetBool("isDone", true);
    }
}
