using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private List<Transform> spawnPoints; // Assign in Inspector

    [Header("Spawn Offset")]
    [SerializeField] private float offsetRange = 10.0f; // tweak in Inspector

    // Spawn a single enemy of given type at random spawn point
    public Enemy SpawnEnemy(EnemyType type)
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Generate small random offset on X/Z
        Vector3 randomOffset = new Vector3(
            Random.Range(-offsetRange, offsetRange),
            0f,
            Random.Range(-offsetRange, offsetRange)
        );

        return enemyPool.GetEnemy(type, point.position + randomOffset, Quaternion.identity);
    }

    // Spawn a single enemy of random type
    public Enemy SpawnRandomEnemy()
    {
        EnemyType randomType = (EnemyType)Random.Range(0, System.Enum.GetValues(typeof(EnemyType)).Length);
        return SpawnEnemy(randomType);
    }

    // Spawn multiple enemies (used by WaveManager later)
    public void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnRandomEnemy();
        }
    }
}
