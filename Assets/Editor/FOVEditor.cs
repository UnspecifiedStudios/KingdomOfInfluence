using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyScript))]

public class FOVEditor : Editor
{
    
    private void OnSceneGUI()
    {
        EnemyScript fov = (EnemyScript)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.fovRadius);

        // draw inner/outer radius
        Handles.color = Color.green;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radiusToReach - fov.innerRadTolerance);
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radiusToReach + fov.outerRadTolerance);
        Handles.color = new Color(0f, 1f, 0f, 0.10f);
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radiusToReach + fov.outerRadTolerance);
        Handles.color = new Color(1f, 0f, 0f, 0.10f);
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radiusToReach - fov.innerRadTolerance);
        
        // draw FOV viewable arc angle
            // init vars
        Handles.color = new Color(1f, 1f, 0f, 0.10f);
        Vector3 flattenYForward = fov.transform.forward;
        flattenYForward.y = 0f;
        flattenYForward.Normalize();

        Handles.DrawSolidArc(fov.transform.position, Vector3.up, flattenYForward, fov.fovAngle / 2, fov.fovRadius);
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, flattenYForward, -1 * (fov.fovAngle / 2), fov.fovRadius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.fovAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.fovAngle / 2);        

        // draw fov angles
        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.fovRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.fovRadius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.targetDestination.position);
        }

        if (fov.isBoss)
        {
            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.bossBarActivationRadius);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
