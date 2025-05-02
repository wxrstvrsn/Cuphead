using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoadingSystem : MonoBehaviour
{
    public static LoadingSystem Instance;

    [Header("UI")]
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private GameObject deathlessObjects;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float minLoadTime = 2f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(deathlessObjects);
    }

    private void Start()
    {
        Debug.Log("LoadingSystem started. Target = " + SceneLoader.TargetSceneName);
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(SceneLoader.TargetSceneName))
            SceneLoader.TargetSceneName = "Intro";
#endif
        if (!string.IsNullOrEmpty(SceneLoader.TargetSceneName))
        {
            LoadTargetScene();
        }
    }

    /// <summary>
    /// Стандартная загрузка любой сцены (в обход SceneLoader)
    /// </summary>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    /// <summary>
    /// Загружает сцену, указанную в SceneLoader.TargetSceneName
    /// Используется, когда загружена сцена-переход (LoadingScene)
    /// </summary>
    public void LoadTargetScene()
    {
        StartCoroutine(LoadSceneRoutine(SceneLoader.TargetSceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        fadeCanvas.gameObject.SetActive(true);
        deathlessObjects.SetActive(true);
        yield return Fade(0f, 1f);

        float timer = 0f;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        while (timer < minLoadTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        op.allowSceneActivation = true;
        while (!op.isDone)
            yield return null;

        // Удаляем текущую сцену (если это не LoadingScene)
        Scene current = SceneManager.GetActiveScene();
        if (current.name != "LoadingScene")
        {
            SceneManager.UnloadSceneAsync(current);
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        CleanupAudioListeners();
        CleanupDuplicateEventSystems();

        yield return Fade(1f, 0f);
        fadeCanvas.gameObject.SetActive(false);
        deathlessObjects.SetActive(false);
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = to;
    }

    private void CleanupAudioListeners()
    {
        var listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        bool keepOne = false;
        foreach (var listener in listeners)
        {
            if (!keepOne)
            {
                keepOne = true;
                continue;
            }

            listener.enabled = false;
        }
    }

    private void CleanupDuplicateEventSystems()
    {
        var eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        bool keepOne = false;
        foreach (var es in eventSystems)
        {
            if (!keepOne)
            {
                keepOne = true;
                continue;
            }

            Destroy(es.gameObject);
        }
    }
}
