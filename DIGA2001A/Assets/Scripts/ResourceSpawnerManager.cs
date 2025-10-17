using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ResourceSpawnerManager : MonoBehaviour
{
    [Header("Zone Visualizer Reference")]
    public ZoneVisualizer zoneVisualizer;

    [Header("Prefabs")]
    public GameObject Wood;
    public GameObject Fish;
    public GameObject IceBlock;
    public GameObject Wolf;
    public GameObject Mountain;

    [Header("Spawn Settings")]
    [Range(5, 10)] public int outerZoneMinCount = 5;
    [Range(5, 10)] public int outerZoneMaxCount = 10;
    [Range(5, 10)] public int innerZoneMinCount = 5;
    [Range(5, 10)] public int innerZoneMaxCount = 10;
    [Range(3, 7)] public int innermostZoneMinCount = 3;
    [Range(3, 7)] public int innermostZoneMaxCount = 7;

    [Header("Mountains Settings")]
    public int mountainCount = 5;
    public Vector3 playerStartPosition = new Vector3(0, 0.61f, 35.1f);
    public Vector2 mountainSpawnRadius = new Vector2(10f, 30f); // min and max distance from zone center
    public float minDistanceFromPlayer = 15f;
    public float mountainMinSpacing = 5f; // minimum distance between mountains
    public Vector2 mountainScaleRange = new Vector2(0.5f, 1f);

    [Header("General Spacing")]
    public float objectMinSpacing = 2f; // minimum spacing between any objects

    [Header("Spawn Layer Settings")]
    public LayerMask groundMask;

    [Header("Runtime Options")]
    public bool autoClearOldObjects = true;

    private Transform spawnParent;
    private List<Vector3> occupiedPositions = new List<Vector3>(); // tracks all spawned positions

    public void Start()
    {
        if (Application.isPlaying)
            GenerateAll();
    }

    [ContextMenu("Generate All")]
    public void GenerateAll()
    {
        if (!zoneVisualizer)
        {
            Debug.LogError("ZoneVisualizer not assigned!");
            return;
        }

        if (autoClearOldObjects)
            ClearSpawnedObjects();

        spawnParent = new GameObject("SpawnedObjects").transform;
        spawnParent.SetParent(transform);

        occupiedPositions.Clear();

        // 1️⃣ Mountains first
        SpawnMountains();

        // 2️⃣ Resources and enemies
        SpawnZone("Outer", zoneVisualizer.innerRadius, zoneVisualizer.outerRadius, outerZoneMinCount, outerZoneMaxCount, true);
        SpawnZone("Inner", zoneVisualizer.innermostRadius, zoneVisualizer.innerRadius, innerZoneMinCount, innerZoneMaxCount, false);
        SpawnZone("Innermost", 0f, zoneVisualizer.innermostRadius, innermostZoneMinCount, innermostZoneMaxCount, false);

        Debug.Log("All zones generated successfully!");
    }

    #region Zone Spawning
    private void SpawnZone(string zoneName, float minRadius, float maxRadius, int minCount, int maxCount, bool spawnEnemies)
    {
        Vector3 center = zoneVisualizer.transform.position;
        int resourceCount = Random.Range(minCount, maxCount + 1);

        for (int i = 0; i < resourceCount; i++)
        {
            SpawnPrefabInZone(Wood, center, minRadius, maxRadius);
            SpawnPrefabInZone(Fish, center, minRadius, maxRadius);
            SpawnPrefabInZone(IceBlock, center, minRadius, maxRadius);
        }

        if (spawnEnemies)
        {
            int wolfCount = Random.Range(3, 6);
            for (int i = 0; i < wolfCount; i++)
                SpawnPrefabInZone(Wolf, center, minRadius, maxRadius);
        }
    }

    private void SpawnPrefabInZone(GameObject prefab, Vector3 center, float minRadius, float maxRadius)
    {
        if (!prefab) return;

        for (int attempt = 0; attempt < 30; attempt++)
        {
            float angle = Random.Range(0f, 360f);
            float radius = Random.Range(minRadius, maxRadius);
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 spawnPos = center + offset + Vector3.up * 10f;

            if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 50f, groundMask))
            {
                Vector3 adjustedPos = hit.point + Vector3.up * 0.2f;

                if (IsPositionAvailable(adjustedPos))
                {
                    Instantiate(prefab, adjustedPos, Quaternion.identity, spawnParent);
                    occupiedPositions.Add(adjustedPos);
                    break;
                }
            }
        }
    }
    #endregion

    #region Mountain Spawning
    private void SpawnMountains()
    {
        if (!Mountain) return;

        Vector3 zoneCenter = zoneVisualizer.transform.position;

        for (int i = 0; i < mountainCount; i++)
        {
            for (int attempt = 0; attempt < 50; attempt++)
            {
                float angle = Random.Range(0f, 360f);
                float radius = Random.Range(mountainSpawnRadius.x, mountainSpawnRadius.y);
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Vector3 spawnPos = zoneCenter + offset + Vector3.up * 10f;

                // Avoid player
                if (Vector3.Distance(new Vector3(spawnPos.x, 0, spawnPos.z), new Vector3(playerStartPosition.x, 0, playerStartPosition.z)) < minDistanceFromPlayer)
                    continue;

                // Avoid overlapping any existing object
                if (!IsPositionAvailable(spawnPos, mountainMinSpacing))
                    continue;

                if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 50f, groundMask))
                {
                    Vector3 adjustedPos = hit.point + Vector3.up * 0.5f;
                    GameObject mountainInstance = Instantiate(Mountain, adjustedPos, Quaternion.Euler(0, Random.Range(0f, 360f), 0), spawnParent);

                    // Random scale
                    float scale = Random.Range(mountainScaleRange.x, mountainScaleRange.y);
                    mountainInstance.transform.localScale = Vector3.one * scale;

                    occupiedPositions.Add(adjustedPos);
                    break;
                }
            }
        }
    }
    #endregion

    #region Helper Functions
    private bool IsPositionAvailable(Vector3 pos, float spacing = -1f)
    {
        float minDist = spacing > 0 ? spacing : objectMinSpacing;

        foreach (Vector3 existing in occupiedPositions)
        {
            if (Vector3.Distance(new Vector3(pos.x, 0, pos.z), new Vector3(existing.x, 0, existing.z)) < minDist)
                return false;
        }
        return true;
    }

    private void ClearSpawnedObjects()
    {
        GameObject oldParent = GameObject.Find("SpawnedObjects");
        if (oldParent)
            DestroyImmediate(oldParent);
    }
    #endregion
}
