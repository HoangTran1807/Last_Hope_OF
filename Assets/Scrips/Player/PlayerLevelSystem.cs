using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    public static PlayerLevelSystem Instance;

    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    public delegate void LevelUpHandler(int newLevel);
    public event LevelUpHandler OnLevelUp;

    private void Awake()
    {
        Instance = this;
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        currentExp -= expToNextLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);

        OnLevelUp?.Invoke(level);
    }
}
