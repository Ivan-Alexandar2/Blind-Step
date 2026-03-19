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

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // 1. Get input from WASD or Arrow Keys
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 2. Determine if we are sprinting (holding Left Shift)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        // 3. Calculate direction
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 4. Move the player if there is input
        if (moveDirection.magnitude >= 0.1f)
        {
            // Calculate the rotation we need to face the direction we are moving
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // Smoothly rotate the player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the player using the CharacterController
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        }

        // Apply basic gravity so the player stays on the floor
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