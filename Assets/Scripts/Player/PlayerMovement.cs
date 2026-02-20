using System.Collections;
using System;
using System.Runtime.CompilerServices;
using System.Timers;
using UnityEditor.Rendering.Canvas.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MovementSettings
{
    public float defaultSpeed = 5f;
    public float sprintSpeedMult = 1.5f;
    public float gravity = -9.81f;
    public float rotationSpeed = 750f;
}

[Serializable]
public class AbilitySettings
{
    // Jump
    public float jumpForce = 6f;
    public float jumpStaminaCost = 20f;
    
    // Dodge
    public float dodgeDistance = 5f;
    public float dodgeDuration = 0.5f;
    public float dodgeStaminaCost = 30f;

    // Sprint
    public float runStaminaCost = 2f;
}

public class PlayerMovement : MonoBehaviour
{   
    // use classes for variables since it looks nicer and is collapsable in editor
    public MovementSettings moveSettings;
    public AbilitySettings ablSettings;
    public Transform cameraTransform;
    public GameObject playerAnimatorObject;

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
    private PlayerStats playerStats;
    private Quaternion rotationDirection;
    private bool hasJumped = false;
    private Animator playerAnimator;
    private int runStaminaCostMult = 10;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerAnimator = playerAnimatorObject.GetComponent<Animator>();
    }

    /*
     * Get player stats after it initializes with it's own awake function
    */
    private void Start()
    {
        playerStats = transform.parent.Find("Stats").GetComponent<PlayerStats>();
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
            JumpReleasedPlayer();
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
    
    /* 
     * OnDodge function will check to see if player can dodge, remove the required stamina,
     * and then will make the necessary calculations for movement. It will then start the coroutine.
     */
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed && !isDodging && playerStats.Stamina.TryConsume(ablSettings.dodgeStaminaCost)) 
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 inputDir = forward * moveInput.y + right * moveInput.x;
            dodgeDirection = inputDir.sqrMagnitude > 0.01f ? inputDir.normalized : forward * -1;

            dodgeSpeed = ablSettings.dodgeDistance / ablSettings.dodgeDuration;
            StartCoroutine(DodgeCoroutine());
        }
    }

    /*
     * Coroutine for dodge. carries out the actual movement.
    */
    IEnumerator DodgeCoroutine()
    {
        isDodging = true;
        float elapsed = 0f;

        while (elapsed < ablSettings.dodgeDuration)
        {
            // get gravity
            verticalVelocity = GetGravityVector();

            // apply direction
            Vector3 movement = dodgeDirection * dodgeSpeed + Vector3.up * verticalVelocity;

            controller.Move(movement * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isDodging = false;
    }

    // FixedUpdate (main movement)
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
        if (isHoldingRun && playerStats.Stamina.TryConsume(ablSettings.runStaminaCost * Time.deltaTime * runStaminaCostMult))
        {
            movement *= moveSettings.defaultSpeed * moveSettings.sprintSpeedMult;
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
            movement *= moveSettings.defaultSpeed;
        }

        // apply gravity 
        movement.y = GetGravityVector();

        // if controller is on ground and user pressed space (and seperate bool check for no doubledipping)
        if (isHoldingSpace && controller.isGrounded && !hasJumped)
        {
            // make them jump
            if (playerStats.Stamina.TryConsume(ablSettings.jumpStaminaCost))
            {
                playerAnimator.SetBool("isJumping", true);
                verticalVelocity = ablSettings.jumpForce;
                hasJumped = true;
            }
        }

        // move the player
        controller.Move(movement * Time.deltaTime);

        // handle rotation
        // check if player is moving in any x or z direction
        //  TODO: prolly gonna have to come back here to lock players movment rotation for attack rotation
        if(movement.x != 0 && movement.z != 0)
        {   
            // set animation
            playerAnimator.SetBool("isWalking", true);

            // instantiate new vector thats copied from movement, but with y-axis/gravity zeroed out
            Vector3 movementWithoutGrav = new Vector3(movement.x, 0, movement.z);

            // calculate rotation direction with new copied vector
            rotationDirection = Quaternion.LookRotation(movementWithoutGrav, Vector3.up);

            // rotate the player's transform
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationDirection, moveSettings.rotationSpeed * Time.deltaTime);
        }
        else
        {
            playerAnimator.SetBool("isWalking", false);
        }
    }

    public float GetGravityVector()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            playerAnimator.SetBool("isJumping", false);
            // set to -3 if on floor
            verticalVelocity = moveSettings.gravity; // gravity is applied to reduce/eliminate slope bouncing
            hasJumped = false;
        }
        
        // if the velocity is negative
        if (verticalVelocity + moveSettings.gravity * Time.deltaTime < 0f)
        {
            // apply direction * 2
            verticalVelocity += moveSettings.gravity * Time.deltaTime * 2f;
        }
        else
        {
            // apply direction * 1
            verticalVelocity += moveSettings.gravity * Time.deltaTime;
        }
         
        return verticalVelocity;
    }

    public void JumpReleasedPlayer()
    {
        if (!controller.isGrounded && verticalVelocity > 0f)
        {
            // TOMAYBE: Wanted to not let the player release too early into the jump, 
            //          but also wanted to still let them cancel it after a certain point (9/10 jumpForce).
            //          Code was decently complicated so it wasn't implemented, but maybe in the future?
            //          Use an else{} statement to Implement 
            if (verticalVelocity < 9*ablSettings.jumpForce/10)
            {
                verticalVelocity = 0f;    
            }
            
        }
    }

}
