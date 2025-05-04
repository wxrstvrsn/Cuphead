// SceneFader.cs

using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class SceneFader : MonoBehaviour
{
    [Header("Overlay CanvasGroup (fullscreen black)")] [SerializeField]
    private CanvasGroup canvasGroup;

    [Header("Transition Settings")] [SerializeField]
    private string nextSceneName;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float minBlackTime = 0.5f;
    [SerializeField] private float postFadeDelay = 0.3f;

    private bool isTransitioning;
    public bool IsTransitioning => isTransitioning;
    public event Action OnTransitionComplete;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // Изначально — чёрный экран, блокируем ввод
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = false;
    }

    private void Start()
    {
        StartCoroutine(InitialFadeIn());
    }

    public void StartTransition(string targetScene = null)
    {
        if (isTransitioning) return;
        
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
        
        if (!string.IsNullOrEmpty(targetScene))
            nextSceneName = targetScene;


        // Блокируем ввод, даже если canvas уже был прозрачным
        canvasGroup.blocksRaycasts = true;

        StartCoroutine(TransitionSequence());
    }

    private IEnumerator InitialFadeIn()
    {
        isTransitioning = true;

        yield return Fade(1f, 0f, fadeDuration);

        // После FadeIn разрешаем ввод
        canvasGroup.blocksRaycasts = false;

        yield return new WaitForSeconds(postFadeDelay);
        isTransitioning = false;
    }

    private IEnumerator TransitionSequence()
    {
        print("[SceneFader] Transition started");
        isTransitioning = true;

        // FadeOut: прозрачный → чёрный
        yield return Fade(0f, 1f, fadeDuration);

        // Асинхронная загрузка новой сцены
        var loadOp = SceneManager.LoadSceneAsync(nextSceneName);
        loadOp.allowSceneActivation = false;

        float timer = 0f;
        while (timer < minBlackTime || loadOp.progress < 0.9f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        loadOp.allowSceneActivation = true;
        yield return new WaitUntil(() => loadOp.isDone);

        // FadeIn: чёрный → прозрачный
        yield return Fade(1f, 0f, fadeDuration);

        // После FadeIn разрешаем ввод и уведомляем подписчиков
        canvasGroup.blocksRaycasts = false;
        yield return new WaitForSeconds(postFadeDelay);

        isTransitioning = false;
        print("[SceneFader] Transition Complete, isTransitioning set to **false**");
        OnTransitionComplete?.Invoke();
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}