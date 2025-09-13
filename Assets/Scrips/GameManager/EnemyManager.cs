using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyState
    {
        public string stateName;
        public List<GameObject> enemyPrefabs;
        public float spawnCooldown = 3f; // thời gian chờ giữa các lần spawn
    }

    public static EnemyManager Instance;

    private List<BaseEnemy> activeEnemies = new List<BaseEnemy>();
    private float gameTimer = 0f;

    [Header("Infested Point System")]
    [SerializeField] private int startingPoints = 10;     // điểm ban đầu
    [SerializeField] private float pointsPerSecond = 0.5f; // tăng dần theo thời gian
    [SerializeField]
    private float currentPoints;

    [Header("State System")]
    [SerializeField] private float stateDuration = 300f; // 5 phút = 300 giây
    [SerializeField]
    private int currentStateIndex = 0;

  



    [SerializeField] public List<EnemyState> states;
    private Transform player;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else Destroy(gameObject);

        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Start()
    {
        currentPoints = startingPoints;
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;
        currentPoints += pointsPerSecond * Time.deltaTime;

        int newStateIndex = Mathf.FloorToInt(gameTimer / stateDuration);
        if (newStateIndex != currentStateIndex && newStateIndex < states.Count)
        {
            currentStateIndex = newStateIndex;
            Debug.Log("Đã chuyển sang state: " + states[currentStateIndex].stateName);
        }

        // EnemyManager vẫn quản lý move
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            BaseEnemy enemy = activeEnemies[i];
            if (enemy == null) continue;

            enemy.DoMove(player);
            enemy.DoSwap(player);
            

            if (enemy.flashCouter > 0)
            {
                enemy.spriteRenderer.color = Color.red;
                enemy.flashCouter -= Time.deltaTime;
            }
            else
            {
                enemy.spriteRenderer.color = Color.white;
            }
        }
    }

    public void RegisterEnemy(BaseEnemy enemy)
    {
        if (!activeEnemies.Contains(enemy))
            activeEnemies.Add(enemy);

        ApplyScaling(enemy);
    }

    public void UnregisterEnemy(BaseEnemy enemy)
    {
        if (activeEnemies.Contains(enemy))
            activeEnemies.Remove(enemy);
    }

    public int GetEnemyCount() => activeEnemies.Count;
    public List<BaseEnemy> GetEnemies() => activeEnemies;

    private void ApplyScaling(BaseEnemy enemy)
    {
        float minutes = gameTimer / 60f;

        // Ví dụ exponential scaling
        float healthMultiplier = Mathf.Pow(1f + 0.1f, minutes);
        float damageMultiplier = Mathf.Pow(1f + 0.08f, minutes);
        float speedMultiplier = Mathf.Pow(1f + 0.03f, minutes);

        enemy.ApplyScaling(healthMultiplier, damageMultiplier, speedMultiplier);
    }

    // 👇 Hàm này để Spawner gọi khi muốn sinh quái
    public bool TrySpawnEnemy(GameObject prefab, Vector3 position)
    {
        BaseEnemy enemyPrefab = prefab.GetComponent<BaseEnemy>();
        if (enemyPrefab == null) return false;

        // kiểm tra điểm đủ để spawn chưa
        if (currentPoints < enemyPrefab.InfestedCost)
            return false;

        // trừ điểm
        currentPoints -= enemyPrefab.InfestedCost;

        // lấy enemy từ pool thay vì Instantiate
        GameObject obj = EnemyPoolManager.Instance.GetObject(prefab);

        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        // reset enemy
        BaseEnemy newEnemy = obj.GetComponent<BaseEnemy>();
        if (newEnemy != null)
        {
            newEnemy.Init(prefab);
            RegisterEnemy(newEnemy);
        }

        return true;
    }


    public EnemyState GetCurrentState()
    {
        if (currentStateIndex >= 0 && currentStateIndex < states.Count)
            return states[currentStateIndex];
        return null;
    }



    public void ReturnPoint(float point)
    {
        currentPoints += point;
    }

    
}
