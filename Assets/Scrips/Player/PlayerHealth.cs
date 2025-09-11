using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStats PlayerStats;
    private float immunityCouter;
    [SerializeField] private ParticleSystem hitEffect;

    void Start()
    {
        PlayerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (PlayerStats != null)
        {
            PlayerRegen();
            if (immunityCouter >  0)
            {
                immunityCouter = immunityCouter - Time.deltaTime;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        // n?u ng??i ch?i ?ang trong tr?ng thái kháng sát th??ng b? qua sát th??ng l?n này 
        if (immunityCouter > 0)
        {
            Debug.Log("player ignore damage");
            return;
        }


        damage = Mathf.Max(damage, 0f);

        PlayerStats.CurrentHealth -= damage;
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
        // n?u ng??i ch?i ch?t hi?n th? ui k?t qu? c?a game 
    }
}
