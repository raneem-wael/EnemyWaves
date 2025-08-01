using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Ghost, Large , Alien }

public class EnemyPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolConfig
    {
        public EnemyType type;
        public Enemy prefab;
        public int initialSize;
    }

    [Header("Pool Settings")]
    [SerializeField] private List<PoolConfig> poolConfigs;

    private Dictionary<EnemyType, Queue<Enemy>> pools = new Dictionary<EnemyType, Queue<Enemy>>();

    private void Awake()
    {
        // Initialize pool for each type
        foreach (var config in poolConfigs)
        {
            Queue<Enemy> enemyQueue = new Queue<Enemy>();

            for (int i = 0; i < config.initialSize; i++)
            {
                Enemy enemy = Instantiate(config.prefab, transform);
                enemy.gameObject.SetActive(false);
                enemy.Initialize(this);
                enemyQueue.Enqueue(enemy);
            }

            pools.Add(config.type, enemyQueue);
        }
    }

    public Enemy GetEnemy(EnemyType type, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(type))
        {
            Debug.LogWarning($"No pool for type {type}");
            return null;
        }

        // If pool empty, create new
        if (pools[type].Count == 0)
        {
            Debug.Log($"Expanding pool for {type}");
            var prefab = poolConfigs.Find(p => p.type == type).prefab;
            var newEnemy = Instantiate(prefab, transform);
            newEnemy.Initialize(this);
            return ActivateEnemy(newEnemy, position, rotation);
        }

        var enemy = pools[type].Dequeue();
        return ActivateEnemy(enemy, position, rotation);
    }

    private Enemy ActivateEnemy(Enemy enemy, Vector3 position, Quaternion rotation)
    {
        enemy.transform.SetPositionAndRotation(position, rotation);
        enemy.gameObject.SetActive(true);
        return enemy;
    }

    public void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        EnemyType type = GetTypeFromPrefab(enemy); // Helper to detect type
        pools[type].Enqueue(enemy);
    }

    private EnemyType GetTypeFromPrefab(Enemy enemy)
    {
        if (enemy.name.Contains("Ghost")) return EnemyType.Ghost; 
        if (enemy.name.Contains("Large")) return EnemyType.Large;  
        return EnemyType.Alien; 
    }
}
