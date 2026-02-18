using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Melee Weapon Attacks")]
public class MeleeWeaponAttackScriptableObject : ScriptableObject
{
    public AnimatorOverrideController animatorOverride;
    public float damage;    //Not being used at the moment; keeping this here for later for modular combat system rework
    public float duration;
    public string debugMsg;

}
