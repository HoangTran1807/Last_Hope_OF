using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Weapons/Gun Data")]
public class GunData : ScriptableObject
{
    [Header("Gun Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public int damage = 10;
    public float attackRadius = 5f;
    [Range(0f, 1f)] public float accuracy = 1f;
    public int maxLevel;
    public string weaponID;

    [Header("Cooldown")]
    public float cooldown = 0.5f;

    [Header("Multi Shot")]
    public int bulletsPerShot = 1;
    public float spreadAngle = 10f;


    [Header("Targeting Strategy")]
    public string targetingStrategyID;
}
