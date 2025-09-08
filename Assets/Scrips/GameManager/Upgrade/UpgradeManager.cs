using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("All Available Upgrades")]
    [SerializeField] private List<UpgradeData> allUpgrades; // gán sẵn trong Inspector

    [Header("Upgrade Settings")]
    [SerializeField] private int choiceCount = 3;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Sinh ra danh sách các nâng cấp khả dụng theo logic:
    /// - Nếu player chưa đủ súng: có thể random vũ khí mới hoặc nâng cấp vũ khí đang có
    /// - Nếu đã full súng: chỉ random nâng cấp vũ khí đang có
    /// - Nếu vũ khí max level: bỏ qua
    /// - Nếu không còn upgrade nào hợp lệ => trả về rỗng
    /// </summary>
    public List<UpgradeData> GetUpgradeChoices()
    {
        List<UpgradeData> validUpgrades = new List<UpgradeData>();

        foreach (var upgrade in allUpgrades)
        {
            if (upgrade.isWeaponUpgrade)
            {
                // 1. Đã có súng này
                if (PlayerWeaponManager.Instance.HasWeapon(upgrade.upgradeName))
                {
                    if (PlayerWeaponManager.Instance.CanUpgrade(upgrade.upgradeName))
                        validUpgrades.Add(upgrade);
                }
                else
                {
                    // 2. Chưa có súng này
                    if (!PlayerWeaponManager.Instance.IsWeaponFull())
                        validUpgrades.Add(upgrade);
                }
            }
            else
            {
                // 3. Nâng cấp nội tại (player stats)
                validUpgrades.Add(upgrade);
            }
        }

        // Shuffle danh sách để random
        Shuffle(validUpgrades);

        // Lấy số lượng theo choiceCount
        List<UpgradeData> result = new List<UpgradeData>();
        for (int i = 0; i < Mathf.Min(choiceCount, validUpgrades.Count); i++)
        {
            result.Add(validUpgrades[i]);
        }

        return result;
    }

    /// <summary>
    /// Người chơi chọn upgrade trong danh sách
    /// </summary>
    public void ApplyUpgrade(UpgradeData upgrade)
    {
        Debug.Log("appline upgrade");
        if (upgrade == null) return;
        PlayerWeaponManager.Instance.ApplyUpgrade(upgrade);
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
