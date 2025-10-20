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
    public int totalBlocksNeeded = 6;         // number of blocks to place
    public float blockSpacing = 1f;
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
        if (zoneVisualizer != null && buildZoneCenter != null)
        {
            // Keep the build radius synced with innermost zone
            buildZoneRadius = zoneVisualizer.innermostRadius;
        }

        if (iglooBuilt || playerInventory == null || buildZoneCenter == null)
            return;

        GameObject player = GameObject.FindWithTag("Player");
        if (!player) return;

        float distance = Vector3.Distance(player.transform.position, buildZoneCenter.position);
        isInsideBuildZone = distance <= buildZoneRadius;

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

        // Ensure only one instance exists
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
        int index = blocksPlaced % totalBlocksNeeded; // circular layout
        float angleStep = 360f / totalBlocksNeeded;
    
        // Calculate XZ position around center
        float angle = index * angleStep * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)) * blockSpacing;
        Vector3 basePos = buildZoneCenter.position + offset;

        // Raycast down from above to find ground height
        Ray ray = new Ray(basePos + Vector3.up * 5f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            basePos.y = hit.point.y + 0.05f; // slightly above ground
        }
        else
        {
            basePos.y = buildZoneCenter.position.y; // fallback to center height
        }

        return basePos;
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

        Debug.Log($"Placed ice block {blocksPlaced}/{totalBlocksNeeded}");

        if (blocksPlaced >= totalBlocksNeeded)
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
