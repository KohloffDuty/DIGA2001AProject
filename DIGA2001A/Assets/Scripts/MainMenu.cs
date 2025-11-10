using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject tutorialPanel;
    public GameObject backstoryPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void ShowBackstory()
    {
        backstoryPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    public void CloseBackstory()
    {
        backstoryPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
