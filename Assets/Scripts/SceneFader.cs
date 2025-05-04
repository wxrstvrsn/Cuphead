using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class SceneFader : MonoBehaviour
{
    [Header("Overlay CanvasGroup (fullscreen black)")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Transition Settings")]   
    [Tooltip("Scene name as specified in Build Settings")]  
    [SerializeField] private string nextSceneName;

    [Tooltip("Duration of fade in/out in seconds")]            
    [SerializeField] private float fadeDuration = 1f;
    [Tooltip("Minimum time to display black screen after fade out")]            
    [SerializeField] private float minBlackTime = 0.5f;
    [Tooltip("Delay after fade in before accepting new transitions")]            
    [SerializeField] private float postFadeDelay = 0.3f;

    private bool isTransitioning;
    
    public event System.Action OnTransitionComplete;

    private void Awake()
    {
        // Cache CanvasGroup and initialize fully opaque overlay
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
    }

    private void Start()
    {
        // Initial fade-in sequence when this scene starts
        StartCoroutine(InitialFadeIn());
    }

    /// <summary>
    /// Public method to trigger transition to another scene.
    /// </summary>
    public void StartTransition(string targetScene = null)
    {
        if (isTransitioning) return;
        if (!string.IsNullOrEmpty(targetScene))
            nextSceneName = targetScene;

        StartCoroutine(TransitionSequence());
    }

    private IEnumerator InitialFadeIn()
    {
        isTransitioning = true;

        // Fade from black to transparent
        yield return Fade(1f, 0f, fadeDuration);

        // Hide overlay and wait a moment
        canvasGroup.gameObject.SetActive(false);
        yield return new WaitForSeconds(postFadeDelay);

        isTransitioning = false;
    }

    private IEnumerator TransitionSequence()
    {
        isTransitioning = true;

        // Show overlay and fade to black
        canvasGroup.gameObject.SetActive(true);
        yield return Fade(0f, 1f, fadeDuration);

        // Begin async load and prevent auto-activation
        var loadOp = SceneManager.LoadSceneAsync(nextSceneName);
        loadOp.allowSceneActivation = false;

        // Wait for both minimum black time and load completion
        float timer = 0f;
        while (timer < minBlackTime || loadOp.progress < 0.9f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Activate scene
        loadOp.allowSceneActivation = true;
        yield return new WaitUntil(() => loadOp.isDone);

        // Fade back in
        yield return Fade(1f, 0f, fadeDuration);
        OnTransitionComplete?.Invoke();

        // Hide overlay and delay
        canvasGroup.gameObject.SetActive(false);
        yield return new WaitForSeconds(postFadeDelay);

        isTransitioning = false;
    }

    /// <summary>
    /// Generic fade coroutine: interpolates canvasGroup.alpha.
    /// </summary>
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
