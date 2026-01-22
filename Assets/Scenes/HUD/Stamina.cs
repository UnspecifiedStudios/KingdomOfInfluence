using JetBrains.Annotations;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float regenRate = 15f;

    public float CurrentStamina { get; private set; }
    public float MaxStamina => maxStamina;
    public GameObject staminaBar;
    public int barEmptyXPosition;
    public int barFullXPosition;
    // private vars
    private float staminaBarPosition;
    private float positionDifference;
    private void Awake()
    {
        CurrentStamina = maxStamina;

    }

    private void Update()
    {
        Regenerate();
        positionDifference = barFullXPosition - barEmptyXPosition;
        staminaBarPosition = -1 * (positionDifference - ((CurrentStamina / maxStamina) * positionDifference));
        staminaBar.transform.localPosition = new Vector3(staminaBarPosition, 0f, 0f);
    }

    public bool TryConsume(float amount)
    {
        if (CurrentStamina < amount)
        {
            return false;
        }

        CurrentStamina -= amount;
        return true;
    }

    private void Regenerate()
    {
        if (CurrentStamina >= maxStamina) return;

        CurrentStamina += regenRate * Time.deltaTime;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, maxStamina);
    }
}
