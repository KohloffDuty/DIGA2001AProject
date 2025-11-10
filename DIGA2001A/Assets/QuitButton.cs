using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator quitAnimator;      // Assign your Animator in the Inspector
    public string quitAnimationName;   // The name of the quit animation trigger
    public float quitDelay = 1.5f;     // Time to wait before quitting (match animation length)

    private bool isQuitting = false;

    // This function can be linked to your button’s OnClick event
    public void QuitGame()
    {
        if (isQuitting) return; // Prevent double clicks
        isQuitting = true;

        if (quitAnimator != null && !string.IsNullOrEmpty(quitAnimationName))
        {
            quitAnimator.SetTrigger(quitAnimationName);
            Invoke(nameof(ExitGame), quitDelay);
        }
        else
        {
            ExitGame();
        }
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        // Stop play mode if running in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the actual application
        Application.Quit();
#endif
    }
}
