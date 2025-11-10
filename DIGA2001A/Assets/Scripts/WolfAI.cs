using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class WolfAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Detection Settings")]
    public float detectionRadius = 20f;
    public float moveSpeed = 3f;
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;

    [Header("Combat Settings")]
    public float attackDistance = 2f;
    public float attackCooldown = 1.5f;
    public float attackDuration = 1f; // how long the attack animation lasts
    public float damage = 20f; // new: amount of damage per attack

    [Header("Health Settings")]
    public float maxHealth = 50f;

    private float currentHealth;
    private bool isAlive = true;
    private bool isAttacking = false;

    private Rigidbody rb;
    private Animator anim;
    private Vector3 wanderTarget;
    private float lastAttackTime;
    private float lastWanderTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        PickNewWanderTarget();
    }

    private void Update()
    {
        if (!isAlive || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (isAttacking)
        {
            anim.SetBool("isMoving", false); // stop moving animation
            return; // stop all movement while attacking
        }

        if (distance <= detectionRadius)
        {
            // Chase player
            MoveTowards(player.position);

            // Face player
            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
                transform.forward = lookDir;

            // Attack if close
            if (distance <= attackDistance && Time.time - lastAttackTime > attackCooldown)
            {
                StartCoroutine(AttackRoutine());
            }

            anim.SetBool("isMoving", distance > attackDistance);
        }
        else
        {
            // Wander
            if (Time.time - lastWanderTime > wanderInterval)
                PickNewWanderTarget();

            MoveTowards(wanderTarget);
            float wanderDistance = Vector3.Distance(transform.position, wanderTarget);
            anim.SetBool("isMoving", wanderDistance > 0.5f);
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 newPosition = Vector3.MoveTowards(
            transform.position,
            new Vector3(target.x, transform.position.y, target.z),
            moveSpeed * Time.deltaTime
        );

        rb.MovePosition(newPosition);
    }

    private void PickNewWanderTarget()
    {
        lastWanderTime = Time.time;
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.SetTrigger("attack");
        lastAttackTime = Time.time;

        // Deal proper damage instead of instant kill
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Debug.Log("Wolf attacked player for " + damage);
        }

        // Wait for attack animation to finish before moving again
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;

        currentHealth -= amount;
        Debug.Log($"Wolf took {amount} damage. HP: {currentHealth}");

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        isAlive = false;
        anim.SetTrigger("die");
        rb.isKinematic = true;
        Destroy(gameObject, 2f);
        Debug.Log("Wolf died.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
