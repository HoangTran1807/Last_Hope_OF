using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance;

    [SerializeField] private int maxWeapons = 6;
    [SerializeField]
    private List<BaseWeapon> weapons = new List<BaseWeapon>();

    private void Awake()
    {
        Instance = this;
    }

    public bool IsWeaponFull() => weapons.Count >= maxWeapons;

    public bool HasWeapon(string weaponID)
    {
        return weapons.Exists(w => w.WeaponID == weaponID);
    }

    public bool CanUpgrade(string weaponID)
    {
        var gun = weapons.Find(w => w.WeaponID == weaponID);
        return gun != null && !gun.IsMaxLevel;
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        // Tìm vũ khí có sẵn
        var gun = weapons.Find(w => w.WeaponID == upgrade.upgradeName);

        // Nếu tìm thấy, áp dụng nâng cấp
        if (gun != null)
        {
            gun.ApplyUpgrade(upgrade);
        }
        // Nếu không tìm thấy, đây là vũ khí mới
        else
        {
            if (!IsWeaponFull())
            {
                AddWeapon(upgrade.upgradeName);
            }
        }
    }

    public void AddWeapon(string weaponID)
    {
        var newGunPrefab = Resources.Load<BaseWeapon>($"Weapons/{weaponID}");
        if (newGunPrefab == null)
        {
            Debug.LogError($"Weapon prefab not found at Resources/Weapons/{weaponID}");
            return;
        }

        BaseWeapon gun = Instantiate(newGunPrefab, transform);
        weapons.Add(gun);
    }

    void Update()
    {
        foreach (BaseWeapon weapon in weapons)
        {
            weapon.UpdateWeapon(transform.position);
        }
    }



}
