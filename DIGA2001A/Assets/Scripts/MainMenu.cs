using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [Header("UI Panels")]
    public GameObject tutorialPanel;
    public GameObject backstoryPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (backstoryPanel != null)
            tutorialPanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
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
        Application.Quit();
    }
}