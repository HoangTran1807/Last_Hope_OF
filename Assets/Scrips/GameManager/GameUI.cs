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

    private float timer = 0;
    private int killedEnemy = 0;

    public static GameUI Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // tăng timer
        timer += Time.deltaTime;

        // cập nhật HP bar
        HPBar.value = PlayerStats.Instance.CurrentHealth / PlayerStats.Instance.MaxHealth;

        //// cập nhật exp bar (giả sử PlayerStats có exp hiện tại và exp cần để lên level)
        //expBar.value = PlayerStats.Instance.CurrentExp / PlayerStats.Instance.RequiredExp;

        // hiển thị thời gian dạng mm:ss
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerUI.text = "Time Survival: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        // số quái đã giết hiển thị liên tục
        killedEnemyUI.text = "Enemy Killled: " + killedEnemy.ToString();
    }

    public void AddKilledEnemy()
    {
        killedEnemy++;
    }
}
