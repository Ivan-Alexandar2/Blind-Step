using UnityEngine;

public class SoundBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 4f;

    private bool hasHitWall = false;
    private AudioSource audioSource;
    private Renderer meshRenderer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponentInChildren<Renderer>();

        // Destroy this bullet after 4 seconds to save memory
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Only move forward if we haven't hit a wall yet
        if (!hasHitWall)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    // This triggers when the bullet's trigger collider touches another collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") && !hasHitWall)
        {
            hasHitWall = true; // Stop moving

            // Change color to red for visual/deaf players
            meshRenderer.material.color = Color.red;

            // Play the 3D thud sound for blind players
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
    }
}
