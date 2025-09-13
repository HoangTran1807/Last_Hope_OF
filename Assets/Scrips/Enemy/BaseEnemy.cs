using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected int expDrop = 10;

    [Header("Spawn Cost")]
    [SerializeField] protected int infestedCost = 5; // số điểm cần để spawn
    public int InfestedCost => infestedCost;

    [Header("Flash Effect")]
    public SpriteRenderer spriteRenderer;
    public float flashTime = 0.1f;
    public float flashCouter = 0;

    [Header("swarpSystem")]
    [SerializeField] protected float maxDistanceFromPlayer = 15f;
    [SerializeField] protected float offset = 1f;


    protected Rigidbody2D rb;
    private GameObject originPrefab; // prefab gốc (dùng cho pool)

    public float Health => currentHealth;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(GameObject prefab)
    {
        originPrefab = prefab;
        currentHealth = maxHealth; // reset máu
        flashCouter = 0;
        spriteRenderer.color = Color.white;
        gameObject.SetActive(true);
    }

    public virtual void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        flashCouter = flashTime;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void DropOrb()
    {
        OrbPool.Instance.GetOrb(transform.position, expDrop);
    }

    protected abstract void Move(Transform player);
    protected abstract void CheckWrapPosition(Transform player);

    protected virtual void Die()
    {
        EnemyManager.Instance.UnregisterEnemy(this);
        EnemyManager.Instance.ReturnPoint(this.infestedCost);
        GameController.Instance.AddKilledEnemy();
        DropOrb();


        if (originPrefab != null)
            EnemyPoolManager.Instance.ReturnObject(originPrefab, gameObject);
        else
            Destroy(gameObject); // fallback
    }

    // Cho phép EnemyManager gọi mỗi frame
    public void DoMove(Transform player) => Move(player);
    public void DoSwap(Transform player) => CheckWrapPosition(player);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    // scaling được gọi từ EnemyManager khi spawn
    public virtual void ApplyScaling(float healthMult, float damageMult, float speedMult)
    {
        maxHealth *= healthMult;
        currentHealth = maxHealth;
        damage *= damageMult;
        moveSpeed *= speedMult;
    }
}
