using UnityEngine;

public enum EquipmentSlot
{
    None,
    Weapon
}

public class Weapons : MonoBehaviour
{
    public GameObject EquippedWeapon { get; private set; }

    public void EquipWeapon(GameObject weapon)
    {
        EquippedWeapon = weapon;
    }

    public void UnequipWeapon()
    {
        EquippedWeapon = null;
    }

    public bool HasWeapon()
    {
        return EquippedWeapon != null;
    }
}
