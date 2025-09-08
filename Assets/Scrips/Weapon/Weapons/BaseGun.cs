using UnityEngine;

public class BaseGun : BaseWeapon
{
    [Header("Gun Data Template")]
    [SerializeField] private GunData gunData;


    [Header("Runtime Stats")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int bulletsPerShot;
    [SerializeField] private float spreadAngle;
    [SerializeField, Range(0f, 1f)] private float accuracy;
    [SerializeField] private float attackRadius;
    [SerializeField] private string targetingStrategyID;



    private ITargetingStrategy targetingStrategy;

    private void Awake()
    {
        if (gunData != null)
        {
            bulletPrefab   = gunData.bulletPrefab;
            damage         = gunData.damage;
            bulletSpeed    = gunData.bulletSpeed;
            cooldown       = gunData.cooldown;   // từ BaseWeapon
            bulletsPerShot = gunData.bulletsPerShot;
            spreadAngle    = gunData.spreadAngle;
            accuracy       = gunData.accuracy;
            attackRadius   = gunData.attackRadius;
            maxLevel       = gunData.maxLevel;
            weaponID       = gunData.weaponID;
            targetingStrategyID = gunData.targetingStrategyID;


        }
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(targetingStrategyID))
        {
            targetingStrategy = CreateTargetingStrategy(targetingStrategyID);
        }
        else
        {
            targetingStrategy = new ClosestEnemyStrategy();
        }
    }

    private ITargetingStrategy CreateTargetingStrategy(string id)
    {
        switch (id)
        {
            case "ClosestEnemy":
                return new ClosestEnemyStrategy();
            case "HighestHealth":
                return new HighestHealthEnemyStrategy();
            case "ClusteredEnemy":
                return new ClusteredEnemyStrategy();
            default:
                // Trả về chiến lược mặc định nếu không tìm thấy ID
                Debug.LogWarning($"Targeting strategy ID '{id}' not found. Using ClosestEnemyStrategy.");
                return new ClosestEnemyStrategy();
        }
    }

    protected override void Fire(Vector3 playerPos)
    {
        if (bulletsPerShot <= 0) return;
        Debug.Log("player post");
        BaseEnemy target = GetTarget(playerPos);
        if (target == null) return;

        Vector2 dir = (target.transform.position - playerPos).normalized;

        if (bulletsPerShot <= 1)
        {
            ShootBullet(playerPos, dir);
        }
        else
        {
            float halfSpread = spreadAngle * 0.5f;
            for (int i = 0; i < bulletsPerShot; i++)
            {
                float t = (float)i / (bulletsPerShot - 1);
                float angleOffset = Mathf.Lerp(-halfSpread, halfSpread, t);
                Vector2 spreadDir = Quaternion.Euler(0, 0, angleOffset) * dir;
                ShootBullet(playerPos, spreadDir);
            }
        }
    }

    private BaseEnemy GetTarget(Vector3 playerPos)
    {
        if (targetingStrategy == null) return null;
        return targetingStrategy.GetTarget(playerPos, attackRadius);
    }

    private void ShootBullet(Vector3 playerPos, Vector2 dir)
    {
        Debug.Log("shot a bullet");

        float inaccuracyAngle = (1f - accuracy) * 15f;
        float randomAngle = Random.Range(-inaccuracyAngle, inaccuracyAngle);
        Vector2 spreadDir = Quaternion.Euler(0, 0, randomAngle) * dir;

        GameObject bullet = SpawnFromPool(bulletPrefab, playerPos, Quaternion.identity);
        if (bullet != null && bullet.TryGetComponent(out BaseBullet bulletScript))
        {
            bulletScript.Init(bulletPrefab, spreadDir, damage, bulletSpeed, 3f);
        }
    }


    public void SetTargetingStrategy(ITargetingStrategy strategy)
    {
        targetingStrategy = strategy;
    }

    // -------------------------
    // Runtime stat upgrades
    // -------------------------
    public void UpgradeDamage(float amount) => damage += amount;
    public void UpgradeFireRate(float factor)
    => cooldown = Mathf.Max(0.05f, cooldown / Mathf.Max(factor, 0.01f));
    public void UpgradeAccuracy(float amount) => accuracy = Mathf.Clamp01(accuracy + amount);

    // -------------------------
    // Upgrade System
    // -------------------------

    public override void ApplyUpgrade(UpgradeData upgrade)
    {
        if (IsMaxLevel) return;

        switch (upgrade.upgradeType)
        {
            case UpgradeType.Damage:
                UpgradeDamage(upgrade.value);
                break;
            case UpgradeType.FireRate:
                UpgradeFireRate(upgrade.value);
                break;
            case UpgradeType.Projectile:
                bulletsPerShot += Mathf.RoundToInt(upgrade.value);
                break;
        }

        currentLevel++;
        Debug.Log($"{weaponID} upgraded (Gun) → Level {currentLevel}");
    }

}
