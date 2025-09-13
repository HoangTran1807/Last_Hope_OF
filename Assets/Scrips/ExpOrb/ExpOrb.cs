using UnityEngine;

public class EXPOrb : MonoBehaviour
{
    public int expAmount = 10;
    public float moveSpeed = 7f;

    [HideInInspector] public Transform targetPlayer;
    [HideInInspector] public bool isMoving = false;

    private Vector3 baseScale = Vector3.one;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        baseScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning("EXPOrb không có SpriteRenderer, vui lòng gắn thêm!");
    }

    void OnEnable()
    {
        OrbManager.Instance.RegisterOrb(this);
        UpdateVisual();
    }

    void OnDisable()
    {
        if (OrbManager.HasInstance)
            OrbManager.Instance.UnregisterOrb(this);
    }

    public void StartMoveToPlayer(Transform player)
    {
        targetPlayer = player;
        isMoving = true;
    }

    public void UpdateVisual()
    {
        // Scale theo exp
        float scaleFactor = 1f + (expAmount / 50f);
        scaleFactor = Mathf.Clamp(scaleFactor, 1f, 4f);
        transform.localScale = baseScale * scaleFactor;

        // Màu sắc theo exp
        if (spriteRenderer != null)
        {
            if (expAmount < 20) spriteRenderer.color = Color.green;
            else if (expAmount < 50) spriteRenderer.color = Color.yellow;
            else if (expAmount < 150) spriteRenderer.color = new Color(1f, 0.5f, 0f); // cam
            else spriteRenderer.color = Color.magenta;
        }
    }

    public void MergeWith(EXPOrb otherOrb)
    {
        if (otherOrb == null || otherOrb == this) return;

        expAmount += otherOrb.expAmount;
        OrbPool.Instance.ReturnOrb(otherOrb.gameObject);
        UpdateVisual();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerLevelSystem>().AddExp(expAmount);
            OrbPool.Instance.ReturnOrb(gameObject);
            AudioManager.Instance.PlaySE("pickOrb");
        }
    }

    public float GetMergeRadius()
    {
        // Merge radius dựa vào scale (orb càng to thì càng dễ hút nhau)
        return transform.localScale.x * 0.5f; // ví dụ lấy nửa kích thước orb làm bán kính
    }

}
