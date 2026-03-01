using JetBrains.Annotations;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float regenRate = 15f;

    [Header("Exhaustion Penalty")]
    [SerializeField] private float exhaustedRegenMultiplier = 0.7f; // 30% slower regernation when exhausted
    [SerializeField] private float exhaustedRegenDelay = 3f;    // Time in seconds before regeneration starts after exhaustion
    public float CurrentStamina { get; private set; }
    public float MaxStamina => maxStamina;
    public GameObject staminaBar;
    public int barEmptyXPosition;
    public int barFullXPosition;
    // private vars
    private float staminaBarPosition;
    private float positionDifference;
    
    private bool isExhausted;   //adding a condition for exhausted state
    private float regenDelayTimer;  //adadding a delay for the exhaustion penalty
    private void Awake()
    {
        CurrentStamina = maxStamina;
        isExhausted = false;
        regenDelayTimer = 0.0f;
    }

    private void Update()
    {
        Regenerate();
        CustomSliderBarUtils.UpdateBarPosition(staminaBar, CurrentStamina, maxStamina, barEmptyXPosition, barFullXPosition);
    }

    public bool TryConsume(float amount)
    {
        if (amount <= 0f) return true;

        
        if (CurrentStamina < amount)
        {
            CurrentStamina = 0f;
            EnterExhaustedState();
            return false;
        }

        CurrentStamina -= amount;

        if (CurrentStamina <= 0f)
        {
            CurrentStamina = 0f;
            EnterExhaustedState();
        }

        return true;
    }

    private void EnterExhaustedState()
    {
        isExhausted = true;
        regenDelayTimer = exhaustedRegenDelay;
    }


    private void Regenerate()
    {
        if (CurrentStamina >= maxStamina) return;

        if (isExhausted && regenDelayTimer > 0f)
        {
            regenDelayTimer -= Time.deltaTime;
            return;
        }

        float rate = regenRate;

        if (isExhausted)
        {
            rate *= exhaustedRegenMultiplier;  // 15 * 0.7 = 10.5
        }

        CurrentStamina += rate * Time.deltaTime;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, maxStamina);

        if (isExhausted && CurrentStamina >= maxStamina)
        {
            isExhausted = false;
            regenDelayTimer = 0f;
        }


    }

}
