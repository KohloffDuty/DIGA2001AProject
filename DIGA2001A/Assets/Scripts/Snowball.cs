using UnityEngine;

public class Snowball : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 10f;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // destroy after a while
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if it hit the wolf
        WolfAI wolf = other.GetComponent<WolfAI>();
        if (wolf != null)
        {
            wolf.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Optional: destroy on hitting any other object
        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}