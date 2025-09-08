using UnityEngine;
public enum UpgradeCategory
{
    Weapon_New,   // Mở thêm vũ khí mới
    Weapon_Upgrade, // Nâng cấp vũ khí đang có
    Player_Upgrade  // Nâng cấp nội tại (HP, Speed, EXP gain…)
}

public enum UpgradeType
{
    Damage,
    FireRate,
    Accuracy,
    Projectile,
    ConeAngle,
    Range
}


[CreateAssetMenu(fileName = "UpgradeData", menuName = "Upgrades/New Upgrade")]
public class UpgradeData : ScriptableObject
{
    [Header("General Info")]
    public string upgradeName;          // tên vũ khí hoặc nâng cấp
    public string description;          // mô tả nâng cấp
    public bool isWeaponUpgrade;        // true = vũ khí, false = nâng cấp nội tại
    public int maxLevel = 5;
    public Sprite icon;

    [Header("Effect")]
    public UpgradeType upgradeType;
    public float value;                 // giá trị tăng (damage +x, fireRate *factor, ...)
}