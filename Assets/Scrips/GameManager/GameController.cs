using UnityEngine;

public class GameController : MonoBehaviour
{
    private float deltaTime = 0.0f;

    void Update()
    {
        // Sử dụng deltaTime trung bình để giảm giật
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100); // vị trí hiển thị
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50; // cỡ chữ
        style.normal.textColor = Color.white;

        float msec = deltaTime * 1000.0f;   // thời gian 1 frame (ms)
        float fps = 1.0f / deltaTime;       // số khung hình/giây
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
