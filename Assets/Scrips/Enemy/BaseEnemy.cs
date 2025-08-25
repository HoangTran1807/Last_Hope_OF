using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float moveSpeed = 2f;
    public float damage = 10f;
    protected Rigidbody2D rb;

    private GameObject originPrefab; // lưu prefab để return đúng pool

    public float Health => currentHealth;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(GameObject prefab)
    {
        originPrefab = prefab;
        currentHealth = maxHealth; // reset máu mỗi lần spawn lại
    }

    public virtual void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected abstract void Move();

    protected virtual void Die()
    {
        EnemyManager.Instance.UnregisterEnemy(this);
        if (originPrefab != null)
            EnemyPoolManager.Instance.ReturnObject(originPrefab, gameObject);
        else
            Destroy(gameObject); // fallback
    }

    // Cho phép EnemyManager gọi
    public void DoMove() => Move();

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
}
