using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelStartHandler : MonoBehaviour
{
    [Header("Scale Settings")] [SerializeField]
    private float scaleDuration = 0.3f;

    [SerializeField] private Vector3 targetScale = Vector3.one;
    [SerializeField] private GameObject blackPanel;
    
    [SerializeField] TransitionController transition;

    [Header("Level To Start")]
    private string levelName;
    
    private string soundtrack;

    private Coroutine _scaleCoroutine;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
        blackPanel.SetActive(false);
    }

    public void Activate(string _levelName, string _soundtrack)
    {
        levelName = _levelName;
        soundtrack = _soundtrack;
        blackPanel.SetActive(true);
        gameObject.SetActive(true);

        if (_scaleCoroutine != null)
            StopCoroutine(_scaleCoroutine);

        _scaleCoroutine = StartCoroutine(ScaleTo(targetScale));
    }

    public void Deactivate()
    {
        blackPanel.SetActive(false);
        if (_scaleCoroutine != null)
            StopCoroutine(_scaleCoroutine);

        _scaleCoroutine = StartCoroutine(ScaleTo(Vector3.zero, () => gameObject.SetActive(false)));
    }

    private IEnumerator ScaleTo(Vector3 target, System.Action onComplete = null)
    {
        float timer = 0f;
        Vector3 startScale = transform.localScale;

        while (timer < scaleDuration)
        {
            float t = timer / scaleDuration;
            transform.localScale = Vector3.Lerp(startScale, target, t);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = target;
        onComplete?.Invoke();
    }

    public void PlayLevel()
    {
        print("[LevelStartHandler] PlayLevel CLICKED");
        transition.StartTransitionOut(levelName);
        print("[LevelStartHandler] TRANSITION STARTED");
        AudioManager.Instance.PlayMusic(soundtrack); 
    }
}