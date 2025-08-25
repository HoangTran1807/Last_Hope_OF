using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]
    private List<BaseWeapon> weapons = new List<BaseWeapon>();

    void Update()
    {
        foreach (BaseWeapon weapon in weapons)
        {
            weapon.UpdateWeapon(transform.position);
        }
    }

    public void AddWeapon(BaseWeapon newWeapon)
    {
        weapons.Add(newWeapon);
    }
}
