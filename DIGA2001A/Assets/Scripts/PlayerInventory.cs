using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public int woodCount = 0;
    public int iceCount = 0;

    public int woodNeededForFire = 3;
    public int iceNeededForShelter = 6;

    // Simple event so UI/manager updates when inventory changes
    public event Action OnInventoryChanged;

    // Call this to notify listeners manually (instead of Invoke())
    public void NotifyInventoryChanged()
    {
        OnInventoryChanged?.Invoke();
    }

    public void AddWood(int amount = 1)
    {
        woodCount += amount;
        NotifyInventoryChanged();
        Debug.Log($"Picked up wood: {woodCount}");
    }

    public void AddIce(int amount = 1)
    {
        iceCount += amount;
        NotifyInventoryChanged();
        Debug.Log($"Picked up ice: {iceCount}");
    }

    public void RemoveWood(int amount = 1)
    {
        woodCount = Mathf.Max(0, woodCount - amount);
        NotifyInventoryChanged();
    }

    public void RemoveIce(int amount = 1)
    {
        iceCount = Mathf.Max(0, iceCount - amount);
        NotifyInventoryChanged();
    }

    public bool CanBuildFire() => woodCount >= woodNeededForFire;
    public bool CanBuildShelter() => iceCount >= iceNeededForShelter;

    public void UseWoodForFire()
    {
        if (!CanBuildFire()) return;
        woodCount -= woodNeededForFire;
        NotifyInventoryChanged();
    }

    public void UseIceForShelter()
    {
        if (!CanBuildShelter()) return;
        iceCount -= iceNeededForShelter;
        NotifyInventoryChanged();
    }
}