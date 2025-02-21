using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShieldBossEnemyAI : MonoBehaviour, IEnemyAI
{
    private IEnemyState currentState;
    public float AttackRange = 3f;
    public LayerMask playerLayer;

    [Header("공격 판정 변수")] private float DashRange = 3.0f;
    
    [Header("가중치 변수")]
    private float shieldWeight = 1f;
    private float dashWeight = 2f;
    private float sprialWeight = 0.5f;


    // 공격 판정을 위한 플레이어 찾기
    private Transform playerPos;

    [SerializeField] private SPUM_Prefabs animator;
    public SPUM_Prefabs Animator => animator;

    private bool isBlocking = false;
    public bool IsBlocking
    {
        get { return isBlocking; }
        set { isBlocking = value; }
    }

    void Start()
    {
        ChangeState(new IdleState());
        // 만약 이렇게 할 시 분신같은 건 구현 못한다고 봐야함
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

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

    public void AttackPlayer()
    {
        ExecuteRandomPattern();
    }
    public void ExecuteRandomPattern()
    {
        ShieldBossAttack attack = GetComponent<ShieldBossAttack>();
        float totalWeight = shieldWeight + dashWeight + sprialWeight; 
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        if (randomValue < shieldWeight)
        {
            // TODO: 매직넘버 지우기
            attack.ShieldAttack(1.5f);        
        }
        else if (randomValue < shieldWeight + dashWeight)
        {
            attack.Attack(); 
        }
        else
        {
            attack.SpiralAttack();
        }
    }
    public bool IsPlayerInAttackRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, AttackRange, playerLayer);
        return hits.Length > 0;
    }

    public void KnockBack()
    {
        // 보스는 넉백없이 가겠습니다. 
        return;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    public void Move()
    {
        transform.GetComponent<ShieldBossEnemyMovement>().Move();
    }
}
