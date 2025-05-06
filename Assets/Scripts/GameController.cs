using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("UI References")] [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField] private GameObject blackPanel;
    [SerializeField] private TransitionController transition;

    [Header("Narrator Pause")] [SerializeField]
    private float narratorPause;

    [Header("Wallop_Ready")] [SerializeField]
    private GameObject wallopReady;

    private bool _isPaused;
    private string _currentSceneName;

    private void Awake()
    {
        _currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            StartCoroutine(PlayNarratorSequence());
        }
    }

    private IEnumerator PlayNarratorSequence()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.PlayMusic("MUS_ForestFollies");
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.PlayNarratorA();
        yield return new WaitForSeconds(narratorPause);
        wallopReady.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused) Resume();
            else Pause();
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

    /// <summary>
    /// Перезапустить текущий уровень через SceneFader
    /// </summary>
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        _isPaused = false;

        transition.StartTransitionOut(_currentSceneName);
    }

    /// <summary>
    /// Перейти в меню выбора уровня через SceneFader
    /// </summary>
    public void GoToLevelSelect()
    {
        Time.timeScale = 1f;
        _isPaused = false;

        transition.StartTransitionOut("Level Select");
        AudioManager.Instance.PlayMusic("MUS_Intro");
    }

    /// <summary>
    /// Выйти из игры (или остановить Play Mode в Editor)
    /// </summary>
    public void QuitGame()
    {
        Time.timeScale = 1f;
        _isPaused = false;

        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}