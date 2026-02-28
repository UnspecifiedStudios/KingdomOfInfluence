using UnityEngine;

public class AnimBeamOuter : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float rotationDirection = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed*rotationDirection*Time.deltaTime,0f,0f);
    }
}
