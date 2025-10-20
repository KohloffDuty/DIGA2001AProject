using UnityEngine;

public class WolfAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Detection Settings")]
    public float detectionRadius = 20f; // start hunting when player is within this radius
    public float moveSpeed = 3f;        // wolf movement speed

    [Header("Combat Settings")]
    public float attackDistance = 2f;   // distance at which the wolf kills player
    public float attackCooldown = 1.5f;

    [Header("Health Settings")]
    public float maxHealth = 50f;
    private float currentHealth;
    private bool isAlive = true;

    private float lastAttackTime = 0f;

    private void Start()
    {
        currentHealth = maxHealth;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (!isAlive || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Detect player and start moving
        if (distance <= detectionRadius)
        {
            // Move toward player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Optional: rotate wolf to face player
            transform.forward = direction;

            // Attack player if close
            if (distance <= attackDistance && Time.time - lastAttackTime > attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            // Idle or do nothing
        }
    }

    private void AttackPlayer()
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(health.currentHealth); // instant kill
            Debug.Log("Player attacked by wolf!");
        }
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;

        currentHealth -= amount;
        Debug.Log($"Wolf took {amount} damage. HP: {currentHealth}");

        if (currentHealth <= 0f) Die();
    }

    private void Die()
    {
        isAlive = false;
        Destroy(gameObject, 2f);
        Debug.Log("Wolf died.");
    }
}
