using UnityEngine;

public static class SceneLoader
{
    public static string TargetSceneName;

    /// <summary>
    /// Загружаем любую сцену через прокладку LoadingScene
    /// </summary>
    public static void Load(string targetScene)
    {
        TargetSceneName = targetScene;

        if (LoadingSystem.Instance != null)
        {
            LoadingSystem.Instance.LoadScene("Loading Scene");
        }
        else
        {
            Debug.LogError("LoadingSystem.Instance is NULL. Did you start from LoadingScene?");
        }
    }
}