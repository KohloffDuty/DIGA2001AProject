using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    [Header("Resource Counts")]
    public int woodCount = 0;
    public int iceCount = 0;
    public int fishCount = 0; // 🐟 Added for fish collection

    [Header("Requirements")]
    public int woodNeededForFire = 3;
    public int iceNeededForShelter = 6;

    // Event so UI/manager updates when inventory changes
    public event Action OnInventoryChanged;

    public void NotifyInventoryChanged()
    {
        OnInventoryChanged?.Invoke();
    }

    // 🪵 WOOD
    public void AddWood(int amount = 1)
    {
        woodCount += amount;
        NotifyInventoryChanged();
        Debug.Log($"Picked up wood: {woodCount}");
    }

    public void RemoveWood(int amount = 1)
    {
        woodCount = Mathf.Max(0, woodCount - amount);
        NotifyInventoryChanged();
    }

    public bool CanBuildFire() => woodCount >= woodNeededForFire;

    public void UseWoodForFire()
    {
        if (!CanBuildFire()) return;
        woodCount -= woodNeededForFire;
        NotifyInventoryChanged();
    }

    // ❄️ ICE
    public void AddIce(int amount = 1)
    {
        iceCount += amount;
        NotifyInventoryChanged();
        Debug.Log($"Picked up ice: {iceCount}");
    }

    public void RemoveIce(int amount = 1)
    {
        iceCount = Mathf.Max(0, iceCount - amount);
        NotifyInventoryChanged();
    }

    public bool CanBuildShelter() => iceCount >= iceNeededForShelter;

    public void UseIceForShelter()
    {
        if (!CanBuildShelter()) return;
        iceCount -= iceNeededForShelter;
        NotifyInventoryChanged();
    }

    // 🐟 FISH
    public void AddFish(int amount = 1)
    {
        fishCount += amount;
        NotifyInventoryChanged();
        Debug.Log($"Picked up fish: {fishCount}");
        // In the future: hook this to PlayerHealth.AddHealth(amount * healValue);
    }

    public void RemoveFish(int amount = 1)
    {
        fishCount = Mathf.Max(0, fishCount - amount);
        NotifyInventoryChanged();
    }
}
