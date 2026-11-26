using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] WaveManager waves;
    [SerializeField] PlayerMaterials materials;
    [SerializeField] PlayerHealth health;
    [SerializeField] PlayerController controller;
    [SerializeField] GameObject panel;
    [SerializeField] Button healButton;
    [SerializeField] Button maxHealthButton;
    [SerializeField] Button speedButton;
    [SerializeField] Button nextWaveButton;
    [SerializeField] int healCost = 3;
    [SerializeField] int healAmount = 30;
    [SerializeField] int maxHealthCost = 5;
    [SerializeField] int maxHealthAmount = 20;
    [SerializeField] int speedCost = 5;
    [SerializeField, Range(0f, 1f)] float speedBonus = 0.15f;

    readonly List<(Button button, int cost)> items = new();

    void Awake()
    {
        if (health != null)
        {
            Add(healButton, $"Heal +{healAmount}", healCost, () => health.Heal(healAmount));
            Add(maxHealthButton, $"+{maxHealthAmount} Max HP", maxHealthCost, () => health.AddMaxHealth(maxHealthAmount));
        }

        if (controller != null)
            Add(speedButton, $"+{Mathf.RoundToInt(speedBonus * 100f)}% Speed", speedCost, () => controller.AddSpeed(speedBonus));

        if (nextWaveButton != null)
        {
            SetLabel(nextWaveButton, "Next Wave");
            nextWaveButton.onClick.AddListener(NextWave);
        }

        SetOpen(false);
    }

    void OnEnable()
    {
        if (waves != null)
        {
            waves.WaveStarted += OnWaveStarted;
            waves.WaveEnded += OnWaveEnded;
        }
        if (materials != null) materials.Changed += Refresh;
    }

    void OnDisable()
    {
        if (waves != null)
        {
            waves.WaveStarted -= OnWaveStarted;
            waves.WaveEnded -= OnWaveEnded;
        }
        if (materials != null) materials.Changed -= Refresh;
    }

    void Add(Button button, string label, int cost, Action apply)
    {
        if (button == null) return;

        SetLabel(button, $"{label}  ({cost})");
        button.onClick.AddListener(() => Buy(cost, apply));
        items.Add((button, cost));
    }

    static void SetLabel(Button button, string label)
    {
        var text = button.GetComponentInChildren<TMP_Text>();
        if (text != null) text.text = label;
    }

    void OnWaveStarted(int _) => SetOpen(false);
    void OnWaveEnded(int _) => SetOpen(true);

    void SetOpen(bool open)
    {
        if (panel != null) panel.SetActive(open);
        if (open) Refresh();
    }

    bool CanBuy(int cost) =>
        materials != null && materials.Materials >= cost && (health == null || !health.IsDead);

    void Buy(int cost, Action apply)
    {
        if (CanBuy(cost) && materials.Spend(cost)) apply();
    }

    void Refresh()
    {
        foreach (var (button, cost) in items)
            button.interactable = CanBuy(cost);
    }

    void NextWave()
    {
        if (waves != null) waves.StartNextWave();
    }

    [ContextMenu("Give 10 materials")]
    void GiveMaterials()
    {
        if (materials != null) materials.Add(10, 0);
    }
}
