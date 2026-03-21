using UnityEngine;

public class EarTracking : MonoBehaviour
{
    public Transform targetPlayer;

    // We use LateUpdate instead of Update so the camera/player moves FIRST, 
    // and the ears follow perfectly without jittering.

    private void Start()
    {
        targetPlayer = FindObjectOfType<PlayerMovement>().transform;
    }
    void LateUpdate()
    {
        if (targetPlayer != null)
        {
            transform.position = targetPlayer.position;
            transform.rotation = Quaternion.identity;
        }
    }
}
