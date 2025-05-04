using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject blackPanel;
    [SerializeField] private SceneFader sceneFader;
    private bool _isPaused;

    private void Update()
    {
        if (!sceneFader.IsTransitioning && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Resume()
    {
        blackPanel.SetActive(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void Pause()
    {
        blackPanel.SetActive(true);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void RestartLevel()
    {
        if (sceneFader.IsTransitioning) return; // TODO переделать ебаный костыль
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLevelSelect()
    {
        if (sceneFader.IsTransitioning) return; // TODO переделать ебаный костыль
        
        Time.timeScale = 1f;
        sceneFader.StartTransition("Level Select");
        AudioManager.Instance.PlayMusic("MUS_Intro");
    }

    public void QuitGame()
    {
        if (sceneFader.IsTransitioning) return; // TODO переделать ебаный костыль
        
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}