using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadState : MonoBehaviour, IEnemyState 
{
    public void EnterState(IEnemyAI enemy)
    {
        Debug.Log("Entering Dead State"); // 로그
        enemy.Animator.PlayAnimation(2);
        MonoBehaviour enemyMB = enemy as MonoBehaviour;
        if (enemyMB != null)
        {
            Debug.Log("아니 이거 왜 안됨?");
            Destroy(enemyMB.gameObject, 2.0f);
        }
    }
    public void UpdateState(IEnemyAI enemy)
    {
    }
    public void ExitState(IEnemyAI enemy)
    {
    }
}
