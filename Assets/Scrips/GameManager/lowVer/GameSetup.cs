using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [Header("Starting Upgrades")]
    [SerializeField] 
    private List<UpgradeData> startingUnlocks;
    [SerializeField] 
    private GameObject PickerUIPrefabs;
     private List<GameObject> startingUpgradesUI;

    [SerializeField] private GameObject panelRoot;

    public static GameSetup instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        startingUpgradesUI = new List<GameObject>();
    }

    private void Start()
    {
        ShowAllWeapons();
    }

    public void ShowAllWeapons()
    {
        if (startingUnlocks == null || startingUnlocks.Count == 0)
        {
            Debug.Log("Không có vũ khí nào để lựa chọn");
            return;
        }

        ClearPickers();
        panelRoot.SetActive(true);

        foreach (UpgradeData upgrade in startingUnlocks)
        {
            GameObject picker = Instantiate(PickerUIPrefabs, panelRoot.transform);
            Debug.Log("Tạo picker UI");

            SelectOptionUI selectOptionUI = picker.GetComponent<SelectOptionUI>();
            if (selectOptionUI != null)
            {
                selectOptionUI.SetData(upgrade);
            }

            startingUpgradesUI.Add(picker);
        }

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        Debug.Log("Đã ẩn giao diện chọn vũ khí");
        panelRoot.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ClearPickers()
    {
        foreach (GameObject go in startingUpgradesUI)
        {
            if (go != null) Destroy(go);
        }
        startingUpgradesUI.Clear();
    }
}
