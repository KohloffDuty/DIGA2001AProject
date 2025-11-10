using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject snowballPrefab;   
    public Transform shootPoint;        
    public float fireRate = 0.5f;      

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        // Instantiate snowball at shootPoint position & rotation
        GameObject snowball = Instantiate(snowballPrefab, shootPoint.position, shootPoint.rotation);

      
        Rigidbody rb = snowball.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = shootPoint.forward * snowball.GetComponent<Snowball>().speed;
    }
}