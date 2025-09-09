using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    public static PlayerUpgradeManager Instance;

    [SerializeField] private int maxPassiveCount = 6;

    // passive hiện tại và level
    private Dictionary<string, int> passiveLevels = new Dictionary<string, int>();

    private PlayerStats stats;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        stats = PlayerStats.Instance;
    }

    public bool HasPassive(string name) => passiveLevels.ContainsKey(name);
    public bool IsPassiveFull() => passiveLevels.Count >= maxPassiveCount;

    public bool CanUpgrade(UpgradeData passive)
    {
        if (!passiveLevels.ContainsKey(passive.upgradeName))
        {
            bool canTake = !IsPassiveFull();
            return canTake;
        }

        bool canLevelUp = passiveLevels[passive.upgradeName] < passive.maxLevel;
        return canLevelUp;
    }

    public void ApplyUpgrade(UpgradeData passive)
    {
        if (!passiveLevels.ContainsKey(passive.upgradeName))
        {
            passiveLevels[passive.upgradeName] = 1;
            Debug.Log($"[ApplyUpgrade] Thêm passive mới: {passive.upgradeName} (Lv.1)");
            // số lượng upgrade hiện tại 
            Debug.Log($"số bị động hiện tại {passiveLevels.Count} số bị động tối đa {maxPassiveCount}");
        }
        else
        {
            passiveLevels[passive.upgradeName]++;
            Debug.Log($"[ApplyUpgrade] Nâng cấp passive: {passive.upgradeName} (Lv.{passiveLevels[passive.upgradeName]})");
        }

        ApplyEffect(passive);
    }

    private void ApplyEffect(UpgradeData upgrade)
    {
        if (upgrade.category != UpgradeCategory.Player_Upgrade)
            return;

        var stats = PlayerStats.Instance;

        switch (upgrade.upgradeType)
        {
            // --- Nhóm scaling % ---
            case UpgradeType.MaxHealth:
                stats.MaxHealth *= (1 + upgrade.value);
                stats.CurrentHealth = stats.MaxHealth; // hồi đầy khi tăng máu
                Debug.Log($"[Effect] +{upgrade.value * 100}% MaxHealth, hiện tại = {stats.MaxHealth}");
                break;

            case UpgradeType.MoveSpeed:
                stats.MoveSpeed *= (1 + upgrade.value);
                Debug.Log($"[Effect] +{upgrade.value * 100}% MoveSpeed, hiện tại = {stats.MoveSpeed}");
                break;

            case UpgradeType.PickupRange:
                stats.PickupRange *= (1 + upgrade.value);
                Debug.Log($"[Effect] +{upgrade.value * 100}% PickupRange, hiện tại = {stats.PickupRange}");
                break;

            case UpgradeType.CritDamage:
                stats.CritDamageMultiplier *= (1 + upgrade.value);
                Debug.Log($"[Effect] +{upgrade.value * 100}% CritDamage Multiplier, hiện tại = {stats.CritDamageMultiplier}");
                break;

            case UpgradeType.ExpGain:
                stats.ExpMultiplier *= (1 + upgrade.value);
                Debug.Log($"[Effect] +{upgrade.value * 100}% ExpGain, hiện tại = {stats.ExpMultiplier}");
                break;

            // --- Nhóm cộng thẳng ---
            case UpgradeType.Armor:
                stats.Armor += Mathf.RoundToInt(upgrade.value);
                Debug.Log($"[Effect] +{upgrade.value} Armor, hiện tại = {stats.Armor}");
                break;

            case UpgradeType.Regen:
                stats.RegenPerSecond += upgrade.value;
                Debug.Log($"[Effect] +{upgrade.value} Regen/sec, hiện tại = {stats.RegenPerSecond}");
                break;

            case UpgradeType.CritChance:
                stats.CritChance += upgrade.value;
                Debug.Log($"[Effect] +{upgrade.value}% CritChance, hiện tại = {stats.CritChance}");
                break;
        }
    }

}
