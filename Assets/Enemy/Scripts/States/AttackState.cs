using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    public float DELAY_DURATION = 1.5f;
    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Attack State"); // 로그
        enemy.Animator.PlayAnimation(4);
        enemy.AttackPlayer();
    }
    public void UpdateState(IEnemyAI enemy)
    {
        enemy.ChangeState(new AttackDelayState(DELAY_DURATION));
    }
    public void ExitState(IEnemyAI enemy)
    {
    }
}
