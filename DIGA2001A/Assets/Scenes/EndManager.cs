using UnityEngine;

public class EndManager : MonoBehaviour
{
    [Header("End Panels")]
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject FailurePanel;

    private bool iglooBuilt;
    private bool fireBuilt;

    // These will be called by your other systems
    public void SetIglooBuilt(bool built)
    {
        iglooBuilt = built;
        CheckEndCondition();
    }

    public void SetFireBuilt(bool built)
    {
        fireBuilt = built;
        CheckEndCondition();
    }

    private void CheckEndCondition()
    {
        // Example condition: both must be true to win
        if (iglooBuilt && fireBuilt)
        {
            ShowSuccess();
        }
        else if (!iglooBuilt && !fireBuilt)
        {
            // Optional: you can trigger failure after a timer, or from other logic
            // For now, just placeholder condition
        }
    }

    public void ShowSuccess()
    {
        WinPanel.SetActive(true);
        FailurePanel.SetActive(false);
        Debug.Log("✅ Player succeeded!");
    }

    public void ShowFailure()
    {
        WinPanel.SetActive(true);
        FailurePanel.SetActive(false);
        Debug.Log("❌ Player failed!");
    }
}