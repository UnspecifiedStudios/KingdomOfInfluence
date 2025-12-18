using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(Weapons))]
public class PlayerStats : MonoBehaviour
{
    public Health Health { get; private set; }
    public Stamina Stamina { get; private set; }
    public Weapons Weapons { get; private set; }

    private void Awake()
    {
        Health = GetComponent<Health>();
        Stamina = GetComponent<Stamina>();
        Weapons = GetComponent<Weapons>();
    }
}
