using UnityEngine;

[ExecuteAlways]
public class IglooBuilder : MonoBehaviour
{
    [Header("References")]
    public float buildZoneRadius = 60f;
    
    public Transform buildZoneCenter;
    public PlayerInventory playerInventory;
    public GameObject iceBlockPrefab;
    public GameObject iglooPrefab;
    public ZoneVisualizer zoneVisualizer;

    [Header("Ghost Preview")]
    public GameObject ghostBlockPrefab; // semi-transparent block prefab
    private GameObject ghostBlockInstance;

    [Header("Settings")]
    public int totalBlocksNeeded = 6;         // blocks per layer
    public int totalLayers = 2;               // number of stacked layers
    public float blockSpacing = 1f;
    public float layerHeight = 0.5f;
    public KeyCode placeKey = KeyCode.E;

    private int blocksPlaced = 0;
    private bool isInsideBuildZone = false;
    private bool iglooBuilt = false;
    
    public int BlocksPlaced => blocksPlaced;
    public int TotalBlocksNeeded => totalBlocksNeeded;
    public bool IsInsideBuildZone => isInsideBuildZone;
    public bool IglooBuilt => iglooBuilt;


    private void Update()
    {
        if (zoneVisualizer != null)
        {
            // Keep the build radius synced with innermost zone
            if (buildZoneCenter != null)
                buildZoneRadius = zoneVisualizer.innermostRadius;
        }

        if (iglooBuilt || playerInventory == null || buildZoneCenter == null)
            return;

        GameObject player = GameObject.FindWithTag("Player");
        if (!player) return;

        float distance = Vector3.Distance(player.transform.position, buildZoneCenter.position);
        isInsideBuildZone = distance <= zoneVisualizer.innermostRadius;

        if (isInsideBuildZone)
        {
            ShowGhostBlock();
            if (Input.GetKeyDown(placeKey))
                TryPlaceBlock();
        }
        else
        {
            HideGhostBlock();
        }
    }

    #region Ghost Preview
    private void ShowGhostBlock()
    {
        if (!ghostBlockPrefab) return;

        if (ghostBlockInstance == null)
        {
            ghostBlockInstance = Instantiate(ghostBlockPrefab, Vector3.zero, Quaternion.identity, transform);
        }

        Vector3 spawnPos = GetNextBlockPosition();
        ghostBlockInstance.transform.position = spawnPos;
        ghostBlockInstance.transform.rotation = Quaternion.identity;
        ghostBlockInstance.SetActive(true);
    }

    private void HideGhostBlock()
    {
        if (ghostBlockInstance != null)
            ghostBlockInstance.SetActive(false);
    }
    #endregion

    private Vector3 GetNextBlockPosition()
    {
        int currentLayer = blocksPlaced / totalBlocksNeeded;
        int indexInLayer = blocksPlaced % totalBlocksNeeded;

        float angleStep = 360f / totalBlocksNeeded;
        Vector3 offset = Quaternion.Euler(0, indexInLayer * angleStep, 0) * Vector3.forward * blockSpacing;
        Vector3 rawPos = buildZoneCenter.position + offset + Vector3.up * (currentLayer * layerHeight + 0.2f);

        // Ground alignment
        if (Physics.Raycast(rawPos + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f))
        {
            rawPos.y = hit.point.y + currentLayer * layerHeight + 0.2f;
        }

        return rawPos;
    }

    private void TryPlaceBlock()
    {
        if (playerInventory.iceCount <= 0)
        {
            Debug.Log("No ice blocks to place!");
            return;
        }

        Vector3 spawnPos = GetNextBlockPosition();
        Instantiate(iceBlockPrefab, spawnPos, Quaternion.identity, transform);

        blocksPlaced++;
        playerInventory.iceCount--;
        playerInventory.NotifyInventoryChanged();

        Debug.Log($"Placed ice block {blocksPlaced}/{totalBlocksNeeded * totalLayers}");

        if (blocksPlaced >= totalBlocksNeeded * totalLayers)
            BuildFinalIgloo();
    }

    private void BuildFinalIgloo()
    {
        iglooBuilt = true;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Instantiate(iglooPrefab, buildZoneCenter.position, Quaternion.identity);
        Debug.Log("Igloo built! You can now start a fire!");
    }

    private void OnDrawGizmosSelected()
    {
        if (buildZoneCenter == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(buildZoneCenter.position, zoneVisualizer ? zoneVisualizer.innermostRadius : 10f);
    }
}
