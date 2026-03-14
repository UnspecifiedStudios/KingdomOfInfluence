using UnityEngine;

public class LockOnCameraPivotManager : MonoBehaviour
{
    public Transform playerTransform;
    public Transform lockOnCameraTransform;
    public Transform currentLockOnTarget;
    public Vector3 offsetFromPlayer;

    //CameraTargeting targetManagerComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Offset the lock on camera's position relative to its parent (the object this script is attached to)
        lockOnCameraTransform.localPosition = offsetFromPlayer;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Set the current position to player's position
        transform.position = playerTransform.position;

        //Check for currentLockOnTarget being null
        if (currentLockOnTarget != null)
        {
            //Get this object to look at player so lock on camera rotates with this object
            transform.LookAt(currentLockOnTarget);
        }
    }
}
