using UnityEngine;

public class LockOnCamera : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offsetFromPlayer;
    public GameObject currentLockOnTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Update camera's position to follow player + an offset
        //float offsetDistance = 1f;
        //Vector3 absVectorPosition = new Vector3(Mathf.Abs(currentLockOnTarget.transform.position.x), 2, Mathf.Abs(currentLockOnTarget.transform.position.z));
        Vector3 cameraPosition = playerTransform.position + currentLockOnTarget.transform.position;
        //transform.position = cameraPosition;
        transform.localPosition = new Vector3(0, 2, -3);
        // Calculate camera's rotation relative to lock on target's position
        Vector3 lockOntargetPosition = currentLockOnTarget.transform.position - transform.position;
        Quaternion lockOnRotation = Quaternion.LookRotation(lockOntargetPosition, Vector3.up);
        //transform.LookAt(currentLockOnTarget.transform.position);

        // Rotate the camera to the target rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, lockOnRotation, 0.2f);


    }
}
