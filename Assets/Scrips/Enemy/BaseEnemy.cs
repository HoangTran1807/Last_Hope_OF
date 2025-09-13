using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float baseSpeed = 2f;
    [SerializeField] protected int expDrop = 10;

    [Header("Runtime Stats")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float damage;
    [SerializeField] protected float moveSpeed;

    [Header("Spawn Cost")]
    [SerializeField] protected int infestedCost = 5;
    public int InfestedCost => infestedCost;

    [Header("Flash Effect")]
    public SpriteRenderer spriteRenderer;
    public float flashTime = 0.1f;
    public float flashCouter = 0;

    [Header("Wrap System")]
    [SerializeField] protected float maxDistanceFromPlayer = 15f;
    [SerializeField] protected float offset = 1f;

    protected Rigidbody2D rb;
    private GameObject originPrefab;

    public float Health => currentHealth;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(GameObject prefab)
    {
        originPrefab = prefab;

        // reset về base stats mỗi lần spawn
        maxHealth = baseHealth;
        currentHealth = maxHealth;
        damage = baseDamage;
        moveSpeed = baseSpeed;

        flashCouter = 0;
        if (spriteRenderer != null)
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
            Destroy(gameObject);
    }

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

    // scale dựa trên base stats, không nhân chồng
    public virtual void ApplyScaling(float healthMult, float damageMult, float speedMult)
    {
        maxHealth = baseHealth * healthMult;
        currentHealth = maxHealth;
        damage = baseDamage * damageMult;
        moveSpeed = baseSpeed * speedMult;
    }
}
