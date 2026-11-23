using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] float interval = 0.5f;

    float nextHit;

    void OnCollisionStay2D(Collision2D other)
    {
        if (Time.time < nextHit) return;
        if (!other.collider.TryGetComponent(out PlayerHealth player)) return;

        player.TakeDamage(damage);
        nextHit = Time.time + interval;
    }
}