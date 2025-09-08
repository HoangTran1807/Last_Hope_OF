using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;   // tốc độ bay
    [SerializeField] private float damage = 10f;  // sát thương
    [SerializeField] private float lifeTime = 3f; // thời gian tồn tại mặc định
    [SerializeField] private float rotationOffset = -90f;


    private Vector2 direction;
    private float timer;
    private GameObject originPrefab; // để biết viên đạn thuộc loại nào trong pool

    // --- Setup từ BaseGun khi bắn ---
    public void Init(GameObject prefab, Vector2 dir, float dmg, float speed, float lifeTime)
    {
        this.originPrefab = prefab;
        this.direction = dir.normalized;
        this.damage = dmg;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.timer = 0f;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // xoay viên đạn theo hướng bay
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // offset thêm góc (vd: -90, +90, -45 tuỳ sprite)
        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);

        // countdown lifeTime
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            ReturnToPool();
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
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
