using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [Header("UI Panels")]
    public GameObject tutorialPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ShowTutorial()
    {
        if (!tutorialPanel != null)
            tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
