using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{


    // === Xử lý nút Play Game ===
    public void OnPlayButtonClick()
    {
       GameController.Instance.StartGame();
    }

    // === Xử lý nút Sound Setting ===
    public void OnSoundSettingClick()
    {
        Debug.Log("Mở Setting Panel!");
        UIManager.Instance.SettingPanel.gameObject.SetActive(true);
    }

    // === Xử lý nút Quit Game ===
    public void OnQuitButtonClick()
    {
        Debug.Log("Thoát Game!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // dừng Play Mode trong Editor
#else
        Application.Quit(); // thoát hẳn khi build
#endif
    }
}
