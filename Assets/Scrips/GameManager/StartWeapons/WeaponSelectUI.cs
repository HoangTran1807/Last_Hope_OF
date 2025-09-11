using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectUI : MonoBehaviour
{
    [Header("Prefabs & Container")]
    [SerializeField] private GameObject pickerUIPrefab; // prefab cho mỗi nút vũ khí
    [SerializeField] private Transform optionParent;     // parent chứa các option

    private List<GameObject> currentPickers = new List<GameObject>();

    /// <summary>
    /// Tạo danh sách các option vũ khí.
    /// </summary>
    /// <param name="upgrades">Danh sách vũ khí để hiển thị</param>
    /// <param name="onSelected">Callback khi người chơi chọn 1 option</param>
    public void BuildOptions(List<UpgradeData> upgrades, Action<UpgradeData> onSelected)
    {
        ClearPickers();
        Debug.Log("start bulid option");
        foreach (var upgrade in upgrades)
        {
            GameObject picker = Instantiate(pickerUIPrefab, optionParent);
            var optionUI = picker.GetComponent<SelectOptionUI>();

            optionUI.SetData(upgrade);
            optionUI.OnSelected.AddListener(data => onSelected?.Invoke(data));

            currentPickers.Add(picker);
        }
    }


    /// <summary>
    /// Xóa tất cả option hiện tại
    /// </summary>
    public void ClearPickers()
    {
        foreach (var go in currentPickers)
        {
            if (go != null) Destroy(go);
        }
        currentPickers.Clear();
    }
}
