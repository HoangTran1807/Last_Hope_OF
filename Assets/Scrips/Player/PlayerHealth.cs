using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStats PlayerStats;
    private float immunityCouter;

    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private Collider2D playerCollider; // 👈 tham chiếu Collider

    private Color originalColor;

    private void Awake()
    {
        PlayerStats = GetComponent<PlayerStats>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>(); // lấy collider của Player
    }

    void Start()
    {
        if (playerRenderer != null)
            originalColor = playerRenderer.color;
    }

    private void Update()
    {
        if (PlayerStats != null)
        {
            PlayerRegen();

            if (immunityCouter > 0)
            {
                immunityCouter -= Time.deltaTime;

                // hiệu ứng làm mờ (nhấp nháy alpha)
                if (playerRenderer != null)
                {
                    float t = Mathf.PingPong(Time.time * 5f, 1f);
                    Color c = originalColor;
                    c.a = Mathf.Lerp(0.3f, 1f, t);
                    playerRenderer.color = c;
                }

                // tắt collider trong lúc miễn nhiễm
                if (playerCollider != null && playerCollider.enabled)
                    playerCollider.enabled = false;
            }
            else
            {
                // hết miễn nhiễm -> trả lại màu gốc
                if (playerRenderer != null)
                    playerRenderer.color = originalColor;

                // bật lại collider
                if (playerCollider != null && !playerCollider.enabled)
                    playerCollider.enabled = true;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (immunityCouter > 0)
        {
            Debug.Log("player ignore damage");
            return;
        }

        AudioManager.Instance.PlaySE("playerHurt");
        damage = Mathf.Max(damage, 0f);

        PlayerStats.CurrentHealth -= damage;

        if (hitEffect != null)
            hitEffect.Play();

        Debug.Log("Take damage");
        immunityCouter = PlayerStats.ImmunityTime;

        if (PlayerStats.CurrentHealth <= 0)
        {
            PlayerStats.CurrentHealth = 0;
            Die();
        }
    }

    public void PlayerRegen()
    {
        if (PlayerStats.CurrentHealth < PlayerStats.MaxHealth)
        {
            PlayerStats.CurrentHealth += PlayerStats.RegenPerSecond * Time.fixedDeltaTime;

            if (PlayerStats.CurrentHealth > PlayerStats.MaxHealth)
            {
                PlayerStats.CurrentHealth = PlayerStats.MaxHealth;
            }
        }
    }

    public void Die()
    {
        Debug.Log("Player Died");
        // show UI kết thúc game ở đây
    }
}
