using UnityEngine;
using System;

public class EnemyAttackState : IEnemyState
{
    public void Enter(Enemy enemy)
    {
        enemy.PlayAnimation("Attack");
        // TODO: apply damage to player (cooldown-based)
    }

    public void Update(Enemy enemy)
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.Player.position);

        if (distance > enemy.attackRange)
            enemy.ChangeState(new EnemyChaseState());
    }

    public void Exit(Enemy enemy) { }
}
