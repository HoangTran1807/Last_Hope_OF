using System;
using UnityEngine;

public class GameController : BaseManager<GameController>
{
    private float deltaTime = 0.0f;
    public Boolean isPlay = false;


    private void Start()
    {
        // Fade-out nhạc menu và phát nhạc in-game
        AudioManager.Instance.PlayRandomBGM(isPlay);
        Time.timeScale = 0f;
    }

    void Update()
    {
        // deltaTime trung bình để giảm giật (FPS counter)
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPlay)
            {
                // Nếu đang chơi, pause và mở menu/setting
                PauseGame();
            }
        }
    }


    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100); // vị trí hiển thị
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50; // cỡ chữ
        style.normal.textColor = Color.white;

        float msec = deltaTime * 1000.0f;   // thời gian 1 frame (ms)
        float fps = 1.0f / deltaTime;       // số khung hình/giây
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }

    public void PlayGame()
    {
        // ẩn main menu ui
        // hiện menu chọn vũ khí    
        // chơi nhạc nền in game
        
        Debug.Log("Play Game được ấn!");
        UIManager.Instance.MenuUI.gameObject.SetActive(false);
        UIManager.Instance.SelectWeaponPanel.gameObject.SetActive(true);
        GameSetup.Instance.ShowWeaponSelection();
        AudioManager.Instance.PlayRandomBGM(isPlay);

    }

    public void StartGame()
    {
        UIManager.Instance.ShowGameUI();
        Time.timeScale = 1.0f;
        isPlay = true;
    }

    public void PauseGame()
    {
        UIManager.Instance.SetPauseUI(true);
        Time.timeScale = 0f;
        isPlay= false;
    }

    public void ResumeGame()
    {
        UIManager.Instance.SetPauseUI(false);
        Time.timeScale = 1f;
        isPlay = true;
    }


    public void ShowUpgrade()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance.UpgradePanel.gameObject.SetActive(true);
            UpgradePanelController.Instance.ShowUpgradePanel();
        }
    }


    public void OpenSetting()
    {
        Debug.Log("open setting");
        UIManager.Instance.SettingPanel.gameObject.SetActive(true);
        
    }

    // khi thoát ra main menu 
    public void ExitToMenu()
    {
       
        Debug.Log("back to menu");

    }
}
