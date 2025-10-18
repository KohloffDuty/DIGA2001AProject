using UnityEngine;

public class IglooBuilder : MonoBehaviour
{
    [Header("References")]
    public Transform buildZoneCenter; 
    public PlayerInventory playerInventory;
    public GameObject iceBlockPrefab;
    public GameObject iglooPrefab;

    [Header("Settings")]
    public float buildZoneRadius = 10f;
    public int totalBlocksNeeded = 6;
    public float blockSpacing = 1.0f;
    public KeyCode placeKey = KeyCode.E; // key to place a block

    private int blocksPlaced = 0;
    private bool isInsideBuildZone = false;
    private bool iglooBuilt = false;

    private void Update()
    {
        if (iglooBuilt || playerInventory == null) return;

        // Check player distance from the build zone
        GameObject player = GameObject.FindWithTag("Player");
        if (!player) return;

        float distance = Vector3.Distance(player.transform.position, buildZoneCenter.position);
        isInsideBuildZone = distance <= buildZoneRadius;

        if (isInsideBuildZone && Input.GetKeyDown(placeKey))
        {
            TryPlaceBlock(player.transform);
        }
    }

    private void TryPlaceBlock(Transform player)
    {
        if (playerInventory.iceCount <= 0)
        {
            Debug.Log("No ice blocks to place!");
            return;
        }

        // Calculate block position (circle around center)
        Vector3 offset = Quaternion.Euler(0, blocksPlaced * (360f / totalBlocksNeeded), 0) * Vector3.forward * blockSpacing;
        Vector3 spawnPos = buildZoneCenter.position + offset;

        // Spawn block slightly above the ground
        GameObject block = Instantiate(iceBlockPrefab, spawnPos + Vector3.up * 0.2f, Quaternion.identity);
        block.transform.SetParent(transform);

        blocksPlaced++;
        playerInventory.iceCount--;
        playerInventory.OnInventoryChanged?.Invoke();

        Debug.Log($"Placed ice block {blocksPlaced}/{totalBlocksNeeded}");

        if (blocksPlaced >= totalBlocksNeeded)
        {
            BuildFinalIgloo();
        }
    }

    private void BuildFinalIgloo()
    {
        iglooBuilt = true;

        // Destroy temp blocks
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Spawn final igloo
        Instantiate(iglooPrefab, buildZoneCenter.position, Quaternion.identity);

        Debug.Log("Igloo built! You can now start a fire!");
    }

    private void OnDrawGizmosSelected()
    {
        if (buildZoneCenter == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(buildZoneCenter.position, buildZoneRadius);
    }
}
