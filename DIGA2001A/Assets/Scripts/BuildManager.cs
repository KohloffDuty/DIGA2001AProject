using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildManager : MonoBehaviour
{
    [Header("Prefabs & Spawn points")]
    public GameObject firePrefab;         // assign a prefab that has FireArea trigger and visuals
    public Transform fireSpawnPoint;      // where fire will appear

    public GameObject shelterPrefab;      // assign a prefab that has ShelterArea trigger
    public Transform shelterSpawnPoint;   // where shelter will appear

    [Header("UI")]
    public Text woodText;                 // UI text to show wood count
    public Text iceText;                  // UI text to show ice count
    public Button fireButton;             // Start Fire button
    public Button shelterButton;          // Create Shelter button

    [Header("Fire Settings")]
    public float fireDuration = 60f;      // auto-extinguish after X seconds (optional)

    private PlayerInventory inventory;
    private PlayerHealth playerHealth;

    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) Debug.LogError("Player not found (tag 'Player').");

        inventory = player.GetComponent<PlayerInventory>();
        playerHealth = player.GetComponent<PlayerHealth>();

        if (inventory == null) Debug.LogError("PlayerInventory missing on Player.");
        else inventory.OnInventoryChanged += UpdateUI;

        UpdateUI();

        if (fireButton != null) fireButton.onClick.AddListener(StartFire);
        if (shelterButton != null) shelterButton.onClick.AddListener(CreateShelter);
    }

    void UpdateUI()
    {
        if (inventory == null) return;
        if (woodText != null) woodText.text = $"Wood: {inventory.woodCount}/{inventory.woodNeededForFire}";
        if (iceText != null) iceText.text = $"Ice: {inventory.iceCount}/{inventory.iceNeededForShelter}";
        if (fireButton != null) fireButton.interactable = inventory.CanBuildFire();
        if (shelterButton != null) shelterButton.interactable = inventory.CanBuildShelter();
    }

    public void StartFire()
    {
        if (inventory == null || !inventory.CanBuildFire()) return;

        inventory.UseWoodForFire();
        UpdateUI();

        var newFire = Instantiate(firePrefab, fireSpawnPoint.position, Quaternion.identity);
        // optional: if you want instant benefit regardless of position:
        // playerHealth.SetNearFire(true);

        if (fireDuration > 0f)
            StartCoroutine(ExtinguishAfter(newFire, fireDuration));
    }

    IEnumerator ExtinguishAfter(GameObject go, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (go != null) Destroy(go);
        // If you forced SetNearFire(true) above, you may want to SetNearFire(false) here.
    }

    public void CreateShelter()
    {
        if (inventory == null || !inventory.CanBuildShelter()) return;

        inventory.UseIceForShelter();
        UpdateUI();

        Instantiate(shelterPrefab, shelterSpawnPoint.position, Quaternion.identity);
        // Shelter effect is handled by ShelterArea on player entry
    }
}
