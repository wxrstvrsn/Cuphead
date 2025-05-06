using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Leve Select"; // Название следующей сцены
    [SerializeField] private TransitionController transition;

    private bool _isLoading = false;

    private void Start()
    {
        AudioManager.Instance.PlayMusic("MUS_Intro_DontDealWithDevil_Vocal");
    }

    void Update()
    {
        if (Input.anyKeyDown && !IsMouseInput())
        {
            _isLoading = true;
            transition.StartTransitionOut(nextSceneName);
            AudioManager.Instance.PlayAmbient();
            AudioManager.Instance.currentSoundTrack = "MUS_Intro";
            // AudioManager.Instance.PlayMusic("MUS_Intro");
        }
    }

    private bool IsMouseInput()
    {
        // Проверяем все кнопки мыши (0 - левая, 1 - правая, 2 - средняя и т.д.)
        for (int i = 0; i < 3; i++)
        {
            if (Input.GetMouseButtonDown(i))
            {
                return true;
            }
        }

        return false;
    }
}