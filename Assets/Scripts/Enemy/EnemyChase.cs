using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChase : MonoBehaviour
{
    [SerializeField] float speed = 2.5f;

    Rigidbody2D rb;
    Transform target;
    PlayerHealth targetHealth;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        target = player.transform;
        targetHealth = player.GetComponent<PlayerHealth>();
    }

    void FixedUpdate()
    {
        if (target == null || (targetHealth != null && targetHealth.IsDead))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        rb.linearVelocity = dir * speed;
    }
}