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
    private PlayerCombat playerCombat;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        IsDead = false;

        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Start()
    {
        healthIconSource.sprite = healthIconTrue; 
    }

    private void Update()
    {
        // update the health bar
        positionDifference = barFullXPosition - barEmptyXPosition;
        healthBarPosition = -1 * (positionDifference - ((CurrentHealth / maxHealth) * positionDifference));
        healthBar.transform.localPosition = new UnityEngine.Vector3(healthBarPosition, 0f, 0f);
    }

    public void Damage(float amount)
    {
        if (IsDead) return;

        if (playerCombat != null && playerCombat.IsInvulnerable) return; // ADD

        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0f, maxHealth);

        if (playerCombat != null && amount > 0f)
            playerCombat.TriggerIFrames(); // ADD

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