using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "LevelSelectScene"; // Название следующей сцены

    private bool _isLoading = false;

    void Update()
    {
        if (!_isLoading && Input.anyKeyDown)
        {
            _isLoading = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}