using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet Stats")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 3f;
    private float timer;

    private Rigidbody2D rb;

    [SerializeField] private GameObject prefabReference; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = lifeTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ReturnToPool();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                ReturnToPool();
            }
        }

        
    }

    public void Init(Vector2 direction, float speed, float dmg, GameObject prefab)
    {
        damage = dmg;
        prefabReference = prefab;

        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * speed;
        }
    }

    private void ReturnToPool()
    {
        rb.linearVelocity = Vector2.zero;
        if (prefabReference != null)
        {
            BulletPoolManager.Instance.ReturnObject(prefabReference, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
