using UnityEngine;
using System;


public class EnemyDeadState : IEnemyState
{
    public void Enter(Enemy enemy)
    {
        enemy.PlayAnimation("Die");
        enemy.Invoke(nameof(enemy.ReturnToPool), 2f); // Wait for animation
    }

    public void Update(Enemy enemy) { }
    public void Exit(Enemy enemy) { }
}
