using UnityEngine;

public enum UpgradeCategory
{
    Weapon_Upgrade,   // Nâng cấp vũ khí
    Player_Upgrade    // Nâng cấp chỉ số người chơi (HP, Speed, EXP, ...)
}

public enum UpgradeType
{
    // ==== Weapon related ====
    Damage,
    FireRate,
    Accuracy,
    Projectile,
    ConeAngle,
    Range,

    // ==== Player stats ====
    MaxHealth,
    MoveSpeed,
    PickupRange,
    Armor,
    Regen,
    CritChance,
    CritDamage,
    ExpGain
}

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Upgrades/New Upgrade")]
public class UpgradeData : ScriptableObject
{
    [Header("General Info")]
    public string upgradeName;          // Tên vũ khí hoặc tên nâng cấp
    public string description;          // Mô tả
    public UpgradeCategory category;    // Loại nâng cấp
    public int maxLevel = 5;            // Giới hạn cấp độ
    public Sprite icon;

    [Header("Effect")]
    public UpgradeType upgradeType;     // Tác động đến chỉ số nào
    public float value;                 // Giá trị tăng mỗi cấp (HP +x, Speed +x, FireRate *x, ...)
}
