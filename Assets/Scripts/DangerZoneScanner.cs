using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed to restart the level

public class DangerZoneScanner : MonoBehaviour
{
    [Header("Danger Settings")]
    public float dangerRadius = 4f;
    public float deathDistance = 0.6f;
    public LayerMask wallLayer;

    [Header("Audio Setup")]
    public GameObject emitterPoolPrefab; // spawn the parent object
    public AudioSource[] dangerEmitters;
    public AudioSource deathSource;
    public AudioClip deathClip;
    public float maxVolume = 1f;

    [Header("Exit Settings")]
    public float safeRadius;
    public LayerMask exitLayer;
    public AudioSource ambience;
    private float baseAmbienceVolume;

    internal bool isDead;
    private CinemachineImpulseSource impulseSource;
    private Animator animator;

    public float X;
    public float Y;
    public float Z;
    // A tiny custom container to hold the math for each wall we find
    private struct WallData
    {
        public Vector3 point;
        public float distance;

        public WallData(Vector3 p, float d)
        {
            point = p;
            distance = d;
        }
    }

    private void Start()
    {
        GameObject ambienceObject = GameObject.Find("GameManager");
        ambience = ambienceObject.GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        animator = GetComponentInChildren<Animator>();

        if (ambience != null) baseAmbienceVolume = ambience.volume;

        for (int i = 0; i < 3; i++)
        {
            GameObject myEmitters = Instantiate(emitterPoolPrefab, Vector3.zero, Quaternion.identity);
            dangerEmitters[i] = myEmitters.GetComponent<AudioSource>();
        }    
    }

    void Update()
    {
        ScanForSurroundWalls();
        ScanForExit();
    }

    private void ScanForSurroundWalls()
    {
        // 1. Find every wall piece inside our danger bubble
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, dangerRadius, wallLayer);

        // Create an empty list to store the math
        List<WallData> nearbyWalls = new List<WallData>();

        // 2. Do the math for every single wall we found
        foreach (Collider col in hitColliders)
        {
            Vector3 closestPoint = col.ClosestPoint(transform.position);
            float dist = Vector3.Distance(transform.position, closestPoint);

            nearbyWalls.Add(new WallData(closestPoint, dist));
        }

        // 3. Sort the list so the absolute closest walls are at the top (Index 0, 1, 2...)
        nearbyWalls = nearbyWalls.OrderBy(w => w.distance).ToList();

        // 4. Check if the VERY closest wall has touched us
        if (nearbyWalls.Count > 0 && nearbyWalls[0].distance <= deathDistance && !isDead)
        {
            isDead = true;
            StartCoroutine(DeathDelay());
            return; // Stop the code right here if we are dead
        }

        // 5. Assign our audio emitters to the closest walls
        for (int i = 0; i < dangerEmitters.Length; i++)
        {
            // If we have a wall for this specific emitter to track
            if (i < nearbyWalls.Count)
            {
                dangerEmitters[i].transform.position = nearbyWalls[i].point;

                if (!dangerEmitters[i].isPlaying) dangerEmitters[i].Play();

                // Calculate volume based on distance
                float volumePercent = 1f - Mathf.InverseLerp(deathDistance, dangerRadius, nearbyWalls[i].distance);
                dangerEmitters[i].volume = volumePercent * maxVolume;
            }
            // If there are NO walls for this emitter (e.g., we are in the open, or only 1 wall is near)
            else
            {
                if (dangerEmitters[i].isPlaying)
                {
                    dangerEmitters[i].volume = 0;
                    dangerEmitters[i].Stop();
                }
            }
        }
    }

    public void ScanForExit()
    {
        Collider[] hitExitColliders = Physics.OverlapSphere(transform.position, safeRadius, exitLayer);

        if (hitExitColliders.Length > 0)
        {
            float closestDistance = Mathf.Infinity;

            foreach (Collider col in hitExitColliders)
            {
                Vector3 point = col.ClosestPoint(transform.position);
                float distance = Vector3.Distance(transform.position, point);

                if (distance < closestDistance) closestDistance = distance;
            }

            // Map the distance (0 to safeRadius) to a multiplier (0.0 to 1.0)
            float volumeMultiplier = Mathf.InverseLerp(0f, safeRadius, closestDistance);

            // Set the absolute volume. If distance is 0, volumeMultiplier is 0 (silence).
            ambience.volume = baseAmbienceVolume * volumeMultiplier;
        }
        else
        {
            // If we walk away from the exit, make sure ambience returns to normal
            if (ambience.volume < baseAmbienceVolume)
            {
                ambience.volume = baseAmbienceVolume;
            }
        }
    }  

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator DeathDelay()
    {
        deathSource.PlayOneShot(deathClip);
        impulseSource.GenerateImpulse();

        // rotate the wrong animation
        animator.transform.localRotation = Quaternion.Euler(X, Y, Z);
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(0.4f);  
        Die();
    }
}