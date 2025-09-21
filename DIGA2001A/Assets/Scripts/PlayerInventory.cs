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

    public void AddWood(int amount = 1)
    {
        woodCount += amount;
        OnInventoryChanged?.Invoke();
        Debug.Log($"Picked up wood: {woodCount}");
    }

    public void AddIce(int amount = 1)
    {
        iceCount += amount;
        OnInventoryChanged?.Invoke();
        Debug.Log($"Picked up ice: {iceCount}");
    }

    public bool CanBuildFire() => woodCount >= woodNeededForFire;
    public bool CanBuildShelter() => iceCount >= iceNeededForShelter;

    public void UseWoodForFire()
    {
        if (!CanBuildFire()) return;
        woodCount -= woodNeededForFire;
        OnInventoryChanged?.Invoke();
    }

    public void UseIceForShelter()
    {
        if (!CanBuildShelter()) return;
        iceCount -= iceNeededForShelter;
        OnInventoryChanged?.Invoke();
    }
}
