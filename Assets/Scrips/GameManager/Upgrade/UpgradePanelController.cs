using System.Collections.Generic;
using UnityEngine;

public class UpgradePanelController : BaseManager<UpgradePanelController>
{
    [SerializeField] private UpgradePanelUI panelUI;


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
    }

    private void OnUpgradeSelected(UpgradeData data)
    {
        UpgradeManager.Instance.ApplyUpgrade(data);
        panelUI.Hide();
        GameController.Instance.ResumeGame();
    }
}
