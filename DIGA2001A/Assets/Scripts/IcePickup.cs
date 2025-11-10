using UnityEngine;

public class IcePickup : MonoBehaviour
{
    public int amount = 1;
    [Tooltip("Mark true if this is a placed igloo block and should not be picked up")]
    public bool isPlacedBlock = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only the player can collect
        if (!other.CompareTag("Player")) return;

        // Prevent pickup if this block is part of the igloo structure
        if (isPlacedBlock) return;

        // Otherwise, add to inventory
        var inv = other.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            inv.AddIce(amount);
        }

        Destroy(gameObject);
    }
}