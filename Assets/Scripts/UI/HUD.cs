using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] WaveManager waves;
    [SerializeField] PlayerMaterials materials;
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text materialText;

    int shownSeconds = -1;

    void OnEnable()
    {
        if (waves != null) waves.WaveStarted += OnWaveStarted;
        if (materials != null) materials.Changed += RefreshMaterials;
    }

    void OnDisable()
    {
        if (waves != null) waves.WaveStarted -= OnWaveStarted;
        if (materials != null) materials.Changed -= RefreshMaterials;
    }

    void Start()
    {
        RefreshWave();
        RefreshMaterials();
    }

    void Update() => RefreshTimer();

    void OnWaveStarted(int _)
    {
        RefreshWave();
        shownSeconds = -1;
    }

    void RefreshWave()
    {
        if (waves != null && waveText != null)
            waveText.text = $"Wave {waves.Wave}";
    }

    void RefreshTimer()
    {
        if (waves == null || timerText == null) return;

        int seconds = Mathf.CeilToInt(waves.TimeLeft);
        if (seconds == shownSeconds) return;

        shownSeconds = seconds;
        timerText.text = seconds.ToString();
    }

    void RefreshMaterials()
    {
        if (materials != null && materialText != null)
            materialText.text = $"Materials: {materials.Materials}";
    }
}
