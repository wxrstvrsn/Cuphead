using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static string TargetSceneName;

    public static void Load(string sceneName)
    {
        TargetSceneName = sceneName;
        SceneManager.LoadScene("Loading Scene");
    }
}