using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Leve Select"; // Название следующей сцены
    [SerializeField] private SceneFader sceneFader;

    private bool _isLoading = false;

    private void Start()
    {
        AudioManager.Instance.PlayMusic("MUS_Intro");
    }

    void Update()
    {
        if (!_isLoading && Input.anyKeyDown && !IsMouseInput())
        {
            _isLoading = true;
            sceneFader.StartTransition(nextSceneName);
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