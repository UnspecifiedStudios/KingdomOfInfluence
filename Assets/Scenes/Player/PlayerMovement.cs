using System.Collections;
using System.Timers;
using UnityEditor.Rendering.Canvas.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float defaultSpeed = 5f;
    public float sprintSpeedMult = 1.5f;
    public float gravity = -9.81f;
    public float jumpForce = 9f;
    public float dodgeDistance = 5f;
    public float dodgeDuration = 1f;
    public Transform cameraTransform;

    CharacterController controller;
    private Vector2 moveInput;
    private Vector2 jumpInput;
    private Vector2 sprintInput;
    private bool isHoldingRun = false;
    private bool isHoldingSpace = false;
    private bool isDodging = false;
    private Vector3 dodgeDirection;
    private float dodgeSpeed;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isHoldingSpace = true;
        }
        else if (context.canceled)
        {
            isHoldingSpace = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isHoldingRun = true;
        }
        else if (context.canceled)
        {
            isHoldingRun = false;
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        // TODO: Implement a check to see if user has enough stamina before starting dodge
        if (context.performed && !isDodging)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 inputDir = forward * moveInput.y + right * moveInput.x;
            dodgeDirection = inputDir.sqrMagnitude > 0.01f ? inputDir.normalized : forward * -1;

            dodgeSpeed = dodgeDistance / dodgeDuration;
            StartCoroutine(DodgeCoroutine());
        }
    }

    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        float elapsed = 0f;

        while (elapsed < dodgeDuration)
        {
            // apply gravity 
            if (controller.isGrounded && verticalVelocity < 0f)
            {
                // set to 0 if on floor
                verticalVelocity = 0f;
            }
            verticalVelocity += gravity * Time.deltaTime;

            // apply direction
            Vector3 movement = dodgeDirection * dodgeSpeed + Vector3.up * verticalVelocity;

            controller.Move(movement * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isDodging = false;
    }

    // Update is called once per frame
    void Update()
    {
        // only do if not dodging
        if (isDodging) return;

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

        // check if user is sprinting and apply move multiplier
        // TODO: Implement a check to see if user has enough stamina before running
        if (isHoldingRun)
        {
            movement *= defaultSpeed * sprintSpeedMult;
        }
        else
        {
            movement *= defaultSpeed;
        }

        // apply gravity 
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            // set to 0 if on floor
            verticalVelocity = 0f;
        }
        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;   // apply direction

        // if controller is on ground and user pressed space
        if (isHoldingSpace && controller.isGrounded)
        {
            // make them jump
            verticalVelocity = jumpForce;
        }

        // move the player
        controller.Move(movement * Time.deltaTime);
    }
}
