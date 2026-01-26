using TMPro;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(Weapons))]
public class PlayerStats : MonoBehaviour
{
    public Health Health { get; private set; }
    public Stamina Stamina { get; private set; }
    public Weapons Weapons { get; private set; }

    public TMP_Text onScreenText;

    private void Awake()
    {
        Health = GetComponent<Health>();    
        Stamina = GetComponent<Stamina>();
        Weapons = GetComponent<Weapons>();
    }
    private void Update()
    {
        float currentHealth = Mathf.Round(Health.CurrentHealth);
        float currentStamina = Mathf.Round(Stamina.CurrentStamina);
        GameObject currentWeapon = Weapons.EquippedWeapon;
        string currentWeaponStr;
        if (currentWeapon == null)
        {
            currentWeaponStr = "None";
        }
        else
        {
            currentWeaponStr = currentWeapon.ToString();
        }
        string textToDisplay = $"Health: {currentHealth}/{Health.MaxHealth}\nStamina: {currentStamina}/{Stamina.MaxStamina}\nWeapon: {currentWeaponStr}\n";
        onScreenText.text = textToDisplay;  
    }
}
