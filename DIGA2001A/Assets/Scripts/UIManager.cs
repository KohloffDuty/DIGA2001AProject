using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject firePanel;      // panel with fire button
    public GameObject shelterPanel;   // panel with shelter button

    [Header("UI Texts (TMP)")]
    public TMP_Text woodText;
    public TMP_Text iceText;

    private PlayerInventory inventory;

    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found (tag 'Player')");
            return;
        }

        inventory = player.GetComponent<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("PlayerInventory missing on Player.");
            return;
        }

        // Subscribe
        inventory.OnInventoryChanged += UpdateUI;

        // Initialize UI
        UpdateUI();

        if (firePanel != null) firePanel.SetActive(false);
        if (shelterPanel != null) shelterPanel.SetActive(false);
    }

    void UpdateUI()
    {
        if (inventory == null) return;

        if (woodText != null)
            woodText.text = $"Wood: {inventory.woodCount}/{inventory.woodNeededForFire}";

        if (iceText != null)
            iceText.text = $"Ice: {inventory.iceCount}/{inventory.iceNeededForShelter}";

        // Toggle panels individually
        if (firePanel != null) firePanel.SetActive(inventory.CanBuildFire());
        if (shelterPanel != null) shelterPanel.SetActive(inventory.CanBuildShelter());
    }

    void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= UpdateUI;
    }
}
