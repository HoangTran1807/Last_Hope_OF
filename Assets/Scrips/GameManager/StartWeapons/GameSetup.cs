using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [Header("Starting Upgrades")]
    [SerializeField] private List<UpgradeData> startingUnlocks;

    [SerializeField] private WeaponSelectUI weaponSelectUI;

    public static GameSetup Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    public void ShowWeaponSelection()
    {
        if (startingUnlocks == null || startingUnlocks.Count == 0)
        {
            Debug.Log("Không có vũ khí nào để lựa chọn");
            return;
        }

        UIManager.Instance.ShowWeaponSelect();
        weaponSelectUI.BuildOptions(startingUnlocks, OnWeaponSelected);
        Time.timeScale = 0f;
    }

    private void OnWeaponSelected(UpgradeData data)
    {
        UpgradeManager.Instance.ApplyUpgrade(data);
        Debug.Log($"Đã thêm vũ khí {data.upgradeName} cho người chơi");

        UIManager.Instance.ShowGameUI();
        Time.timeScale = 1f;
    }
}
