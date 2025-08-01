using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Wave Settings")]
    [SerializeField] private float delayBetweenWaves = 5f;

    private int currentWave = 0;
    private int aliveEnemies = 0;
    private bool isPaused = false;
    private Coroutine nextWaveCoroutine = null;

    private void OnEnable()
    {
        Enemy.OnEnemyDied += HandleEnemyDied;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= HandleEnemyDied;
    }

    private void Start()
    {
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (isPaused) return;

        // Ensure no duplicate coroutines
        if (nextWaveCoroutine != null)
        {
            StopCoroutine(nextWaveCoroutine);
            nextWaveCoroutine = null;
        }

        currentWave++;
        int enemyCount = CalculateEnemiesForWave(currentWave);
        aliveEnemies = enemyCount;

        // Spawn enemies
        enemySpawner.SpawnEnemies(enemyCount);

        // Update UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateWave(currentWave);
            UIManager.Instance.UpdateEnemies(aliveEnemies);
        }

        Debug.Log($"Wave {currentWave} started with {enemyCount} enemies.");
    }

    private int CalculateEnemiesForWave(int waveNumber)
    {
        if (waveNumber == 1) return 30;
        if (waveNumber == 2) return 50;
        if (waveNumber == 3) return 70;

        return 70 + (waveNumber - 3) * 10;
    }

    private void HandleEnemyDied(Enemy enemy)
    {
        aliveEnemies--;

        // Update enemy count UI
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateEnemies(aliveEnemies);

        if (aliveEnemies <= 0 && !isPaused)
        {
            // Start countdown to next wave
            if (nextWaveCoroutine == null)
                nextWaveCoroutine = StartCoroutine(WaitAndSpawnNextWave());
        }
    }

    private IEnumerator WaitAndSpawnNextWave()
    {
        yield return new WaitForSeconds(delayBetweenWaves);
        nextWaveCoroutine = null;
        StartNextWave();
    }

    // UI Button Methods
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (!isPaused)
        {
            Debug.Log("Waves Resumed");
            // Resume next wave if previously cleared
            if (aliveEnemies <= 0 && nextWaveCoroutine == null)
                StartNextWave();
        }
        else
        {
            Debug.Log("Waves Paused");
        }
    }

    

    public void SkipWave()
    {
        Debug.Log("Skipping to next wave (adding enemies instead of killing).");

        // Increment to next wave
        currentWave++;
        int desiredCount = CalculateEnemiesForWave(currentWave);

        // Add enemies if alive count is less than required
        int toSpawn = desiredCount - aliveEnemies;
        if (toSpawn > 0)
        {
            enemySpawner.SpawnEnemies(toSpawn);
            aliveEnemies += toSpawn;
        }

        // Update UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateWave(currentWave);
            UIManager.Instance.UpdateEnemies(aliveEnemies);
        }

        // Stop any pending auto-next-wave coroutine
        if (nextWaveCoroutine != null)
        {
            StopCoroutine(nextWaveCoroutine);
            nextWaveCoroutine = null;
        }
    }


    public void KillAllEnemies()
    {
        Debug.Log("Killing all enemies");
        foreach (Enemy enemy in Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            enemy.TakeDamage(9999);
        }

        aliveEnemies = 0;

        // Update UI enemy count
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateEnemies(aliveEnemies);
    }
}
