using UnityEngine;

public abstract class BaseGun : BaseWeapon
{
    protected GameObject bulletPrefab;
    protected float bulletSpeed;
    protected int damage;
    protected float attackRadius;
    protected float accuracy; // Độ chính xác (0 = bắn lệch nhiều, 1 = bắn chuẩn tuyệt đối)

    protected ITargetingStrategy targetingStrategy;

    public BaseGun(GameObject bulletPrefab, float bulletSpeed, int damage, float attackRadius, float cooldown, float accuracy = 1f)
        : base(cooldown)
    {
        this.bulletPrefab = bulletPrefab;
        this.bulletSpeed = bulletSpeed;
        this.damage = damage;
        this.attackRadius = attackRadius;
        this.accuracy = Mathf.Clamp01(accuracy);
    }

    public void SetTargetingStrategy(ITargetingStrategy strategy)
    {
        targetingStrategy = strategy;
    }

    protected BaseEnemy GetTarget(Vector3 playerPos)
    {
        if (targetingStrategy == null) return null;
        return targetingStrategy.GetTarget(playerPos, attackRadius);
    }

    protected void ShootBullet(Vector3 playerPos, Vector2 dir)
    {
        float inaccuracyAngle = (1f - accuracy) * 15f;
        float randomAngle = Random.Range(-inaccuracyAngle, inaccuracyAngle);
        Vector2 spreadDir = Quaternion.Euler(0, 0, randomAngle) * dir;

        GameObject bullet = BulletPoolManager.Instance.GetObject(bulletPrefab);
        bullet.transform.position = playerPos;
        bullet.transform.rotation = Quaternion.identity;

        if (bullet.TryGetComponent(out Bullet bulletScript))
        {
            bulletScript.Init(bulletPrefab, spreadDir, damage, bulletSpeed, 3f);
        }
    }

}
