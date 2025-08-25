using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private List<BaseEnemy> activeEnemies = new List<BaseEnemy>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterEnemy(BaseEnemy enemy)
    {
        if (!activeEnemies.Contains(enemy))
            activeEnemies.Add(enemy);
    }

    public void UnregisterEnemy(BaseEnemy enemy)
    {
        if (activeEnemies.Contains(enemy))
            activeEnemies.Remove(enemy);
    }

    // 👇 Hàm này để EnemySpawner có thể kiểm tra số lượng enemy hiện có
    public int GetEnemyCount()
    {
        return activeEnemies.Count;
    }

    // Nếu cần lấy danh sách enemy
    public List<BaseEnemy> GetEnemies()
    {
        return activeEnemies;
    }

    private void Update()
    {
        // ✅ Chỉ EnemyManager điều khiển di chuyển
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            activeEnemies[i].DoMove();
        }
    }
}
