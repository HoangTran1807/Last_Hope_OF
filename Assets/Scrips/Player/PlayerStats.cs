using UnityEngine;

[System.Serializable]
public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    [Header("Core Stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private float fireRateMultiplier = 1f;
    [SerializeField] private float areaMultiplier = 1f;
    [SerializeField] private int projectileAmount = 1;    
    [SerializeField] private float pickupRange = 2f;

    [Header("Defense")]
    [SerializeField] private int armor = 0;
    [SerializeField] private float regenPerSecond = 0f;
    [SerializeField] private float immunityTime = 0.5f;

    [Header("Critical")]
    [SerializeField] private float critChance = 0f;       // %
    [SerializeField] private float critDamageMultiplier = 2f;
    [SerializeField] private float expMultiplier = 1f;

    // =============== GETTERS & SETTERS =================
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = Mathf.Clamp(value, 0, maxHealth); }

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float SpeedMultiplier { get => speedMultiplier; set => speedMultiplier = value; }
    public float DamageMultiplier { get => damageMultiplier; set => damageMultiplier = value; }
    public float FireRateMultiplier { get => fireRateMultiplier; set => fireRateMultiplier = value; }
    public float AreaMultiplier { get => areaMultiplier; set => areaMultiplier = value; }
    public int ProjectileAmount { get => projectileAmount; set => projectileAmount = value; }
    public float PickupRange { get => pickupRange; set => pickupRange = value; }

    public int Armor { get => armor; set => armor = value; }
    public float RegenPerSecond { get => regenPerSecond; set => regenPerSecond = value; }
    public float ImmunityTime { get => immunityTime; set => immunityTime = value; }

    public float CritChance { get => critChance; set => critChance = value; }
    public float CritDamageMultiplier { get => critDamageMultiplier; set => critDamageMultiplier = value; }
    public float ExpMultiplier { get => expMultiplier; set => expMultiplier = value; }
}
