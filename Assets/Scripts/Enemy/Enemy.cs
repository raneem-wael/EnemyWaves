using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int maxHealth = 100;
    protected int currentHealth;

    [Header("Ranges")]
    public float chaseRange = 60f;
    public float attackRange = 150f;

    public Transform Player { get; private set; }

    private IEnemyState currentState;
    private Animator animator;
    private EnemyPool pool;

    public static event Action<Enemy> OnEnemyDied;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;
        ChangeState(new EnemyIdleState());
    }

    private void Update()
    {
        currentState?.Update(this);
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public void PlayAnimation(string animName)
    {
        animator.Play(animName);
    }

    public void MoveTowardsPlayer()
    {
        // 1. Get direction to player
        Vector3 direction = (Player.position - transform.position).normalized;
        direction.y = 0f; // ignore vertical tilt so enemy doesn't look up/down

        // 2. Rotate towards player smoothly
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // rotation speed

        // 3. Move towards player
        transform.position += direction * Time.deltaTime * 3f; // movement speed
    }

    public void Initialize(EnemyPool poolReference)
    {
        pool = poolReference;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnMouseDown()
    {
        TakeDamage(50);
    }

    protected virtual void Die()
    {
        OnEnemyDied?.Invoke(this);
        ChangeState(new EnemyDeadState());
    }

    public void ReturnToPool()
    {
        if (pool != null)
            pool.ReturnEnemy(this);
        else
            gameObject.SetActive(false);
    }
}
