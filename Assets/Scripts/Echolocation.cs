using UnityEngine;

public class Echolocation : MonoBehaviour
{
    [Header("Echolocation Settings")]
    public GameObject bulletPrefab;
    public int numberOfStepBullets;
    
    [Header("Pulse Timings")]
    public float walkPulseRate = 0.6f;
    public float sprintPulseRate = 0.25f;
    private float pulseTimer;

    [Header("Audio")]
    public AudioSource footstepSource;
    public AudioClip[] footstepClips;
    public AudioClip clappingClips;

    [Header("Clapping")]
    public int numberOfClapBullets;
    public float clapCooldown;

    void Update()
    {
        if(clapCooldown <= 3 && clapCooldown >= 0)
        {
            clapCooldown -= Time.deltaTime;
        }      

        if(Input.GetKeyDown(KeyCode.Q) && clapCooldown <= 0)
        {
            FireClapPulse();
            clapCooldown = 3;
        }
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
        float angleStep = 360f / numberOfStepBullets;

        for (int i = 0; i < numberOfStepBullets; i++)
        {
            float currentAngle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);
            Instantiate(bulletPrefab, transform.position, rotation);
        }
    }

    public void FireClapPulse()
    {
        footstepSource.PlayOneShot(clappingClips);

        // 2. Fire the echolocation bullets
        float angleStep = 360f / numberOfClapBullets;

        for (int i = 0; i < numberOfClapBullets; i++)
        {
            float currentAngle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);
            Instantiate(bulletPrefab, transform.position, rotation);
        }
    }
}
