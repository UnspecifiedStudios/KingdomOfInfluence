using UnityEngine;

public class LockOnCameraPivotManager : MonoBehaviour
{
    public Transform playerTransform;
    public Transform lockOnCameraTransform;
    public GameObject currentLockOnTarget;
    public Vector3 offsetFromPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Offset the lock on camera's position relative to its parent (the object this script is attached to)
        lockOnCameraTransform.localPosition = offsetFromPlayer;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // TODO: Utilize the "TargetManager" script here to calculate the current lock on target
        // For now, just set the currentLockOnTarget in the editor


        //Set the current position to player's position
        transform.position = playerTransform.position;

        //Get this object to look at player so lock on camera rotates with this object
        transform.LookAt(currentLockOnTarget.transform);
    }
}
