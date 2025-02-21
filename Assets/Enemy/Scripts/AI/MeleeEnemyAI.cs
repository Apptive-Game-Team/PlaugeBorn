using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class MeleeEnemyAI : MonoBehaviour, IEnemyAI
{
    private IEnemyState currentState;
    // 공격 범위
    public float AttackRange = 0.01f;
    public LayerMask playerLayer;

    [SerializeField] private SPUM_Prefabs animator;
    public SPUM_Prefabs Animator => animator;

    private bool isBlocking = false;
    public bool IsBlocking
    {
        get { return isBlocking; }
        set {  isBlocking = value; }
    }

    // Start is called before the first frame update
    private void Start()
    {
        ChangeState(new IdleState());
    }
    // Update is called once per frame
    private void Update()
    {
        currentState.UpdateState(this);
    }
/// <summary>
/// 상태를 바꾸는 함수입니다.
/// </summary>
/// <param name="newState"></param>
    public void ChangeState(IEnemyState newState)
    {
        if (currentState is DeadState)
        {
            Debug.Log("Dead");
            return;
        }
        if (IsBlocking)
        {
            Debug.LogWarning("Now In Charging Attack");
            return;
        }
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public bool IsPlayerInAttackRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, AttackRange, playerLayer);
        return hits.Length > 0;
    }

    public void Move()
    {
        transform.GetComponent<MeleeEnemyMovement>().Move();
    }

    // TODO: 해야 함
    public void AttackPlayer()
    {
        // TODO: 수정해야함.
        return;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    public void KnockBack()
    {
        transform.GetComponent<MeleeEnemyMovement>().KnockBack();
    }
}
