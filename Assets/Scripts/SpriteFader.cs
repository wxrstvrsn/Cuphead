using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFader : MonoBehaviour
{
    [Header("Имя сцены для перехода")]
    [SerializeField] private string nextSceneName;

    [Header("Время фейда (сек)")]
    [SerializeField] private float fadeDuration = 1f;

    [Header("Минимальное время чёрного экрана (сек)")]
    [SerializeField] private float minBlackTime = 0.5f;

    // Флаг, что сейчас transition идёт
    private bool isTransitioning;
    public bool IsTransitioning => isTransitioning;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // Растянем спрайт под размер камеры (ортографическая)
        var cam = Camera.main;
        float h = cam.orthographicSize * 2f;
        float w = h * cam.aspect;
        transform.localScale = new Vector3(w, h, 1f);
        // Позиционируем строго перед камерой
        transform.position = cam.transform.position + cam.transform.forward * (cam.nearClipPlane + 0.1f);

        // Сразу чёрный экран
        SetAlpha(1f);
        isTransitioning = true;
    }

    private void Start()
    {
        // Первый FadeIn при старте сцены
        StartCoroutine(FadeRoutine(1f, 0f, fadeDuration, onComplete: () =>
        {
            isTransitioning = false;
            gameObject.SetActive(false);
        }));
    }

    /// <summary>
    /// Запустить плавный переход к nextSceneName (или к targetScene, если передали).
    /// </summary>
    public void StartTransition(string targetScene = null)
    {
        if (isTransitioning) return;

        if (!string.IsNullOrEmpty(targetScene))
            nextSceneName = targetScene;

        gameObject.SetActive(true);
        isTransitioning = true;

        // Сначала FadeOut
        StartCoroutine(FadeRoutine(0f, 1f, fadeDuration, () =>
        {
            // Параллельно запускаем асинхронную загрузку
            StartCoroutine(LoadAndFadeIn());
        }));
    }

    private IEnumerator LoadAndFadeIn()
    {
        // Асинхронная загрузка (Single)
        var op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (timer < minBlackTime || op.progress < 0.9f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        op.allowSceneActivation = true;
        yield return new WaitUntil(() => op.isDone);

        // FadeIn обратно
        yield return FadeRoutine(1f, 0f, fadeDuration, onComplete: () =>
        {
            isTransitioning = false;
            gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// Плавно меняет альфу спрайта от from→to за duration секунд.
    /// </summary>
    private IEnumerator FadeRoutine(float from, float to, float duration, System.Action onComplete)
    {
        float elapsed = 0f;
        SetAlpha(from);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }

        SetAlpha(to);
        onComplete?.Invoke();
    }

    private void SetAlpha(float a)
    {
        var c = sr.color;
        c.a = a;
        sr.color = c;
    }
}
