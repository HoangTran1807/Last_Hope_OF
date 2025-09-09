using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectOptionUI : MonoBehaviour
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponName;

    private UpgradeData gunData;

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

        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.ApplyUpgrade(gunData);
            Debug.Log($"Đã thêm vũ khí {gunData.upgradeName} cho người chơi");
        }
        else
        {
            Debug.LogError("UpgradeManager chưa tồn tại trong scene!");
        }

        if (GameSetup.instance != null)
        {
            GameSetup.instance.Hide();
        }
    }
}
