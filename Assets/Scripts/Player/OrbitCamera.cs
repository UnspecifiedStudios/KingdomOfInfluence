using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    public Transform player;
    public float distance = 5f;
    public float sensitivity = 0.15f;
    public float minY = -20f;
    public float maxY = 80f;
    public float verticalOffset = 1f;
    public GameObject lockOnCameraPosition;

    private float yaw;
    private float pitch;
    private Vector2 lookInput;

    [HideInInspector] public bool currentlyLockingOn = false;

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
        if (currentlyLockingOn)
        {
            transform.position = lockOnCameraPosition.transform.position;
            transform.rotation = lockOnCameraPosition.transform.rotation;
        }
        else
        {
            yaw += lookInput.x * sensitivity;
            pitch -= lookInput.y * sensitivity;
            pitch = Mathf.Clamp(pitch, minY, maxY);

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

            Vector3 offset = rotation * new Vector3(0, 0, -distance);

            Vector3 targetPosition = player.position + Vector3.up * verticalOffset;
            transform.position = targetPosition + offset;

            transform.LookAt(targetPosition);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
