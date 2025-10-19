using UnityEngine;

[ExecuteAlways]
public class IglooBuilder : MonoBehaviour
{
    [Header("References")]
    public Transform buildZoneCenter;
    public PlayerInventory playerInventory;
    public GameObject iceBlockPrefab;
    public GameObject iglooPrefab;
    public ZoneVisualizer zoneVisualizer;

    [Header("Ghost Preview")]
    public GameObject ghostBlockPrefab; // semi-transparent block prefab
    private GameObject ghostBlockInstance;

    [Header("Settings")]
    public float buildZoneRadius = 10f;
    public int totalBlocksNeeded = 6;
    public float blockSpacing = 1.0f;
    public KeyCode placeKey = KeyCode.E;

    private int blocksPlaced = 0;
    private bool isInsideBuildZone = false;
    private bool iglooBuilt = false;

    private void Update()
    {
        if (zoneVisualizer != null)
        {
            buildZoneRadius = zoneVisualizer.innermostRadius;
        }

        if (iglooBuilt || playerInventory == null) return;

        GameObject player = GameObject.FindWithTag("Player");
        if (!player) return;

        float distance = Vector3.Distance(player.transform.position, buildZoneCenter.position);
        isInsideBuildZone = distance <= buildZoneRadius;

        if (isInsideBuildZone)
        {
            ShowGhostBlock();
            if (Input.GetKeyDown(placeKey))
                TryPlaceBlock(player.transform);
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

        Vector3 offset = Quaternion.Euler(0, blocksPlaced * (360f / totalBlocksNeeded), 0) * Vector3.forward * blockSpacing;
        ghostBlockInstance.transform.position = buildZoneCenter.position + offset + Vector3.up * 0.2f;
        ghostBlockInstance.transform.rotation = Quaternion.identity;
        ghostBlockInstance.SetActive(true);
    }

    private void HideGhostBlock()
    {
        if (ghostBlockInstance != null)
            ghostBlockInstance.SetActive(false);
    }
    #endregion

    private void TryPlaceBlock(Transform player)
    {
        if (playerInventory.iceCount <= 0)
        {
            Debug.Log("No ice blocks to place!");
            return;
        }

        Vector3 offset = Quaternion.Euler(0, blocksPlaced * (360f / totalBlocksNeeded), 0) * Vector3.forward * blockSpacing;
        Vector3 spawnPos = buildZoneCenter.position + offset;

        GameObject block = Instantiate(iceBlockPrefab, spawnPos + Vector3.up * 0.2f, Quaternion.identity, transform);

        blocksPlaced++;
        playerInventory.iceCount--;
        playerInventory.NotifyInventoryChanged();

        Debug.Log($"Placed ice block {blocksPlaced}/{totalBlocksNeeded}");

        if (blocksPlaced >= totalBlocksNeeded)
        {
            BuildFinalIgloo();
        }
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
        Gizmos.DrawWireSphere(buildZoneCenter.position, buildZoneRadius);
    }
}
