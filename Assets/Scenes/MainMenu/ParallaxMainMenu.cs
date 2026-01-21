using UnityEngine;
using UnityEngine.InputSystem;
using System;

[Serializable]
public class ParallaxObjClass
{
    public GameObject prlxObj;
    public float prlxMult;
}

public class ParallaxMainMenu : MonoBehaviour
{
    //public 
    public float allParallaxCoefficient = -1f;
    public ParallaxObjClass[] parallaxObjects;
    
    //private
    private Vector2 mousePos;
    private Vector2 screenDimensions;
    private float mouseDiff;
    private float calculatedPositionValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenDimensions = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        mouseDiff = mousePos.x - (screenDimensions.x / 2);
        
        foreach (ParallaxObjClass obj in parallaxObjects)
        {
            calculatedPositionValue = mouseDiff * (obj.prlxMult / 100);
            Vector3 newPosition = new Vector3(allParallaxCoefficient * calculatedPositionValue, 0f, 0f);
            obj.prlxObj.transform.localPosition = newPosition;
        }
    }
}
