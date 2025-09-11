using System.Collections.Generic;
using UnityEngine;

public class UpgradePanelController : MonoBehaviour
{
    [SerializeField] private UpgradePanelUI panelUI;

    public static UpgradePanelController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Hiển thị panel upgrade và pause game
    /// </summary>
    public void ShowUpgradePanel()
    {
        var upgrades = UpgradeManager.Instance.GetUpgradeChoices();
        foreach (var upgrade in upgrades)
        {
            Debug.Log(upgrade.name);
        }
        panelUI.Show(upgrades);

        // Gắn listener cho từng option
        foreach (var option in panelUI.optionPanels)
        {
            option.OnSelected.RemoveAllListeners();
            option.OnSelected.AddListener(OnUpgradeSelected);
        }

        GameController.Instance.PauseGame();
    }

    private void OnUpgradeSelected(UpgradeData data)
    {
        UpgradeManager.Instance.ApplyUpgrade(data);
        panelUI.Hide();
        GameController.Instance.ResumeGame();
    }
}
