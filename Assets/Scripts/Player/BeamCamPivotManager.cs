using UnityEngine;

public class BeamCamPivotManager : MonoBehaviour
{
    public Transform playerTransform;
    public Camera playerCameraRef;
    public Vector3 offsetFromPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //transform.localPosition = offSetFromPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position;
        transform.rotation = playerCameraRef.transform.rotation;
    }
}
