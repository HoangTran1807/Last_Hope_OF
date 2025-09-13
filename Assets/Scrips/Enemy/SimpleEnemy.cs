using UnityEngine;

public class SimpleEnemy : BaseEnemy
{

    private void Start()
    {
        EnemyManager.Instance.RegisterEnemy(this);
    }

    protected override void Move(Transform player)
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;

        if (dir.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    protected override void CheckWrapPosition(Transform player)
    {
        if (player == null) return;

        Vector3 dir = transform.position - player.position;
        float dist = dir.magnitude;

        if (dist > maxDistanceFromPlayer)
        {
            // đảo hướng và rút ngắn
            dir = -dir.normalized * (maxDistanceFromPlayer - offset);

            // cập nhật vị trí quái
            transform.position = player.position + dir;
        }
    }
}
