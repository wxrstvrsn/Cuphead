using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image blackPanel; // Панель для fade
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float minLoadTime = 5f;
    [SerializeField] private GameObject deathlessObjects;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(deathlessObjects);
    }

    private void Start()
    {
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        // Плавное появление загрузочной сцены
        yield return FadeIn(); 

        // Загружаем сцену в фоне
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneLoader.TargetSceneName);
        asyncLoad.allowSceneActivation = false;  // Сцена не активируется, пока не будет готова

        float timer = 0f;

        while (!asyncLoad.isDone)
        {
            timer += Time.deltaTime;

            // Как только сцена почти загружена и минимальное время прошло
            if (asyncLoad.progress >= 0.9f && timer >= minLoadTime)
            {
                asyncLoad.allowSceneActivation = true;  // Активируем сцену
            }

            yield return null;
        }

        // Плавное исчезновение загрузочного экрана
        yield return FadeOut(); 
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color color = blackPanel.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            blackPanel.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color color = blackPanel.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            blackPanel.color = color;
            yield return null;
        }
    }
}