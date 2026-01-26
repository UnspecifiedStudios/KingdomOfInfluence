using UnityEngine;

public class LockOnCameraPivotManager : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject lockedOnTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerTransform.position;
        transform.LookAt(lockedOnTarget.transform);
    }
}
