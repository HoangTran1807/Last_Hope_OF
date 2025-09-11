using System.Collections.Generic;
using UnityEngine;

public class UpgradePanelUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    public UpgradeOptionUI[] optionPanels; // mỗi ô panel gắn script UpgradeOptionUI

    /// <summary>
    /// Hiển thị panel với danh sách upgrade
    /// </summary>
    public void Show(List<UpgradeData> upgrades)
    {
        panelRoot.SetActive(true);

        for (int i = 0; i < optionPanels.Length; i++)
        {
            if (i < upgrades.Count)
            {
                optionPanels[i].gameObject.SetActive(true);
                optionPanels[i].SetData(upgrades[i]);
            }
            else
            {
                optionPanels[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Ẩn panel
    /// </summary>
    public void Hide()
    {
        panelRoot.SetActive(false);
    }
}
