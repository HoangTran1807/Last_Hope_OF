using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int maxEnemies = 20;
    [SerializeField] private Transform player;
    [SerializeField] private float minSpawnDistance = 20f;
    [SerializeField] private float maxSpawnDistance = 30f;

    private float spawnTimer = 0f;

    private void Start()
    {
        // tạo pool cho từng loại quái trong tất cả state
        foreach (var state in EnemyManager.Instance.states)
        {
            foreach (var prefab in state.enemyPrefabs)
            {
                EnemyPoolManager.Instance.CreatePool(prefab, maxEnemies);
            }
        }
    }

    private void Update()
    {
        if (player == null) return;

        spawnTimer -= Time.deltaTime;

        int currentEnemyCount = EnemyManager.Instance.GetEnemyCount();
        if (currentEnemyCount < maxEnemies && spawnTimer <= 0f)
        {
            if (TrySpawnEnemy())
            {
                // lấy cooldown theo state hiện tại
                var state = EnemyManager.Instance.GetCurrentState();
                if (state != null)
                {
                    spawnTimer = state.spawnCooldown;
                }
            }
        }
    }

    private bool TrySpawnEnemy()
    {
        var state = EnemyManager.Instance.GetCurrentState();
        if (state == null || state.enemyPrefabs.Count == 0) return false;

        GameObject selectedPrefab = state.enemyPrefabs[Random.Range(0, state.enemyPrefabs.Count)];
        return EnemyManager.Instance.TrySpawnEnemy(selectedPrefab, GetSpawnPosition());
    }



    private Vector3 GetSpawnPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        return (Vector2)player.position + randomDirection * randomDistance;
    }
}
