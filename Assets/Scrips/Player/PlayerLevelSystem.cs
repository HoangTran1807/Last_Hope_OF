using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    public void AddExp(int amount)
    {
        currentExp += amount;
        Debug.Log("EXP + " + amount + " | Total: " + currentExp);

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }


    // kinh nghi?m c?n ?? lên c?p ti?p theo s? b?ng 120% c?p hi?n t?i 
    void LevelUp()
    {
        level++;
        currentExp -= expToNextLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);
        Debug.Log("Level Up! Current Level: " + level);

        if (GameController.Instance != null)
        {
            GameController.Instance.ShowUpgrade();
        }

    }
}
