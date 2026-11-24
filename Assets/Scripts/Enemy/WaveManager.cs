using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [SerializeField] Health enemyPrefab;
    [SerializeField] Transform player;
    [SerializeField] float waveDuration = 30f;
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] Vector2 arenaSize = new Vector2(16f, 9f);
    [SerializeField] float minPlayerDistance = 4f;

    readonly List<Health> alive = new();
    PlayerHealth playerHealth;
    float nextSpawn;

    public int Wave { get; private set; }
    public float TimeLeft { get; private set; }
    public bool Running { get; private set; }

    public event Action<int> WaveStarted;
    public event Action<int> WaveEnded;

    void Start()
    {
        if (player != null) playerHealth = player.GetComponent<PlayerHealth>();
        StartNextWave();
    }

    [ContextMenu("Start next wave")]
    public void StartNextWave()
    {
        if (Running) return;

        Wave++;
        TimeLeft = waveDuration;
        nextSpawn = 0f;
        Running = true;
        WaveStarted?.Invoke(Wave);
    }

    void Update()
    {
        if (!Running) return;

        if (playerHealth != null && playerHealth.IsDead)
        {
            Running = false;
            return;
        }

        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0f)
        {
            EndWave();
            return;
        }

        nextSpawn -= Time.deltaTime;
        if (nextSpawn > 0f) return;

        Spawn();
        nextSpawn = spawnInterval;
    }

    void Spawn()
    {
        if (enemyPrefab == null) return;

        var enemy = Instantiate(enemyPrefab, RandomPoint(), Quaternion.identity);
        enemy.Died += OnEnemyDied;
        alive.Add(enemy);
    }

    
    Vector2 RandomPoint()
    {
        Vector2 center = transform.position;
        Vector2 half = arenaSize / 2f;
        Vector2 point = center;

        for (int i = 0; i < 10; i++)
        {
            point = center + new Vector2(Random.Range(-half.x, half.x), Random.Range(-half.y, half.y));
            if (player == null || Vector2.Distance(point, player.position) >= minPlayerDistance) break;
        }
        return point;
    }

    void OnEnemyDied(Health enemy) => alive.Remove(enemy);

    void EndWave()
    {
        Running = false;
        TimeLeft = 0f;

        foreach (var enemy in alive)
            if (enemy != null) Destroy(enemy.gameObject);
        alive.Clear();

        Debug.Log($"Wave {Wave} ended");
        WaveEnded?.Invoke(Wave);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, arenaSize);
    }
}