using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;  // danh sách quái
    [SerializeField] private int maxEnemies = 20;
    [SerializeField] private Transform player;
    [SerializeField] private float minSpawnDistance = 20f;
    [SerializeField] private float maxSpawnDistance = 30f;

    private void Start()
    {
        // Tạo pool cho từng loại quái
        foreach (var prefab in enemyPrefabs)
        {
            EnemyPoolManager.Instance.CreatePool(prefab, maxEnemies);
        }
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
        if (player == null || enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        // chọn ngẫu nhiên prefab quái
        GameObject selectedPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // chọn vị trí spawn ngẫu nhiên quanh player
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector2 spawnPosition = (Vector2)player.position + randomDirection * randomDistance;

        // lấy quái từ pool
        GameObject enemyObj = EnemyPoolManager.Instance.GetObject(selectedPrefab);
        enemyObj.transform.position = spawnPosition;
        enemyObj.transform.rotation = Quaternion.identity;

        BaseEnemy enemy = enemyObj.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.Init(selectedPrefab); // truyền prefab gốc để biết loại
            EnemyManager.Instance.RegisterEnemy(enemy);
        }
    }
}
