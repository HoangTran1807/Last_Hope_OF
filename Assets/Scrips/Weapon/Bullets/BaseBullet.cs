using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    [Header("Common Settings")]
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float lifeTime = 3f;
    [SerializeField] protected float rotationOffset = -90f;

    protected Vector2 direction;
    protected float timer;
    protected GameObject originPrefab;

    // --- Setup từ vũ khí khi bắn ---
    public virtual void Init(GameObject prefab, Vector2 dir, float dmg, float speed, float lifeTime)
    {
        this.originPrefab = prefab;
        this.direction = dir.normalized;
        this.damage = dmg;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.timer = 0f;
    }

    protected virtual void Update()
    {
        // bay theo hướng
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // xoay viên đạn theo hướng bay
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);

        // countdown lifeTime
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            OnExpire();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            OnHitEnemy(enemy);
        }
    }

    // --- Các hàm abstract / virtual để override ---
    protected abstract void OnHitEnemy(BaseEnemy enemy);
    protected abstract void OnExpire();

    // --- Return to pool ---
    protected void ReturnToPool()
    {
        if (originPrefab != null)
        {
            BulletPoolManager.Instance.ReturnObject(originPrefab, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
