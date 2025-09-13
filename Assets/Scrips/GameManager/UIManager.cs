using UnityEngine;

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
        ShowUI(GameState.Menu); // mặc định menu
    }

    public void ShowUI(GameState state)
    {
        // Ẩn hết trước
        MenuUI.gameObject.SetActive(false);
        GameUI.gameObject.SetActive(false);
        UpgradePanel.gameObject.SetActive(false);
        SelectWeaponPanel.gameObject.SetActive(false);
        SettingPanel.gameObject.SetActive(false);
        PauseUI.gameObject.SetActive(false);

        // Bật UI theo state
        switch (state)
        {
            case GameState.Menu:
                MenuUI.gameObject.SetActive(true);
                break;

            case GameState.WeaponSelect:
                SelectWeaponPanel.gameObject.SetActive(true);
                break;

            case GameState.Playing:
                GameUI.gameObject.SetActive(true);
                break;

            case GameState.Paused:
                PauseUI.gameObject.SetActive(true);
                break;

            case GameState.Upgrade:
                UpgradePanel.gameObject.SetActive(true);
                break;

            case GameState.Setting:
                SettingPanel.gameObject.SetActive(true);
                break;
        }
    }
}
