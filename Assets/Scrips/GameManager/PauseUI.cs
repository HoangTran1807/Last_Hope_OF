using UnityEngine;

public class PauseUI : MonoBehaviour
{
    // Button "Resume"
    public void OnResumeButton()
    {
        GameController.Instance.ResumeGame();
    }

    // Button "Setting"
    public void OnSettingButton() => GameController.Instance.OpenSetting();

    // Button "Exit"
    public void OnExitButton() => GameController.Instance.ExitToMenu();

}
