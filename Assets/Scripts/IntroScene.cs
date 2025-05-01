using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private bool _isLoading = false;

    void Update()
    {
        if (!_isLoading && Input.anyKeyDown && !IsMouseInput())
        {
            _isLoading = true;
            SceneLoader.Load("Level Select");
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