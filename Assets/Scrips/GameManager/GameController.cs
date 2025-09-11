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
                UIManager.Instance.PauseUI.gameObject.SetActive(true);
            }
            else
            {
                // Nếu đang pause, resume game
                ResumeGame();
                UIManager.Instance.PauseUI.gameObject.SetActive(false);
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

    public void StartGame()
    {
        
        Debug.Log("Play Game được ấn!");
        UIManager.Instance.MenuUI.gameObject.SetActive(false);
        UIManager.Instance.SelectWeaponPanel.gameObject.SetActive(true);
        GameSetup.Instance.ShowWeaponSelection();
        AudioManager.Instance.PlayRandomBGM(isPlay);

        ResumeGame();
        isPlay = true;

    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
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
        PauseGame();
        UIManager.Instance.SettingPanel.gameObject.SetActive(true);
        
    }

    // khi thoát ra main menu 
    public void ExitToMenu()
    {
       
        Debug.Log("back to menu");
    }
}
