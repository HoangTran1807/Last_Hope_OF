using UnityEngine;

public class GrenadeBullet : BaseBullet
{
    [Header("Grenade Settings")]
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private LayerMask enemyLayer;
    private bool exploded = false;

    protected override void OnHitEnemy(BaseEnemy enemy)
    {
        Explode();
    }

    protected override void OnExpire()
    {
        Explode();
    }

    private void Explode()
    {
        if (exploded) return;
        exploded = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (var hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }


        Debug.Log("Grenade exploded!");

        ReturnToPool();
        exploded = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
