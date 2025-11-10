using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Snowball : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 10f;
    public float lifetime = 5f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;   // physics enabled
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.linearVelocity = transform.forward * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wolf"))
        {
            WolfAI wolf = other.GetComponent<WolfAI>();
            if (wolf != null)
            {
                wolf.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}