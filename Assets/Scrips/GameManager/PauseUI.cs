using UnityEngine;

public class PauseUI : MonoBehaviour
{
    // Button "Resume"
    public void OnResumeButton() {
        AudioManager.Instance.PlayClickEffect();
        GameController.Instance.ResumeGame();
    }

    // Button "Setting"
    public void OnSettingButton()
    {
        AudioManager.Instance.PlayClickEffect();
        GameController.Instance.OpenSetting();
    }

    // Button "Exit"
    public void OnExitButton()
    {
        AudioManager.Instance.PlayClickEffect();
        GameController.Instance.ExitToMenu();
    }

}
