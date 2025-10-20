using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject tutorialPanel;
    public GameObject backstoryPanel;

    private void Start()
    {
        tutorialPanel.SetActive(false);
        backstoryPanel.SetActive(false);

        var fader = GetComponent<UIFader>();
        if (fader != null)
            fader.FadeIn(1.5f);
    }

    public void PlayGame()
    {
        Debug.Log("Start Game clicked!");
        SceneManager.LoadScene("SampleScene");
    }

    public void ShowTutorial() => tutorialPanel?.SetActive(true);
    public void CloseTutorial() => tutorialPanel?.SetActive(false);

    public void ShowBackstory() => backstoryPanel?.SetActive(true);
    public void CloseBackstory() => backstoryPanel?.SetActive(false);

    public void QuitGame()
    {
        Debug.Log("Quit clicked!");
        Application.Quit();
    }
        
 }

