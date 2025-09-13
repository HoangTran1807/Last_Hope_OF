using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI result;
    [SerializeField] private TextMeshProUGUI liveTime;
    [SerializeField] private TextMeshProUGUI killCount;
    [SerializeField] private Button quitbtn;

    private GameController gameController;

    private void Awake()
    {
        gameController = GameController.Instance;
    }

    public void ShowResult(bool isWin)
    {
        result.text = isWin ? "YOU WIN!" : "YOU LOSE!";
        liveTime.text = $"Time: {FormatTime(gameController.timer)}";
        killCount.text = $"Killed: {gameController.killedEnemy}";
        gameObject.SetActive(true);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
    }

    public void OnCloseButtonClick()
    {
        AudioManager.Instance.PlayClickEffect();
        GameController.Instance.ExitToMenu();
    }
}
