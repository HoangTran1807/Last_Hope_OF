using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UpgradeOptionUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;

    private UpgradeData upgradeData;

    // Sự kiện bắn ra khi click
    public UnityEvent<UpgradeData> OnSelected = new UnityEvent<UpgradeData>();

    public void SetData(UpgradeData data)
    {
        upgradeData = data;
        if (data.icon != null) iconImage.sprite = data.icon;
        nameText.text = data.upgradeName;
        descText.text = data.description;
    }

    public void OnClick()
    {
        if (upgradeData == null) return;

        // Chỉ phát sự kiện nâng cấp vũ khí 
        OnSelected.Invoke(upgradeData);
    }
}
