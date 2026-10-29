using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBar;

    int health;

    public bool IsDead => health <= 0;

    void Start()
    {
        health = maxHealth;
        if (healthBar != null) healthBar.maxValue = maxHealth;
        UpdateBar();
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        SetHealth(health - amount);
        if (IsDead) Debug.Log("Player died");
    }

    public void Heal(int amount)
    {
        // no revival from dead
        if (IsDead) return;

        SetHealth(health + amount);
    }

    void SetHealth(int value)
    {
        health = Mathf.Clamp(value, 0, maxHealth);
        UpdateBar();
    }

    void UpdateBar()
    {
        if (healthBar != null) healthBar.value = health;
    }

    [ContextMenu("Take 10 damage")]
    void TestDamage() => TakeDamage(10);
}