using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject tutorialPanel;
    public GameObject backstoryPanel;

    public void PlayGame()
    {
        GameManager.Instance.StartGame();   // delegate to GameManager
    }

    public void ShowTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    public void ShowBackstory()
    {
        if (backstoryPanel != null)
        {
            backstoryPanel.SetActive(true);
            var fader = backstoryPanel.GetComponent<UIFader>();
            if (fader != null)
                fader.FadeIn(2f);
        }
    }

    public void CloseBackstory()
    {
        if (backstoryPanel != null)
        {
            var fader = backstoryPanel.GetComponent<UIFader>();
            if (fader != null)
                fader.FadeOut(1f);
            else
                backstoryPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        GameManager.Instance.QuitGame();
    }
}
