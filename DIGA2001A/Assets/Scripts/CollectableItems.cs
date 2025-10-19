using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public enum ItemType
    {
        Wood,
        Fish,
        IceBlock
    }

    [Header("Item Settings")]
    public ItemType itemType;
    public int amount = 1;

    // Optional: Add a simple trigger collection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Here you can call PlayerInventory to add the item
            Debug.Log($"Player collected {amount} x {itemType}");
            Destroy(gameObject);
        }
    }
}