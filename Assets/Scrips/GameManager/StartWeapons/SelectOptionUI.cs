using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SelectOptionUI : MonoBehaviour
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponName;

    private UpgradeData gunData;

    // Sự kiện bắn ra khi người chơi click
    public UnityEvent<UpgradeData> OnSelected = new UnityEvent<UpgradeData>();

    public void SetData(UpgradeData data)
    {
        gunData = data;

        if (gunData == null)
        {
            Debug.LogWarning("Không tồn tại vũ khí này");
            return;
        }

        if (weaponIcon != null)
            weaponIcon.sprite = gunData.icon;

        if (weaponName != null)
            weaponName.text = gunData.upgradeName;
    }

    public void OnClick()
    {
        if (gunData == null) return;
        OnSelected.Invoke(gunData);
    }
}
