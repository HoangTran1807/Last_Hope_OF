using UnityEngine;

public class RifleBullet : BaseBullet
{
    protected override void OnHitEnemy(BaseEnemy enemy)
    {
        enemy.TakeDamage(damage);
        ReturnToPool();
    }

    protected override void OnExpire()
    {
        ReturnToPool();
    }
}
