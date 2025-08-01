using UnityEngine;
using System;


public class EnemyChaseState : IEnemyState
{
    public void Enter(Enemy enemy)
    {
        enemy.PlayAnimation("Walk");
    }

    public void Update(Enemy enemy)
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.Player.position);

        // Move toward player
        enemy.MoveTowardsPlayer();

        if (distance < enemy.attackRange)
            enemy.ChangeState(new EnemyAttackState());
        else if (distance > enemy.chaseRange) // lost player
            enemy.ChangeState(new EnemyIdleState());
    }

    public void Exit(Enemy enemy) { }
}
