using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameplaySceneName = "SampleScene"; // For Restart
    [SerializeField] private string startMenuSceneName = "StartMenu";  // For Quit

    // Restart the game (reloads gameplay scene)
    public void RestartGame()
    {
        if (IsSceneInBuild(gameplaySceneName))
        {
            SceneManager.LoadScene(gameplaySceneName);
        }
        else
        {
            Debug.LogError($"Scene '{gameplaySceneName}' is not in Build Settings. " +
                           $"Please add it via File → Build Settings → Add Open Scenes.");
        }
    }

    // Quit button now returns to the Start Menu instead of closing the app
    public void QuitGame()
    {
        if (IsSceneInBuild(startMenuSceneName))
        {
            SceneManager.LoadScene(startMenuSceneName);
        }
        else
        {
            Debug.LogError($"Scene '{startMenuSceneName}' is not in Build Settings. " +
                           $"Please add it via File → Build Settings → Add Open Scenes.");
        }
    }

    /// <summary>
    /// Checks if a given scene name exists in the Build Settings.
    /// </summary>
    private bool IsSceneInBuild(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
                return true;
        }
        return false;
    }
}