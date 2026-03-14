using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class OrbitCamera : MonoBehaviour
{
    [Serializable]
    public enum CameraMouseInteraction
    {
        None,
        Allow, 
        Prevent,
        Other
    }
    [Serializable]
    public class CameraOverrideController
    {
        public Transform overrideCam;
        public CameraMouseInteraction mouseRule;
    }
    
    public Transform player;
    public float distance = 5f;
    public float sensitivity = 0.15f;
    public float minY = -20f;
    public float maxY = 80f;
    public float verticalOffset = 1f;


    public float transitionDuration = 0.2f;

    private float yaw;
    private float pitch;
    private Vector2 lookInput;
    // camera smoothing vars

    private bool isTransitioning;
    private float transitionTimer;
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 targetPos;
    private Quaternion targetRot;
    private Vector3 dynamicTargetPos;
    private Quaternion dynamicTargetRot;

    // assistance
    
    public CameraOverrideController activeOverrideCamera;
    private Transform previousOverrideCamera;
    [HideInInspector] public bool mouseMovementDisabled = false;  // currently handled by DialogueManager.cs


    private void Awake()
    {
        player = transform.parent.GetChild(0).GetComponent<Transform>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        if (mouseMovementDisabled)
        {
            return;
        }
        // calculate mouse inputs
        if (activeOverrideCamera.mouseRule != CameraMouseInteraction.Prevent)
        {
            yaw += lookInput.x * sensitivity;
            pitch -= lookInput.y * sensitivity;
            pitch = Mathf.Clamp(pitch, minY, maxY);
        }

        // start transition if change in bool detected
        if (activeOverrideCamera.overrideCam != previousOverrideCamera)
        {
            StartTransition();
            previousOverrideCamera = activeOverrideCamera.overrideCam;
        }

        // transition logic
        if (isTransitioning)
        {
            // calculate time things
            transitionTimer += Time.deltaTime;
            float transitionProgress = transitionTimer / transitionDuration;
            transitionProgress = Mathf.Clamp01(transitionProgress);

            // ease-out cubic (basically fast start, slow finish)
            float easedProgress = 1f - Mathf.Pow(1f - transitionProgress, 3f);

            if (activeOverrideCamera.overrideCam != null)
            {
                dynamicTargetPos = activeOverrideCamera.overrideCam.position;
                dynamicTargetRot = activeOverrideCamera.overrideCam.rotation;
            }
            else
            {
                Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
                Vector3 offset = rotation * new Vector3(0, 0, -distance);

                Vector3 targetPosition = player.position + Vector3.up * verticalOffset;

                dynamicTargetPos = targetPosition + offset;
                dynamicTargetRot = Quaternion.LookRotation(targetPosition - dynamicTargetPos);
            }
            // lerp/slerp rotatation & position
            transform.position = Vector3.Lerp(startPos, dynamicTargetPos, easedProgress);

            if (activeOverrideCamera != null && activeOverrideCamera.mouseRule == CameraMouseInteraction.Allow)
            {
                // allow mouse rotation during transition
                transform.rotation = Quaternion.Euler(pitch, yaw, 0);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(startRot, dynamicTargetRot, easedProgress);
            }
            

            // check if done transitioning
            if (transitionProgress >= 1f)
            {
                isTransitioning = false;
            }
        }
        else
        {
            // normal, nontransition logic
            if (activeOverrideCamera != null && activeOverrideCamera.overrideCam != null)
            {
                if (activeOverrideCamera.mouseRule == CameraMouseInteraction.Prevent)
                {
                    // Fully locked
                    transform.position = activeOverrideCamera.overrideCam.position;
                    transform.rotation = activeOverrideCamera.overrideCam.rotation;
                }
                else
                {
                    // beam logic 
                    transform.position = activeOverrideCamera.overrideCam.position;
                    transform.rotation = Quaternion.Euler(pitch, yaw, 0);
                }
            }
            else
            {
                Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
                Vector3 offset = rotation * new Vector3(0, 0, -distance);

                Vector3 targetPosition = player.position + Vector3.up * verticalOffset;

                transform.position = targetPosition + offset;
                transform.LookAt(targetPosition);
            }
        }
    }

    private void StartTransition()
    {  
        // reset timer, set bool
        isTransitioning = true;
        transitionTimer = 0f;

        // calculate start positions depending on lock on status
        startPos = transform.position;
        startRot = transform.rotation;

        if (activeOverrideCamera.overrideCam != null)
        {
            targetPos = activeOverrideCamera.overrideCam.position;
            targetRot = activeOverrideCamera.overrideCam.rotation;
        }
        else
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 offset = rotation * new Vector3(0, 0, -distance);

            Vector3 targetPosition = player.position + Vector3.up * verticalOffset;

            targetPos = targetPosition + offset;
            targetRot = Quaternion.LookRotation(targetPosition - targetPos);
        }
    }
}
