using UnityEngine;


public class CameraTargeting : MonoBehaviour
{
    [Header("Detection Settings")]

    //search Radius aka player detection radius
    public float searchRadius = 20f;      
    [Range(0, 1)]

    //cone of detection, 1 is super hard mode and 0 is just anything in front
    public float focusNarrowness = 0.7f;

    //set to enemyLayer later for enemies
    public LayerMask enemyLayer;        

    public Transform freeLockCamera;

    LockOnCameraPivotManager lockOnCameraManagerComponent;

    private void Awake()
    {
        //Get the lockOnCameraPivotManager component from the paren pivot object
        lockOnCameraManagerComponent = transform.parent.GetComponent<LockOnCameraPivotManager>();
    }

    private void OnEnable()
    {
        //Capture the best canidate in view and give it to the lockOnCamera manager
        lockOnCameraManagerComponent.currentLockOnTarget = GetBestTargetInView();
    }

    public Transform GetBestTargetInView()
    {
        //Grab all enemies in view
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, searchRadius, enemyLayer);

        Transform candidate = null;

        //we want something closest to 1 aka most centered on screen
        float bestScore = -1f;

        foreach (Collider collider in potentialTargets)
        {
            //calculate direction from Camera to Enemy
            Vector3 directionToEnemy = (collider.transform.position - freeLockCamera.position).normalized;

            //Focus Check (Dot Product)
            //aligned the Camera's forward direction is with the Enemy
            float alignmentScore = Vector3.Dot(freeLockCamera.forward, directionToEnemy);

            //If target in focus
            if (alignmentScore > focusNarrowness)
            {
                //If not obscured by something
                if (HasLineOfSight(collider.transform))
                {
                    //return most centered enemy on screen
                    if (alignmentScore > bestScore)
                    {
                        bestScore = alignmentScore;
                        candidate = collider.transform;
                    }
                }
            }
        }
        return candidate;
    }

    // we do a little casting
    bool HasLineOfSight(Transform target)
    {
        //making sure we can see them
        RaycastHit hit;
        Vector3 start = freeLockCamera.position;
        Vector3 dir = (target.position - start).normalized;

        //if we can see it and its in our range
        if (Physics.Raycast(start, dir, out hit, searchRadius))
        {
            //send it
            return hit.transform == target;
        }
        return false;
    }
}
