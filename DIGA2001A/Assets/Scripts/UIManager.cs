using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject firePanel;      // panel with fire button
    public GameObject shelterPanel;   // panel with shelter button
    public GameObject iglooPanel;     // new panel for igloo building progress

    [Header("UI Texts (TMP)")]
    public TMP_Text woodText;
    public TMP_Text iceText;
    public TMP_Text iglooText;        // shows blocks placed / total

    private PlayerInventory inventory;
    private IglooBuilder iglooBuilder;

    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (!player)
        {
            Debug.LogError("Player not found (tag 'Player')");
            return;
        }

        inventory = player.GetComponent<PlayerInventory>();
        iglooBuilder = FindObjectOfType<IglooBuilder>();

        if (inventory == null)
            Debug.LogError("PlayerInventory missing on Player.");

        if (iglooBuilder == null)
            Debug.LogWarning("IglooBuilder not found in scene.");

        // Subscribe to inventory changes
        if (inventory != null)
            inventory.OnInventoryChanged += UpdateUI;

        // Initialize UI
        UpdateUI();

        if (firePanel != null) firePanel.SetActive(false);
        if (shelterPanel != null) shelterPanel.SetActive(false);
        if (iglooPanel != null) iglooPanel.SetActive(false);
    }

    void UpdateUI()
    {
        if (inventory != null)
        {
            if (woodText != null)
                woodText.text = $"Wood: {inventory.woodCount}/{inventory.woodNeededForFire}";

            if (iceText != null)
                iceText.text = $"Ice: {inventory.iceCount}/{inventory.iceNeededForShelter}";

            if (firePanel != null)
                firePanel.SetActive(inventory.CanBuildFire());

            if (shelterPanel != null)
                shelterPanel.SetActive(inventory.CanBuildShelter());
        }

        // Update igloo building UI using public getters
        if (iglooBuilder != null && iglooText != null && iglooPanel != null)
        {
            iglooText.text = $"Igloo: {iglooBuilder.BlocksPlaced}/{iglooBuilder.TotalBlocksNeeded}";
            iglooPanel.SetActive(iglooBuilder.IsInsideBuildZone && !iglooBuilder.IglooBuilt);
        }
    }

    void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= UpdateUI;
    }
}
