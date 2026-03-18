using UnityEngine;

public class Echolocation : MonoBehaviour
{
    [Header("Echolocation Settings")]
    public GameObject bulletPrefab;
    public int numberOfBullets = 8;

    // For testing purposes, we'll use a timer. Later we can tie this to footsteps!
    public float timeBetweenPulses = 0.5f;
    private float pulseTimer;

    void Update()
    {
        pulseTimer -= Time.deltaTime;

        // Automatically send out a pulse if the player is moving
        // (We check if the player is pressing WASD/Arrows)
        if (pulseTimer <= 0f && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            FirePulse();
            pulseTimer = timeBetweenPulses; // Reset the timer
        }
    }

    private void FirePulse()
    {
        // 360 degrees divided by 8 bullets = 45 degrees between each bullet
        float angleStep = 360f / numberOfBullets;

        for (int i = 0; i < numberOfBullets; i++)
        {
            // Calculate the angle for this specific bullet
            float currentAngle = i * angleStep;

            // Convert that angle into a rotation direction facing outward on the Y axis
            Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);

            // Spawn the bullet at the player's position with that rotation
            Instantiate(bulletPrefab, transform.position, rotation);
        }
    }
}
