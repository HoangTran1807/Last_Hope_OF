using UnityEngine;

public class simplePlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // tốc độ di chuyển

    private Vector2 movement;

    void Update()
    {
        // Lấy input từ bàn phím (WASD hoặc phím mũi tên)
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D hoặc ←/→
        movement.y = Input.GetAxisRaw("Vertical");   // W/S hoặc ↑/↓

        // Chuẩn hóa để đi chéo không nhanh hơn
        movement = movement.normalized;

        // Di chuyển bằng transform
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
