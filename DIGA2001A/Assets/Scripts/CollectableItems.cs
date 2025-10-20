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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (inv == null) return;

        switch (itemType)
        {
            case ItemType.Wood:
                inv.AddWood(amount);
                break;
            case ItemType.Fish:
                // Heal player for each fish collected
                if (health != null)
                    health.EatFish(amount * 5f); // 5% per fish
                break;
            case ItemType.IceBlock:
                inv.AddIce(amount);
                break;
        }

        Destroy(gameObject);
    }
}