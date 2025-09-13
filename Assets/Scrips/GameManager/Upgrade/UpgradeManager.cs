using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Starting Upgrades")]
    [SerializeField] private List<UpgradeData> startingUnlocks;

    [Header("All Available Upgrades")]
    [SerializeField] private List<UpgradeData> allUpgrades;

    [Header("Upgrade Settings")]
    [SerializeField] private int choiceCount = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeUpgrades();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeUpgrades()
    {
        allUpgrades = new List<UpgradeData>();

        // Load các nâng cấp chỉ số người chơi từ Resources
        List<UpgradeData> playerUpgrades = Resources.LoadAll<UpgradeData>("Upgrades/Player").ToList();

        allUpgrades.AddRange(startingUnlocks); // vũ khí khởi tạo
        allUpgrades.AddRange(playerUpgrades);  // nâng cấp nội tại
    }

    /// <summary>
    /// Sinh ra danh sách các nâng cấp khả dụng
    /// </summary>
    public List<UpgradeData> GetUpgradeChoices()
    {
        List<UpgradeData> validUpgrades = new List<UpgradeData>();

        foreach (var upgrade in allUpgrades)
        {
            switch (upgrade.category)
            {
                case UpgradeCategory.Weapon_Upgrade:
                    if (PlayerWeaponManager.Instance.HasWeapon(upgrade.upgradeName))
                    {
                        // Vũ khí đã có -> chỉ lấy upgrade nếu chưa max level
                        if (PlayerWeaponManager.Instance.CanUpgrade(upgrade.upgradeName))
                            validUpgrades.Add(upgrade);
                    }
                    else
                    {
                        // Vũ khí chưa có -> chỉ hợp lệ nếu chưa đầy slot
                        if (!PlayerWeaponManager.Instance.IsWeaponFull())
                            validUpgrades.Add(upgrade);
                    }
                    break;

                case UpgradeCategory.Player_Upgrade:
                    if (PlayerUpgradeManager.Instance.CanUpgrade(upgrade))
                        validUpgrades.Add(upgrade);
                    break;
            }
        }

        // Shuffle
        Shuffle(validUpgrades);

        // Chọn ra số lượng giới hạn
        return validUpgrades.Take(choiceCount).ToList();
    }

    /// <summary>
    /// Áp dụng upgrade được chọn
    /// </summary>
    public void ApplyUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null) return;

        switch (upgrade.category)
        {
            case UpgradeCategory.Weapon_Upgrade:
                bool isNewWeapon = !PlayerWeaponManager.Instance.HasWeapon(upgrade.upgradeName);

                PlayerWeaponManager.Instance.ApplyUpgrade(upgrade);

                if (isNewWeapon)
                {
                    var newGunPrefab = Resources.Load<BaseWeapon>($"Weapons/{upgrade.upgradeName}");
                    if (newGunPrefab != null)
                    {
                        // Xóa unlock ban đầu
                        allUpgrades.Remove(upgrade);

                        // Thêm các upgrade tiếp theo của vũ khí này
                        allUpgrades.AddRange(newGunPrefab.upgradeable);
                    }
                }
                break;

            case UpgradeCategory.Player_Upgrade:
                // Apply upgrade vào player stats
                PlayerUpgradeManager.Instance.ApplyUpgrade(upgrade);
                break;
        }
    }

    // Fisher-Yates shuffle
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
