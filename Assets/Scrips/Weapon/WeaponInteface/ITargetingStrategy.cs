using UnityEngine;
public interface ITargetingStrategy
{
    BaseEnemy GetTarget(Vector3 playerPos, float radius);
}

public class ClosestEnemyStrategy : ITargetingStrategy
{
    public BaseEnemy GetTarget(Vector3 playerPos, float radius)
    {
        BaseEnemy[] enemies = GameObject.FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None);
        BaseEnemy closest = null;
        float minDist = Mathf.Infinity;

        foreach (BaseEnemy e in enemies)
        {
            float dist = Vector2.Distance(playerPos, e.transform.position);
            if (dist < minDist && dist <= radius)
            {
                minDist = dist;
                closest = e;
            }
        }
        return closest;
    }
}

public class HighestHealthEnemyStrategy : ITargetingStrategy
{
    public BaseEnemy GetTarget(Vector3 playerPos, float radius)
    {
        BaseEnemy[] enemies = GameObject.FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None);
        BaseEnemy target = null;
        float maxHp = -1;

        foreach (BaseEnemy e in enemies)
        {
            float dist = Vector2.Distance(playerPos, e.transform.position);
            if (dist <= radius && e.Health > maxHp)
            {
                maxHp = e.Health;
                target = e;
            }
        }
        return target;
    }
}


public class ClusteredEnemyStrategy : ITargetingStrategy
{
    public BaseEnemy GetTarget(Vector3 playerPos, float radius)
    {
        BaseEnemy[] enemies = GameObject.FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None);

        Vector2 avgDir = Vector2.zero;
        int count = 0;

        foreach (BaseEnemy e in enemies)
        {
            float dist = Vector2.Distance(playerPos, e.transform.position);
            if (dist <= radius)
            {
                avgDir += (Vector2)(e.transform.position - playerPos);
                count++;
            }
        }

        if (count == 0) return null;

        avgDir.Normalize();

        // chọn kẻ địch nào gần nhất theo hướng "avgDir"
        BaseEnemy bestTarget = null;
        float bestDot = -1f;

        foreach (BaseEnemy e in enemies)
        {
            Vector2 dir = (e.transform.position - playerPos).normalized;
            float dot = Vector2.Dot(avgDir, dir);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestTarget = e;
            }
        }
        return bestTarget;
    }
}
