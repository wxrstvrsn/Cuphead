using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image blackPanel; // Панель для fade
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float minLoadTime = 5f;

    private void Start()
    {
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        yield return FadeIn(); // затемнение

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneLoader.TargetSceneName);
        asyncLoad.allowSceneActivation = false;

        float timer = 0f;

        while (!asyncLoad.isDone)
        {
            timer += Time.deltaTime;

            if (asyncLoad.progress >= 0.9f && timer >= minLoadTime)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        yield return FadeOut(); // убираем черноту
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