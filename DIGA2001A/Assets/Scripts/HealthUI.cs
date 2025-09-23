using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Image healthBarFill;
    public TMP_Text healthText;

    private void Update()
    {
        if (playerHealth == null) return;

        float fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
        healthBarFill.fillAmount = fillAmount;

        healthText.text = $"{Mathf.Ceil(playerHealth.currentHealth)} / {playerHealth.maxHealth}";
    }
}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
