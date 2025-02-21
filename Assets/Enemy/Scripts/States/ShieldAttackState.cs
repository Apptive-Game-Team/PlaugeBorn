using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Obsolete]
public class ShieldAttackState : IEnemyState
{
    private ShieldBossAttack bossAttack;
    private float attackDuration;
    public ShieldAttackState(ShieldBossAttack bossAttack, float attackDuration)
    {
        this.bossAttack = bossAttack;
        this.attackDuration = attackDuration;
    }

    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Shield Attack State");
        bossAttack.ShieldAttack(attackDuration);
    }
    public void UpdateState(IEnemyAI enemy)
    {
    }
    public void ExitState(IEnemyAI enemy)
    {
    }
}
