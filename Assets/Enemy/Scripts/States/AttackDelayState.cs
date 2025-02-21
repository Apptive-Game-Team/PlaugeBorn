using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDelayState : IEnemyState
{
    private float delayDuration;

    public AttackDelayState(float delayDuration)
    {
        this.delayDuration = delayDuration;
    }
    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Attack Delay State");
        MonoBehaviour enemyMB = enemy as MonoBehaviour;
        enemyMB.StartCoroutine(DelayCoroutine(enemy));
    }
    public void UpdateState(IEnemyAI enemy)
    {
    }
    public void ExitState(IEnemyAI enemy)
    {
    }

    private IEnumerator DelayCoroutine(IEnemyAI enemy)
    {
        yield return new WaitForSeconds(delayDuration);
        enemy.ChangeState(new MoveState());
    }
}
