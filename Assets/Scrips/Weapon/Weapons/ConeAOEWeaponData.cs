using UnityEngine;

[CreateAssetMenu(fileName = "NewConeAOEWeaponData", menuName = "Weapons/Cone AOE Weapon Data")]
public class ConeAOEWeaponData : ScriptableObject
{
    [Header("General Settings")]
    public string weaponID = "ConeAOEWeapon";
    public float cooldown = 0.2f;   // tốc độ bắn (giữa 2 lần Fire)
    public int maxLevel = 5;

    [Header("Attack Settings")]
    public float damagePerShot = 5f;   // damage mỗi lần Fire
    public float attackRadius = 3f;    // bán kính AOE
    public float coneAngle = 60f;      // góc hình nón

    [Header("Targeting")]
    public string targetingStrategyID = "ClosestEnemy";

    [Header("Visual")]
    public ParticleSystem effectPrefab;
    public LayerMask enemyLayer;
}
