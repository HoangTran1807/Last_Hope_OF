using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : BaseManager<UIManager>
{
    public GameUI GameUI;
    public SettingPanel SettingPanel;
    public WeaponSelectUI SelectWeaponPanel;
    public UpgradePanelUI UpgradePanel;
    public MenuUI MenuUI;
    public PauseUI PauseUI;
    
    

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        MenuUI.gameObject.SetActive(true);
        GameUI.gameObject.SetActive(false);
        UpgradePanel.gameObject.SetActive(false);
        SelectWeaponPanel.gameObject.SetActive(false);
        SettingPanel.gameObject.SetActive(false);
        PauseUI.gameObject.SetActive(false);
    }

    public void ShowWeaponSelect()
    {
        MenuUI.gameObject.SetActive(false);
        GameUI.gameObject.SetActive(false);
        UpgradePanel.gameObject.SetActive(false);
        SelectWeaponPanel.gameObject.SetActive(true);
        SettingPanel.gameObject.SetActive(false);
    }

    public void ShowGameUI()
    {
        MenuUI.gameObject.SetActive(false);
        GameUI.gameObject.SetActive(true);
        UpgradePanel.gameObject.SetActive(false);
        SelectWeaponPanel.gameObject.SetActive(false);
        SettingPanel.gameObject.SetActive(false);
    }

    

}
