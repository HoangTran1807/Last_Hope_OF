using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemies = 20;
    [SerializeField] private Transform player;
    [SerializeField] private float minSpawnDistance = 20f;
    [SerializeField] private float maxSpawnDistance = 30f;

    private void Start()
    {
        // Tạo pool sẵn
        EnemyPoolManager.Instance.CreatePool(enemyPrefab, maxEnemies);
    }

    private void Update()
    {
        int currentEnemyCount = EnemyManager.Instance.GetEnemyCount();

        if (currentEnemyCount < maxEnemies)
        {
            int enemiesToSpawn = maxEnemies - currentEnemyCount;
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        if (player == null || enemyPrefab == null) return;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector2 spawnPosition = (Vector2)player.position + randomDirection * randomDistance;

        GameObject enemyObj = EnemyPoolManager.Instance.GetObject(enemyPrefab);
        enemyObj.transform.position = spawnPosition;
        enemyObj.transform.rotation = Quaternion.identity;

        BaseEnemy enemy = enemyObj.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.Init(enemyPrefab);
            EnemyManager.Instance.RegisterEnemy(enemy);
        }
    }
}
