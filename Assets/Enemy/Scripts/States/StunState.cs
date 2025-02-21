using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : IEnemyState
{
    [SerializeField]
    private float WAIT_SECONDS = 2.0f;

    private Coroutine stunCoroutine;
    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Stun State"); // 로그
        enemy.Animator.PlayAnimation(3);
        MonoBehaviour enemyMB = enemy as MonoBehaviour;

        // When Enter the state, Check Coroutine.
        if (enemyMB != null)
        {
            if (stunCoroutine != null)
            {
                enemyMB.StopCoroutine(stunCoroutine);
            }
            enemy.KnockBack();
            stunCoroutine = enemyMB.StartCoroutine(WaitForNextState(enemy));
        }
    }
    public void UpdateState(IEnemyAI enemy)
    {
    }

    public void ExitState(IEnemyAI enemy)
    {
        // When Exit the State, Check Coroutine.
        if (stunCoroutine != null) {
            MonoBehaviour enemyMB = enemy as MonoBehaviour;
            if (enemyMB != null)
            {
                enemyMB.StopCoroutine(stunCoroutine); // 상태 나갈 때 코루틴 정리
            }
            stunCoroutine = null;
        }
    }
    private IEnumerator WaitForNextState(IEnemyAI enemy)
    {
        yield return new WaitForSeconds(WAIT_SECONDS);
        enemy.ChangeState(new MoveState()); 
    }
}
