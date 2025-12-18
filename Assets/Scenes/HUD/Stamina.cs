using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float regenRate = 15f;

    public float CurrentStamina { get; private set; }
    public float MaxStamina => maxStamina;

    private void Awake()
    {
        CurrentStamina = maxStamina;
    }

    private void Update()
    {
        Regenerate();
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
