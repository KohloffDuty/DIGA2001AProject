using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WarmthUI : MonoBehaviour
{
    [Header("References")]
    public Image warmthFill;          // The gradient bar
    public TMP_Text warmthText;       // Optional text label
    public PlayerHealth playerHealth; // Reference to health script (or your custom warmth manager)

    [Header("Warmth Settings")]
    public float maxWarmth = 100f;
    public float currentWarmth = 100f;

    void Update()
    {
        // Simple simulation – replace this with your actual warmth system
        // For now, it decreases slowly
        currentWarmth -= Time.deltaTime * 2f;
        currentWarmth = Mathf.Clamp(currentWarmth, 0f, maxWarmth);

        // Update bar fill
        if (warmthFill != null)
            warmthFill.fillAmount = currentWarmth / maxWarmth;

        // Update text
        if (warmthText != null)
            warmthText.text = $"{Mathf.CeilToInt(currentWarmth)}°";
    }

    // Optional function if you manage warmth elsewhere
    public void SetWarmth(float value)
    {
        currentWarmth = Mathf.Clamp(value, 0f, maxWarmth);
    }
}
