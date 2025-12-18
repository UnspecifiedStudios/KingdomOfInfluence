using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public Transform cameraTransform;

    CharacterController controller;
    private Vector2 moveInput;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        // calculate camera-relative directions
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // flatten on XZ plane
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // get movement direction
        Vector3 movement = forward * moveInput.y + right * moveInput.x;
        movement *= speed;

        // apply gravity 
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            // set to 0 if on floor
            verticalVelocity = 0f;
        }
        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;   // apply direction

        // move the player
        controller.Move(movement * Time.deltaTime);
    }
}
