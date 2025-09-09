using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int maxEnemies = 20;
    [SerializeField] private Transform player;
    [SerializeField] private float minSpawnDistance = 20f;
    [SerializeField] private float maxSpawnDistance = 30f;

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
        int currentEnemyCount = EnemyManager.Instance.GetEnemyCount();

        if (currentEnemyCount < maxEnemies)
        {
            TrySpawnEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        var state = EnemyManager.Instance.GetCurrentState();
        if (state == null || state.enemyPrefabs.Count == 0 || player == null) return;

        // chọn prefab ngẫu nhiên từ state hiện tại
        GameObject selectedPrefab = state.enemyPrefabs[Random.Range(0, state.enemyPrefabs.Count)];
        BaseEnemy enemyPrefab = selectedPrefab.GetComponent<BaseEnemy>();

        if (enemyPrefab == null) return;

        // kiểm tra điểm còn lại có đủ để spawn không
        if (!EnemyManager.Instance.TrySpawnEnemy(selectedPrefab, GetSpawnPosition()))
        {
            // Debug.Log("Không đủ infested point để spawn " + selectedPrefab.name);
        }
    }



    private Vector3 GetSpawnPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        return (Vector2)player.position + randomDirection * randomDistance;
    }
}
