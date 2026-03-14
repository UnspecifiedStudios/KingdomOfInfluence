using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public Camera cameraToFace;
    public Transform target;
    public Vector3 healthBarOffset;
    

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    void Update()
    {
        transform.rotation = cameraToFace.transform.rotation;
        transform.position = target.position + healthBarOffset;
    }
}
