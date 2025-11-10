using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildManager : MonoBehaviour
{
    [Header("Prefabs & Spawn points")]
    public GameObject firePrefab;
    public Transform fireSpawnPoint;

    [Header("UI")]
    public Button fireButton;
    public InventoryUI inventoryUI;         // reference to your InventoryUI script
    public TMPro.TextMeshProUGUI fireStatusText;

    [Header("Fire Settings")]
    public float fireDuration = 60f;

    private PlayerInventory inventory;
    private GameObject currentFire;

    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) Debug.LogError("Player not found (tag 'Player').");

        inventory = player.GetComponent<PlayerInventory>();
        if (inventory == null) Debug.LogError("PlayerInventory missing on Player.");
        else inventory.OnInventoryChanged += UpdateUI;

        UpdateUI();

        if (fireButton != null) fireButton.onClick.AddListener(StartFire);

        if (fireStatusText != null)
            fireStatusText.text = "";
    }

    void UpdateUI()
    {
        if (inventory == null || inventoryUI == null) return;

        inventoryUI.UpdateWood(inventory.woodCount);

        if (fireButton != null)
            fireButton.interactable = inventory.CanBuildFire() && currentFire == null;
    }

    public void StartFire()
    {
        if (inventory == null || !inventory.CanBuildFire() || currentFire != null) return;

        inventory.UseWoodForFire();
        UpdateUI();

        currentFire = Instantiate(firePrefab, fireSpawnPoint.position, Quaternion.identity);

        // Notify EndManager that fire has been built
        FindObjectOfType<EndManager>()?.SetFireBuilt(true);

        if (fireStatusText != null)
            StartCoroutine(FireCountdown(fireDuration));

        if (fireDuration > 0f)
            StartCoroutine(ExtinguishAfter(currentFire, fireDuration));
    }

    IEnumerator ExtinguishAfter(GameObject go, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (go != null) Destroy(go);
        currentFire = null;
        UpdateUI();

        if (fireStatusText != null)
            fireStatusText.text = "Fire has gone out.";
    }

    IEnumerator FireCountdown(float seconds)
    {
        float timeLeft = seconds;
        while (timeLeft > 0f)
        {
            if (fireStatusText != null)
                fireStatusText.text = $"Fire is burning: {Mathf.CeilToInt(timeLeft)}s left";
            timeLeft -= 1f;
            yield return new WaitForSeconds(1f);
        }
        if (fireStatusText != null)
            fireStatusText.text = "";
    }
}
