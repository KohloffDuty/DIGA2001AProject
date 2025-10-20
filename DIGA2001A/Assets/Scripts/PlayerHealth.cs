using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")] public float maxHealth = 100f;
    public float currentHealth;

    [Header("Cold Damage Per Zone")] public float innerZoneDamageRate = 2f;
    public float outerZoneDamageRate = 5f;

    [Header("Fire/Shelter Healing")] public float fireHealRate = 3f;
    public float shelterHealRate = 1.5f;

    [Header("Wolf Damage")] public float wolfDamage = 20f;

    [Header("Zone Reference")] public ZoneVisualizer zoneVisualizer;

    private bool nearFire = false;
    private bool inShelter = false;
    private bool isAlive = true;

    private enum ZoneType
    {
        Innermost,
        Inner,
        Outer
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isAlive) return;

        ZoneType zone = GetCurrentZone();

        if (nearFire)
            currentHealth += fireHealRate * Time.deltaTime;
        else if (inShelter)
            currentHealth += shelterHealRate * Time.deltaTime;
        else
        {
            switch (zone)
            {
                case ZoneType.Inner:
                    currentHealth -= innerZoneDamageRate * Time.deltaTime;
                    break;
                case ZoneType.Outer:
                    currentHealth -= outerZoneDamageRate * Time.deltaTime;
                    break;
                case ZoneType.Innermost:
                    break;
            }
        }

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth <= 0f) Die();
    }

    public void EatFish(float healPercent = 5f)
    {
        if (!isAlive) return;

        float healAmount = maxHealth * (healPercent / 100f);
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        Debug.Log($"Ate fish! Health +{healAmount}. Current HP: {currentHealth}");
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

    private ZoneType GetCurrentZone()
    {
        if (zoneVisualizer == null)
            return ZoneType.Outer;

        float distance = Vector3.Distance(transform.position, zoneVisualizer.transform.position);

        if (distance <= zoneVisualizer.innermostRadius)
            return ZoneType.Innermost;
        else if (distance <= zoneVisualizer.innerRadius)
            return ZoneType.Inner;
        else
            return ZoneType.Outer;
    }

    private void Die()
    {
        if (!isAlive) return;

        isAlive = false;
        Debug.Log("Penguin died.");

        // Optional: disable movement if you have a movement script
        // var movement = GetComponent<YourMovementScript>();
        // if (movement != null) movement.enabled = false;

        // Load the end scene
        LoadEndScene();
    }

    private void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene"); // Make sure this matches your scene name
    }
}
