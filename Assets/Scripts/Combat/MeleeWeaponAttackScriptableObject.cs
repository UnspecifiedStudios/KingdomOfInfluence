using UnityEngine;



[CreateAssetMenu(menuName = "Attacks/Melee Weapon Attacks")]
public class MeleeWeaponAttackScriptableObject : ScriptableObject
{
    public enum AttackType
    {
        None,
        Single,
        Continous
    }
    public enum AttackClass
    {
        None,
        Light,
        Heavy,
        Special,
        Other
    }

    public int uniqueID;
    // need some sort of enum (continuous/immediate)
    public AttackType attackType; 
    public AttackClass attackClass;
    public float damage;
    public float duration;
    public float staminaCost;
    public float continousHitrate;
    public string debugMsg;
}
