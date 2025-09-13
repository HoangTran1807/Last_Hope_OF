using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    [Header("Ranged Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float desiredDistance = 5f; 
    private float fireTimer;

    private void Start()
    {
        EnemyManager.Instance.RegisterEnemy(this);
        fireTimer = fireRate;
    }

    protected override void Move(Transform player)
    {
        if (player == null) return;

        Vector2 dirToPlayer = (player.position - transform.position);
        float distance = dirToPlayer.magnitude;

        if (distance > desiredDistance + 0.5f)
        {
            rb.linearVelocity = dirToPlayer.normalized * moveSpeed;
        }
        else if (distance < desiredDistance - 0.5f)
        {
            rb.linearVelocity = -dirToPlayer.normalized * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }


        if (dirToPlayer.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dirToPlayer.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);


        HandleShooting(player, dirToPlayer);
    }

    private void HandleShooting(Transform player, Vector2 dirToPlayer)
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot(dirToPlayer.normalized);
            fireTimer = fireRate;
        }
    }

    private void Shoot(Vector2 dir)
    {
        if (bulletPrefab == null || BulletPoolManager.Instance == null) return;

        // Lấy đạn từ pool
        GameObject bullet = BulletPoolManager.Instance.GetObject(bulletPrefab);
        bullet.transform.position = transform.position;

        // Xoay đạn
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Gán vận tốc và damage
        EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
        if (enemyBullet != null)
        {
            enemyBullet.Init(dir, bulletSpeed, 10f, bulletPrefab); // 10f là damage gốc của enemy
        }
    }





    protected override void CheckWrapPosition(Transform player)
    {
        if (player == null) return;

        Vector3 dir = transform.position - player.position;
        float dist = dir.magnitude;

        if (dist > maxDistanceFromPlayer)
        {
            dir = -dir.normalized * (maxDistanceFromPlayer - offset);
            transform.position = player.position + dir;
        }
    }
}
