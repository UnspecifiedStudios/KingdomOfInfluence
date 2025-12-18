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

    private float yaw;
    private float pitch;
    private Vector2 lookInput;

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
        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        Vector3 targetPosition = player.position + Vector3.up * verticalOffset;
        transform.position = targetPosition + offset;

        transform.LookAt(targetPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
