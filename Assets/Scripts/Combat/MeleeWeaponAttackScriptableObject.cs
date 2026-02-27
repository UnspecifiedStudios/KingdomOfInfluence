using UnityEngine;

public enum AttackType
{
    None,
    Single,
    Continous
}

[CreateAssetMenu(menuName = "Attacks/Melee Weapon Attacks")]
public class MeleeWeaponAttackScriptableObject : ScriptableObject
{
    public float uniqueID;
    // need some sort of enum (continuous/immediate)
    public AttackType attackType; 
    public float damage;
    public float duration;
    public float staminaCost;
    public string debugMsg;
}
