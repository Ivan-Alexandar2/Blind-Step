using UnityEngine;
using UnityEngine.SceneManagement; // Needed to restart the level

public class DangerZoneScanner : MonoBehaviour
{
    [Header("Danger Settings")]
    public float dangerRadius = 4f;
    public float deathDistance = 0.6f;
    public LayerMask wallLayer;

    [Header("Audio Setup")]
    public AudioSource dangerEmitter;
    public float maxVolume = 1f;

    void Update()
    {
        ScanForWalls();
    }

    private void ScanForWalls()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, dangerRadius, wallLayer);

        if (hitColliders.Length > 0)
        {
            float closestDistance = Mathf.Infinity;
            Vector3 closestPoint = Vector3.zero;

            foreach (Collider col in hitColliders)
            {
                // Unity calculates the exact closest spot on the collider's surface
                Vector3 point = col.ClosestPoint(transform.position);
                float distance = Vector3.Distance(transform.position, point);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = point;
                }
            }

            // Move the audio emitter to that exact spot on the wall
            dangerEmitter.transform.position = closestPoint;

            if (!dangerEmitter.isPlaying) dangerEmitter.Play();

            // Calculate volume based on distance (closer = louder)
            // InverseLerp maps the distance between deathDistance and dangerRadius to a 0-1 percentage
            float volumePercent = 1f - Mathf.InverseLerp(deathDistance, dangerRadius, closestDistance);
            dangerEmitter.volume = volumePercent * maxVolume;

            if (closestDistance <= deathDistance)
            {
                Die();
            }
        }
        else
        {
            // If no walls are in the danger radius, silence the emitter
            if (dangerEmitter.volume > 0)
            {
                dangerEmitter.volume = 0;
                dangerEmitter.Stop();
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player touched the wall! Restarting...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //private void OnDrawGizmos()
    //{
    //    Debug.DrawLine(transform.position, dangerRadius);
    //}
}