using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Cold")]
    public float coldDamageRate = 5f;      // health lost per second when exposed
    public float fireHealRate = 3f;        // heal per second near fire
    public float shelterHealRate = 1.5f;   // heal per second inside shelter

    [Header("Wolf")]
    public float wolfDamage = 20f;

    private bool nearFire = false;
    private bool inShelter = false;
    private bool isAlive = true;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isAlive) return;

        if (nearFire)
            currentHealth += fireHealRate * Time.deltaTime;
        else if (inShelter)
            currentHealth += shelterHealRate * Time.deltaTime;
        else
            currentHealth -= coldDamageRate * Time.deltaTime;

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth <= 0f) Die();
    }

   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wolf"))
        {
            TakeDamage(wolfDamage);
        }
    }

    // Optional: also handle non-trigger colliders
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wolf"))
        {
            TakeDamage(wolfDamage);
        }
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;
        currentHealth -= amount;
        Debug.Log($"Player took {amount} damage. HP: {currentHealth}");
        if (currentHealth <= 0f) Die();
    }

    public void SetNearFire(bool v) => nearFire = v;
    public void SetInShelter(bool v) => inShelter = v;

    private void Die()
    {
        isAlive = false;
        Debug.Log("Penguin died.");
        // Disable movement or trigger game over here
    }
}

