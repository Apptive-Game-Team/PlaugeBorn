using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI : MonoBehaviour, IEnemyAI
{
    private IEnemyState currentState;

    public float AttackRange = 5f;
    public LayerMask playerLayer;

    [SerializeField]
    private SPUM_Prefabs animator;
    public SPUM_Prefabs Animator => animator;

    private bool isBlocking = false;
    public bool IsBlocking
    {
        get { return isBlocking; }
        set { isBlocking = value; }
    }
    // Start is called before the first frame update
    public void Start()
    {
        ChangeState(new IdleState());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);    
    }

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
        transform.GetComponent<RangedEnemyMovement>().Move();
    }

    public void AttackPlayer()
    {
        transform.GetComponent<RangedEnemyAttack>().Attack();
    }

    // TODO: 넉백 구현하기
    public void KnockBack()
    {
        transform.GetComponent<RangedEnemyMovement>().KnockBack();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
