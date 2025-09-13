using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Menu,
    WeaponSelect,
    Playing,
    Paused,
    Upgrade,
    Setting,
    GameOver
}

public class GameController : BaseManager<GameController>
{
    private float deltaTime = 0.0f;
    public float timer = 0;
    public int killedEnemy = 0;

    [SerializeField]
    private bool isWin = false;

    private const float maxGameTime = 600f; // 10 phút
    public GameState CurrentState { get; private set; } = GameState.Menu;

    public void AddKilledEnemy()
    {
        killedEnemy++;
 
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        // Bắt đầu ở menu
        Time.timeScale = 0f;
        UIManager.Instance.ShowUI(GameState.Menu);

        // Phát nhạc menu
        AudioManager.Instance.PlayRandomBGM(false);

        PlayerLevelSystem.Instance.OnLevelUp += HandleLevelUp;
    }

    private void Update()
    {
        // FPS counter
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;


        // tăng thời gian nếu người chơi đang chơi 
        if (CurrentState == GameState.Playing)
        {
            timer += Time.deltaTime;
            if (!isWin && timer >= maxGameTime)
            {
                isWin = true;
            }
        }
           

        // Pause bằng phím ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing)
            {
                PauseGame();
            }
            else if (CurrentState == GameState.Paused)
            {
                ResumeGame();
            }
        }
    }

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = Color.white;

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }


    private void HandleLevelUp(int newLevel)
    {
        List<UpgradeData> upgrades = UpgradeManager.Instance.GetUpgradeChoices();
        ShowUpgrade(upgrades);
    }

    // ================== GAME FLOW ==================

    public void PlayGame()
    {
        Debug.Log("Play Game được ấn!");
        ChangeState(GameState.WeaponSelect);
        isWin = false;

        GameSetup.Instance.ShowWeaponSelection();
        AudioManager.Instance.PlayRandomBGM(true);
    }

    public void StartGame()
    {
        Debug.Log("Bắt đầu gameplay!");
        ChangeState(GameState.Playing);
    }

    public void PauseGame()
    {
        Debug.Log("Game paused!");
        ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        Debug.Log("Resume game!");
        ChangeState(GameState.Playing);
    }

    public void ShowUpgrade(List<UpgradeData> upgrades)
    {
        if (upgrades.Count == 0 || upgrades == null)
            return;
        Debug.Log("Show upgrade panel!");
        ChangeState(GameState.Upgrade);
        UpgradePanelController.Instance.ShowUpgradePanel(upgrades);
    }


    public void OpenSetting()
    {
        Debug.Log("Open setting!");
        ChangeState(GameState.Setting);
    }

    public void ExitToMenu()
    {
        Debug.Log("Quay về menu!");
        AudioManager.Instance.PlayRandomBGM(false);
        ChangeState(GameState.Menu);
        RestartGame();  
    }

    public void RestartGame()
    {
        // lấy scene hiện tại và load lại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        killedEnemy = 0;
        timer = 0;
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);

        // Hiển thị UI game over
        GameOverUI gameOverUI = UIManager.Instance.GameOverUI;
        if (gameOverUI != null)
        {
            gameOverUI.ShowResult(isWin);
        }

        // Dừng tất cả gameplay
        Time.timeScale = 0f;
    }

    // ================== STATE HANDLER ==================
    private void ChangeState(GameState newState)
    {
        CurrentState = newState;
        UIManager.Instance.ShowUI(newState);

        switch (newState)
        {
            case GameState.Menu:
                Time.timeScale = 0f;
                break;

            case GameState.WeaponSelect:
                Time.timeScale = 0f;
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;

            case GameState.Upgrade:
                Time.timeScale = 0f;
                break;

            case GameState.Setting:
                Time.timeScale = 0f;
                break;
        }
    }



}
