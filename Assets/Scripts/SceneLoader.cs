using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static string TargetSceneName = "Intro";

    public static void Load(string sceneName)
    {
        TargetSceneName = sceneName;
        SceneManager.LoadScene("Loading Scene");
    }
}