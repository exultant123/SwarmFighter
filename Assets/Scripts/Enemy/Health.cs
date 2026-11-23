using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 30;

    int health;

    public event Action<Health> Died;
    public bool IsDead => health <= 0;

    void Awake() => health = maxHealth;

    public void TakeDamage(int amount)
    {
        if (IsDead || amount <= 0) return;

        health = Mathf.Max(health - amount, 0);
        if (!IsDead) return;

        Died?.Invoke(this);
        Destroy(gameObject);
    }

    [ContextMenu("Take 10 damage")]
    void TestDamage() => TakeDamage(10);
}