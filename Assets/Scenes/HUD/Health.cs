using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;

    public bool IsDead { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
        IsDead = false;
    }

    public void Damage(float amount)
    {
        if (IsDead) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0f, maxHealth);

        if (CurrentHealth <= 0f)
        {
            IsDead = true;
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0f, maxHealth);
    }
}