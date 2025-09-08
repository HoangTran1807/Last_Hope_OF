using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerMagnet : MonoBehaviour
{
    public float magnetRadius = 3f;
    [SerializeField]
    private CircleCollider2D magnetCollider;

    void Awake()
    {
        magnetCollider = GetComponent<CircleCollider2D>();
        magnetCollider.isTrigger = true;
        magnetCollider.radius = magnetRadius;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EXPOrb orb = other.GetComponent<EXPOrb>();
        if (orb != null)
        {
            orb.StartMoveToPlayer(transform);
        }
    }
}
