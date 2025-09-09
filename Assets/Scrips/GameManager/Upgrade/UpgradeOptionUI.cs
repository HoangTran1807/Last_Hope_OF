using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOptionUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private int updateIndex;

    private UpgradeData upgradeData;

    public void SetData(UpgradeData data)
    {
        upgradeData = data;
        if (data.icon != null) iconImage.sprite = data.icon;
        nameText.text = data.upgradeName;
        descText.text = data.description;
    }

    // Hàm này s? ???c g?i khi click vào panel
    public void OnClick()
    {
        if (upgradeData == null) return;

        UpgradeManager.Instance.ApplyUpgrade(upgradeData);

        // ?n panel ch?n
        Object.FindFirstObjectByType<UpgradePanel>().HidePanel();

        Time.timeScale = 1f;
    }
}
