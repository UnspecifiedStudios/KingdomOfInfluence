using UnityEngine;
using UnityEngine.InputSystem;
using System;

[Serializable]
public class ParallaxObjClass
{
    public GameObject prlxObj;
    public float prlxXMult;
    public float prlxYMult;
    [HideInInspector]
    public Vector3 originalCoordinates;
}

public class ParallaxMainMenu : MonoBehaviour
{
    //public 
    public float allXParallaxCoefficient = -1f;
    public float allYParallaxCoefficient = -1f;
    public ParallaxObjClass[] parallaxObjects;
    
    //private
    private Vector2 mousePos;
    private Vector2 screenDimensions;
    private float mouseXDiff;
    private float mouseYDiff;

    private float calculatedXPosValue;
    private float calculatedYPosValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenDimensions = new Vector2(Screen.width, Screen.height);
        foreach (ParallaxObjClass obj in parallaxObjects)
        {
            obj.originalCoordinates = obj.prlxObj.transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        mouseXDiff = mousePos.x - (screenDimensions.x / 2);
        mouseYDiff = mousePos.y - (screenDimensions.y / 2);
        
        foreach (ParallaxObjClass obj in parallaxObjects)
        {
            calculatedXPosValue = mouseXDiff * (obj.prlxXMult / 100);
            calculatedYPosValue = mouseYDiff * (obj.prlxYMult / 100);
            Vector3 newPosition = new Vector3((allXParallaxCoefficient * calculatedXPosValue) + obj.originalCoordinates.x, (allYParallaxCoefficient * calculatedYPosValue) + obj.originalCoordinates.y, 0f);
            obj.prlxObj.transform.localPosition = newPosition;
        }
    }
}
