using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;

    [Header("UI Objects")]
    public Image healthIconSource;
    public Sprite healthIconTrue;
    public Sprite healthIconFalse;
    public GameObject healthBar;
    public int barEmptyXPosition;
    public int barFullXPosition;

    public bool IsDead { get; private set; }
    
    //private vars
    private float healthBarPosition;
    private float positionDifference;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        IsDead = false;
        
    }

    private void Start()
    {
        healthIconSource.sprite = healthIconTrue; 
    }

    private void Update()
    {
        // update the health bar
        CustomSliderBarUtils.UpdateBarPosition(healthBar, CurrentHealth, maxHealth, barEmptyXPosition, barFullXPosition);
    }

    public void Damage(float amount)
    {
        if (IsDead) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0f, maxHealth);

        if (CurrentHealth <= 0f)
        {
            IsDead = true;
            healthIconSource.sprite = healthIconFalse;
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0f, maxHealth);
    }
}