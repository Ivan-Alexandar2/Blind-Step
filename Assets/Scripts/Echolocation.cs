using UnityEngine;

public class Echolocation : MonoBehaviour
{
    [Header("Echolocation Settings")]
    public GameObject bulletPrefab;
    public int numberOfBullets = 8;

    [Header("Pulse Timings")]
    public float walkPulseRate = 0.6f;
    public float sprintPulseRate = 0.25f;
    private float pulseTimer;

    [Header("Audio")]
    public AudioSource footstepSource;
    public AudioClip[] footstepClips;

    void Update()
    {
        //pulseTimer -= Time.deltaTime;

        //bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        //bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        //if (pulseTimer <= 0f && isMoving)
        //{
        //    FirePulse();
        //    pulseTimer = isSprinting ? sprintPulseRate : walkPulseRate;
        //}
    }

    public void FirePulse()
    {
        if (footstepSource != null && footstepClips.Length > 0)
        {
            // Pick a random number between 0 and the total number of clips we have
            int randomStep = Random.Range(0, footstepClips.Length);

            // Play that specific clip
            footstepSource.PlayOneShot(footstepClips[randomStep]);
        }

        // 2. Fire the echolocation bullets
        float angleStep = 360f / numberOfBullets;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float currentAngle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);
            Instantiate(bulletPrefab, transform.position, rotation);
        }
    }
}
