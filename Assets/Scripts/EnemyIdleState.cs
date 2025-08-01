using UnityEngine;
using System;


public class EnemyIdleState : IEnemyState
{
    public void Enter(Enemy enemy)
    {
        enemy.PlayAnimation("Idle");
    }

    public void Update(Enemy enemy)
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.Player.position);
        if (distance < enemy.chaseRange)
            enemy.ChangeState(new EnemyChaseState());
    }

    public void Exit(Enemy enemy) { }
}
