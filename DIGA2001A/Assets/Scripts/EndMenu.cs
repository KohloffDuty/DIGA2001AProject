using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("Restartinf game...");
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private void Start()
    {
        var fader = GetComponent<UIFader>();
        if (fader != null)
            fader.FadeIn(1.5f);
    }
}
  
