using UnityEngine;

public class Zombie : BaseEnemy
{
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        EnemyManager.Instance.RegisterEnemy(this);
    }

    protected override void Move()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;

        // 👇 Xử lý flip sprite
        if (dir.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}
