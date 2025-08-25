using UnityEngine;
using System.Linq;

public class HighestHpTargetingStrategy : ITargetingStrategy
{
    public BaseEnemy GetTarget(Vector3 playerPos, float attackRadius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerPos, attackRadius);

        BaseEnemy highestHpEnemy = null;
        float maxHp = -1;

        foreach (var hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
            if (enemy != null && enemy.Health > maxHp)
            {
                maxHp = enemy.Health;
                highestHpEnemy = enemy;
            }
        }

        return highestHpEnemy;
    }
}
