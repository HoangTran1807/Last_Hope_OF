using UnityEngine;

public class ConeAOEWeapon : BaseWeapon
{
    [Header("Weapon Data Template")]
    [SerializeField] private ConeAOEWeaponData weaponData;


    [Header("Runtime Stats")]
    [SerializeField] private float damagePerShot;
    [SerializeField] private float attackRadius;
    [SerializeField] private float coneAngle;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private ParticleSystem effectPrefab;
    [SerializeField] private string targetingStrategyID;

    private ITargetingStrategy targetingStrategy;
    private BaseEnemy currentTarget;


    private void Awake()
    {
        if (weaponData != null)
        {
            cooldown           = weaponData.cooldown;
            upgradeable = weaponData.upgradeable;
            maxLevel           = weaponData.maxLevel;
            weaponID           = weaponData.weaponID;
            
            damagePerShot      = weaponData.damagePerShot;
            attackRadius       = weaponData.attackRadius;
            coneAngle          = weaponData.coneAngle;
            enemyLayer         = weaponData.enemyLayer;
            targetingStrategyID = weaponData.targetingStrategyID;

            if (weaponData.effectPrefab != null)
            {
                effectPrefab = Instantiate(weaponData.effectPrefab, transform);
                effectPrefab.Stop();
            }
        }
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(targetingStrategyID))
        {
            targetingStrategy = CreateTargetingStrategy(targetingStrategyID);
        }
        else
        {
            targetingStrategy = new ClosestEnemyStrategy();
        }
    }

    private ITargetingStrategy CreateTargetingStrategy(string id)
    {
        switch (id)
        {
            case "ClosestEnemy": return new ClosestEnemyStrategy();
            case "HighestHealth": return new HighestHealthEnemyStrategy();
            case "ClusteredEnemy": return new ClusteredEnemyStrategy();
            default:
                Debug.LogWarning($"Targeting strategy '{id}' not found. Using ClosestEnemyStrategy.");
                return new ClosestEnemyStrategy();
        }
    }

    protected override void Fire(Vector3 playerPos)
    {
        // Hướng bắn dựa vào target
        currentTarget = targetingStrategy?.GetTarget(playerPos, attackRadius);
        if (currentTarget  == null) return;
        Vector3 fireDir = currentTarget  != null
            ? (currentTarget.transform.position - playerPos).normalized
            : transform.right;

        // Quét enemy trong phạm vi
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerPos, attackRadius, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out BaseEnemy enemy))
            {
                Vector3 toEnemy = (enemy.transform.position - playerPos).normalized;
                float angle = Vector3.Angle(fireDir, toEnemy);

                if (angle <= coneAngle * 0.5f)
                {
                    enemy.TakeDamage(damagePerShot);
                    // enemy.ApplyStatusEffect(StatusEffectType.Burn, 2f, damagePerShot * 0.5f);
                }
            }
        }

        // Hiệu ứng
        if (effectPrefab != null)
        {

            Vector2 direction =  currentTarget.transform.position -  playerPos;
            effectPrefab.transform.rotation = Quaternion.LookRotation(direction);


            effectPrefab.transform.localScale =  new Vector3(attackRadius /5, attackRadius /5, 1);

            effectPrefab.Play();
        }



    }

    public override void ApplyUpgrade(UpgradeData upgrade)
    {
        if (IsMaxLevel) return;

        switch (upgrade.upgradeType)
        {
            case UpgradeType.Damage:
                damagePerShot += upgrade.value;
                break;
            case UpgradeType.ConeAngle:
                coneAngle += upgrade.value;
                break;
            case UpgradeType.Range:
                attackRadius += upgrade.value;
                break;
        }

        currentLevel++;
        Debug.Log($"{weaponID} upgraded (ConeAOE) → Level {currentLevel}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        if (currentTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, currentTarget.transform.position);
            Gizmos.DrawSphere(currentTarget.transform.position, 0.2f);
        }
    }

}
