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
    public int totalBlocksNeeded = 6;
    public float blockSpacing = 1f;
    public KeyCode placeKey = KeyCode.E;

    [Header("Status Properties")]
    public bool IsInsideBuildZone { get; private set; } = false;
    public bool IglooBuilt { get; private set; } = false;
    public int BlocksPlaced { get; private set; } = 0;

    private Vector3[] blockPositions;
    private float buildRadius;
    private GameObject[] placedBlocks;

    private void Start()
    {
        buildRadius = zoneVisualizer ? zoneVisualizer.innermostRadius : 10f;
        blockPositions = new Vector3[totalBlocksNeeded];
        placedBlocks = new GameObject[totalBlocksNeeded];
        CalculateBlockPositions();
    }

    private void Update()
    {
        if (iglooPrefab == null || playerInventory == null || buildZoneCenter == null)
            return;

        GameObject player = GameObject.FindWithTag("Player");
        if (!player) return;

        buildRadius = zoneVisualizer ? zoneVisualizer.innermostRadius : 10f;
        float distance = Vector3.Distance(player.transform.position, buildZoneCenter.position);
        IsInsideBuildZone = distance <= buildRadius;

        if (IglooBuilt) 
        {
            HideGhostBlock();
            return;
        }

        if (IsInsideBuildZone)
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
        if (ghostBlockPrefab == null) return;

        if (ghostBlockInstance == null)
        {
            ghostBlockInstance = Instantiate(ghostBlockPrefab, Vector3.zero, Quaternion.identity, transform);
        }

        if (BlocksPlaced < totalBlocksNeeded)
        {
            ghostBlockInstance.transform.position = blockPositions[BlocksPlaced];
            ghostBlockInstance.transform.rotation = Quaternion.identity;
            ghostBlockInstance.SetActive(true);
        }
        else
        {
            ghostBlockInstance.SetActive(false);
        }
    }

    private void HideGhostBlock()
    {
        if (ghostBlockInstance != null)
            ghostBlockInstance.SetActive(false);
    }
    #endregion

    private void CalculateBlockPositions()
    {
        float angleStep = 360f / totalBlocksNeeded;

        for (int i = 0; i < totalBlocksNeeded; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)) * blockSpacing;
            Vector3 pos = buildZoneCenter.position + offset;

            // Raycast down to snap to ground
            Ray ray = new Ray(pos + Vector3.up * 5f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                pos.y = hit.point.y + 0.05f;
            }
            else
            {
                pos.y = buildZoneCenter.position.y;
            }

            blockPositions[i] = pos;
        }
    }

    private void TryPlaceBlock()
    {
        if (playerInventory.iceCount <= 0)
        {
            Debug.Log("No ice blocks to place!");
            return;
        }

        if (BlocksPlaced >= totalBlocksNeeded)
            return;

        GameObject block = Instantiate(iceBlockPrefab, blockPositions[BlocksPlaced], Quaternion.identity, transform);
        block.GetComponent<Collider>().isTrigger = false; // prevent pickup
        placedBlocks[BlocksPlaced] = block;

        BlocksPlaced++;
        playerInventory.iceCount--;
        playerInventory.NotifyInventoryChanged();

        Debug.Log($"Placed ice block {BlocksPlaced}/{totalBlocksNeeded}");

        if (BlocksPlaced >= totalBlocksNeeded)
            BuildFinalIgloo();
    }

    private void BuildFinalIgloo()
    {
        IglooBuilt = true;

        // Clean up all ghost/placed blocks
        if (ghostBlockInstance != null)
            Destroy(ghostBlockInstance);

        foreach (var block in placedBlocks)
        {
            if (block != null)
                Destroy(block);
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
