using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionController : MonoBehaviour
{
    [Header("Animation References")] public Animator circleAnimator; // Animator на CircleMask
    public Image hourglassImage; // UI Image песочных часов

    [Header("Animator Triggers")] public string contractTrigger = "Contract";
    public string expandTrigger = "Expand";

    [Header("Timing (сек)")] public float minBlackTime = 3.0f; // минимум чёрного после Contract
    public float hourglassFade = 0.3f; // fade-out часов перед Expand
    
    
    private bool isTransitioning = false;
    
    public bool IsTransitioning => isTransitioning;
    
    private void Start()
    {
        print("START() SUMMONED");
        // Авто PhaseIn при старте сцены
        // (предполагаем, что CircleMask size в префабе – 0,0 и Background+Hourglass активны)
        StartCoroutine(PhaseIn());
    }

    // Для PhaseOut вызываем извне
    public void StartTransitionOut(string nextSceneName)
    {
        print("[TransitionController] START TRANSITION SUMMONED");
        if (isTransitioning) return;
        print("[TransitionController] SET ACTIVE true");
        gameObject.SetActive(true); // Костыль?? TODO
        // Запускаем контракт (сжатие круга → чёрный)
        StartCoroutine(PhaseOut(nextSceneName));
    }

    // PhaseOut: а) Contract circle, б) ждём minBlackTime, в) LoadScene
    private IEnumerator PhaseOut(string nextSceneName)
    {
        print("[TransitionController] PHASEOUT SUMMONED");
        isTransitioning = true;
        // 1) Запустить анимацию Contract
        circleAnimator.SetTrigger(contractTrigger);
        float durContract = GetClipLength(circleAnimator, contractTrigger);
        yield return new WaitForSeconds(durContract);

        // 2) Ждём заданное время на полном чёрном
        yield return new WaitForSeconds(minBlackTime);

        print("[TransitionController] SMOOTH HOURGLASS");
        // 3) Плавно уходим часики (optional)
        if (hourglassImage)
        {
            yield return StartCoroutine(FadeImage(hourglassImage, 1f, 0f, hourglassFade));
            hourglassImage.gameObject.SetActive(false);
        }

        // 4) Загружаем следующую сцену (Single), убивая этот Transition
        print($"[TransitionController] LOADING {nextSceneName}");
        isTransitioning = false;
        SceneManager.LoadScene(nextSceneName);
    }

    // PhaseIn: запускается автоматически в Start() новой сцены
    private IEnumerator PhaseIn()
    {
        print("[TransitionController] PHASE_IN SUMMONED");
        isTransitioning = true;
        // Убедимся, что часы видимы и circleMask уже в size=(0,0)
        // 1) Ждём минимальное время на чёрном
        yield return new WaitForSeconds(minBlackTime);

        // 2) Fade-out часов
        print("[TransitionController] HOURGLASS FADE");
        if (hourglassImage)
        {
            yield return StartCoroutine(FadeImage(hourglassImage, 1f, 0f, hourglassFade));
            hourglassImage.gameObject.SetActive(false);
        }

        // 3) Запустить Expand
        print("[TransitionController] EXPANDING");
        circleAnimator.SetTrigger(expandTrigger);
        float durExpand = GetClipLength(circleAnimator, expandTrigger);
        yield return new WaitForSeconds(durExpand);

        print("[TransitionController] KILL TRANSITION");
        // 4) Всё – скрываем Transition
        isTransitioning = false;
        gameObject.SetActive(false);
    }

    // Вспомогательное: узнать длительность клипа по триггеру
    private float GetClipLength(Animator anim, string trigger)
    {
        foreach (var clip in anim.runtimeAnimatorController.animationClips)
            if (clip.name.ToLower().Contains(trigger.ToLower()))
                return clip.length;
        return 1f; // fallback
    }

    // Плавный fade для UI Image
    private IEnumerator FadeImage(Image img, float from, float to, float duration)
    {
        float t = 0f;
        Color c = img.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / duration);
            img.color = c;
            yield return null;
        }
        c.a = to;
        img.color = c;
    }
}