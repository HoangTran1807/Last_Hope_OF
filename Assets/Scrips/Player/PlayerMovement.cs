using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerStats stats;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // Lấy input từ bàn phím
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.sqrMagnitude > 1)
            moveInput.Normalize();

        // ✅ Xử lý flip theo hướng ngang
        if (moveInput.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);  // quay phải
        else if (moveInput.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1); // quay trái
    }

    void FixedUpdate()
    {
        float moveSpeed = stats.MoveSpeed * stats.SpeedMultiplier;
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
