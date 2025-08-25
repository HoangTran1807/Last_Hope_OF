using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStats PlayerStats;
    private float immunityCouter;

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
        if (immunityCouter > 0)
        {
            Debug.Log("player ignore damage");
            return;
        }
        damage = Mathf.Max(damage, 0f);

        PlayerStats.CurrentHealth -= damage;
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
        // Add death behavior here (disable player, trigger animation, etc.)
    }
}
