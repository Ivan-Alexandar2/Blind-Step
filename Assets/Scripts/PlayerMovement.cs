using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 10f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private Animator animator;
    private float currentAnimSpeed = 0f;

    private DangerZoneScanner zoneScanner;
    void Start()
    {
        zoneScanner = FindObjectOfType<DangerZoneScanner>();
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError(" CRITICAL ERROR: I could not find an Animator in the children! The animations will not play.");
        }
        else
        {
            Debug.Log(" SUCCESS: I found the Animator attached to: " + animator.gameObject.name);
        }
    }

    void Update()
    {
        if(!zoneScanner.isDead)
        {
            MovePlayer();
        }    
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Start by assuming we aren't moving
        float targetSpeed = 0f;

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // We ARE moving! Update the targetSpeed to walk or sprint
            targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

            // Handle Rotation
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Handle Physical Movement
            controller.Move(moveDirection * targetSpeed * Time.deltaTime);
        }

        // Smoothly glide the animation speed toward whatever targetSpeed is (5, 8, or 0)
        currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, targetSpeed, Time.deltaTime * 10f);

        // Send the smoothed number to the Animator!
        if (animator != null)
        {
            animator.SetFloat("Speed", currentAnimSpeed);
        }

        // Apply Gravity
        controller.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit"))
        {
            Debug.Log("VICTORY!");
        }
    }
}