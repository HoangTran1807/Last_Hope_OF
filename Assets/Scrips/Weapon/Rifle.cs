using UnityEngine;

public class Rifle : BaseGun
{
    public Rifle(GameObject bulletPrefab, float bulletSpeed, int damage, float attackRadius, float cooldown, float accuracy)
        : base(bulletPrefab, bulletSpeed, damage, attackRadius, cooldown, accuracy) { }

    protected override void Fire(Vector3 playerPos)
    {
        BaseEnemy target = GetTarget(playerPos);
        if (target == null) return;

        Vector2 dir = (target.transform.position - playerPos).normalized;
        ShootBullet(playerPos, dir);
    }
}
