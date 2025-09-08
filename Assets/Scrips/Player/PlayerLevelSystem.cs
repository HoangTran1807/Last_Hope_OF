using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;
    [SerializeField]
    private UpgradePanel UpgradePanel;

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log("EXP + " + amount + " | Total: " + currentExp);

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        currentExp -= expToNextLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);
        Debug.Log("Level Up! Current Level: " + level);
        if (UpgradePanel != null)
        {
            UpgradePanel.ShowUpgradeChoices();
        }

    }
}
