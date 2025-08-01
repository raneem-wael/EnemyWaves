using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Ranges")]
    [SerializeField] private float chaseRange = 60f;
    [SerializeField] private float attackRange = 150f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    private Transform player;
    private Animator animator;
    private EnemyPool pool;

    // Events
    public static event Action<Enemy> OnEnemyDied;

    // FSM states
    private enum EnemyState { Idle, Chase, Attack, Dead }
    private EnemyState currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        ChangeState(EnemyState.Idle);
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdle();
                break;

            case EnemyState.Chase:
                UpdateChase();
                break;

            case EnemyState.Attack:
                UpdateAttack();
                break;

            case EnemyState.Dead:
                // No behavior when dead (animation event or timer can return to pool)
                break;
        }
    }

    // =============================
    // State Logic
    // =============================

    private void UpdateIdle()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseRange)
        {
            ChangeState(EnemyState.Chase);
        }
    }

    private void UpdateChase()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Move toward player
        MoveTowardsPlayer();

        if (distance <= attackRange)
        {
            ChangeState(EnemyState.Attack);
        }
        else if (distance > chaseRange)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    private void UpdateAttack()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Look at player but don't move
        LookAtPlayer();

        // Trigger attack animation
        PlayAnimation("Attack");

        if (distance > attackRange)
        {
            ChangeState(EnemyState.Chase);
        }
    }

    // =============================
    // State Helpers
    // =============================

    private void ChangeState(EnemyState newState)
    {
        // Exit logic for current state
        if (currentState == EnemyState.Attack)
        {
            // Stop attack animations if needed
        }

        currentState = newState;

        // Enter logic
        switch (newState)
        {
            case EnemyState.Idle:
                PlayAnimation("Idle");
                break;
            case EnemyState.Chase:
                PlayAnimation("Walk");
                break;
            case EnemyState.Attack:
                PlayAnimation("Attack");
                break;
            case EnemyState.Dead:
                PlayAnimation("Die");
                break;
        }
    }

    private void PlayAnimation(string animName)
    {
        if (animator != null)
            animator.Play(animName);
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    // =============================
    // Health & Pooling
    // =============================

    public void Initialize(EnemyPool poolReference)
    {
        pool = poolReference;
    }

    public void TakeDamage(int damage)
    {
        if (currentState == EnemyState.Dead) return;

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

    private void Die()
    {
        OnEnemyDied?.Invoke(this);
        ChangeState(EnemyState.Dead);

        // After animation ends, return to pool (or delay a bit)
        Invoke(nameof(ReturnToPool), 1f); // Wait 1s for animation
    }

    private void ReturnToPool()
    {
        if (pool != null)
            pool.ReturnEnemy(this);
        else
            gameObject.SetActive(false);
    }
}
