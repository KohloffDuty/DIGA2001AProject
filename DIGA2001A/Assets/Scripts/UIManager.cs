using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject firePanel;      // Shows if enough wood to build fire
    public GameObject inventoryPanel; // Optional: general inventory
    public GameObject iglooPanel;     // Shows igloo progress

    [Header("UI Texts")]
    public TMP_Text woodText;
    public TMP_Text iceText;
    public TMP_Text iglooText;

    private PlayerInventory inventory;
    private IglooBuilder iglooBuilder;

    void Start()
    {
        // Find player and components
        GameObject player = GameObject.FindWithTag("Player");
        if (!player)
        {
            Debug.LogError("Player not found. Make sure it is tagged 'Player'.");
            return;
        }

        inventory = player.GetComponent<PlayerInventory>();
        iglooBuilder = FindObjectOfType<IglooBuilder>();

        if (!inventory)
            Debug.LogError("PlayerInventory missing on Player.");
        if (!iglooBuilder)
            Debug.LogWarning("IglooBuilder not found in scene.");

        // Subscribe to inventory updates
        if (inventory != null)
            inventory.OnInventoryChanged += UpdateInventoryUI;

        // Initialize UI
        UpdateInventoryUI();
        UpdateIglooUI();

        if (firePanel) firePanel.SetActive(false);
        if (iglooPanel) iglooPanel.SetActive(false);
    }

    void Update()
    {
        // Dynamically update igloo panel and progress
        if (iglooBuilder && iglooPanel)
        {
            iglooPanel.SetActive(iglooBuilder.IsInsideBuildZone && !iglooBuilder.IglooBuilt);
            UpdateIglooUI();
        }
    }

    /// <summary>
    /// Updates inventory UI (wood/ice) and fire panel visibility
    /// </summary>
    void UpdateInventoryUI()
    {
        if (inventory == null) return;

        if (woodText) woodText.text = $"Wood: {inventory.woodCount}/{inventory.woodNeededForFire}";
        if (iceText) iceText.text = $"Ice: {inventory.iceCount}/{inventory.iceNeededForShelter}";

        if (firePanel)
            firePanel.SetActive(inventory.CanBuildFire());
    }

    /// <summary>
    /// Updates igloo progress text
    /// </summary>
    void UpdateIglooUI()
    {
        if (iglooBuilder == null || iglooText == null) return;

        iglooText.text = $"Igloo: {iglooBuilder.BlocksPlaced}/{iglooBuilder.totalBlocksNeeded}";


    }

    void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= UpdateInventoryUI;
    }
}
