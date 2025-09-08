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
        if (upgrade.isWeaponUpgrade)
        {
            // tìm các vũ khí có id = name 

            var gun = weapons.Find(w => w.WeaponID == upgrade.upgradeName);
            // nếu tìm thấy nghĩa là đã có vũ khí này rồi chỉ áp dung hiệu ứng nâng cấp 
            if (gun != null)
            {
                gun.ApplyUpgrade(upgrade);
            }
            // ngược lại nếu ko tìm thấy nghĩa là không có vũ khí này thêm vũ khí vào list vk
            else
            {
                if (!IsWeaponFull())
                    AddWeapon(upgrade.upgradeName);
            }
        }
        else
        {
            // TODO: Apply player passive upgrade (HP, MoveSpeed, EXP Range...)
            Debug.Log($"Apply passive upgrade: {upgrade.upgradeName}");
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
            Debug.Log(weapon.name);
            weapon.UpdateWeapon(transform.position);
        }
    }



}
