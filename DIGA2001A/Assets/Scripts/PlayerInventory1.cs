using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerInventory1 : MonoBehaviour
{
    [Header("Inventory Counts")]
    public int woodCount;
    public int fishCount;
    public int iceCount;

    [Header("UI Reference")]
    public InventoryUI inventoryUI;

    private void Start()
    {
        // Initialize UI counts on start
        if (inventoryUI != null)
        {
            inventoryUI.UpdateCount(0, woodCount); // Wood
            inventoryUI.UpdateCount(1, fishCount); // Fish
            inventoryUI.UpdateCount(2, iceCount);  // Ice
        }
    }

    public void AddWood(int amount)
    {
        woodCount += amount;
        if (inventoryUI != null)
            inventoryUI.UpdateCount(0, woodCount);
    }

    public void AddFish(int amount)
    {
        fishCount += amount;
        if (inventoryUI != null)
            inventoryUI.UpdateCount(1, fishCount);
    }

    public void AddIce(int amount)
    {
        iceCount += amount;
        if (inventoryUI != null)
            inventoryUI.UpdateCount(2, iceCount);
    }
}
