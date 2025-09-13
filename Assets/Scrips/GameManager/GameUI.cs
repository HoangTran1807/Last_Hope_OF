using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider expBar;
    [SerializeField] private Slider HPBar;
    [SerializeField] private TextMeshProUGUI timerUI;
    [SerializeField] private TextMeshProUGUI killedEnemyUI;

    

    public static GameUI Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // cập nhật exp bar
        if (PlayerLevelSystem.Instance != null)
        {
            // currentExp / expToNextLevel sẽ cho ra tỉ lệ (0 -> 1)
            expBar.value = (float)PlayerLevelSystem.Instance.currentExp
                           / PlayerLevelSystem.Instance.expToNextLevel;
        }



        // cập nhật HP bar
        HPBar.value = PlayerStats.Instance.CurrentHealth / PlayerStats.Instance.MaxHealth;

        //// cập nhật exp bar (giả sử PlayerStats có exp hiện tại và exp cần để lên level)
        //expBar.value = PlayerStats.Instance.CurrentExp / PlayerStats.Instance.RequiredExp;

        // hiển thị thời gian dạng mm:ss
        int minutes = Mathf.FloorToInt(GameController.Instance.timer / 60f);
        int seconds = Mathf.FloorToInt(GameController.Instance.timer % 60f);
        timerUI.text = "Time Survival: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        // số quái đã giết hiển thị liên tục
        killedEnemyUI.text = "Enemy Killled: " + GameController.Instance.killedEnemy.ToString();
    }


}
