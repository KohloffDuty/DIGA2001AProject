using UnityEngine;

public class IcePickup : MonoBehaviour
{
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = other.GetComponent<PlayerInventory>();
        if (inv != null) inv.AddIce(amount);

        Destroy(gameObject);
    }
}
