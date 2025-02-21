using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBossEnemyStats : MonoBehaviour, IEnemyStats
{
    [SerializeField] private float Hp = 100.0f;
    [SerializeField] private float Defence = 5.0f;
    public void TakeHit(float damage)
    {
        float finalDamage = damage * (100.0f - Defence) / 100.0f;
        SubHp(finalDamage);
        
        // 보스는 스턴 없이 진행
        CheckDie();
        GetComponent<ShieldBossEnemyAI>().ChangeState(new MoveState());
    }
    private void SubHp(float damage)
    {
        this.Hp -= damage;
    }
    private void CheckDie()
    {
        if (this.Hp <= 0)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            IEnemyAI ai = GetComponent<IEnemyAI>();
            // 블락 차단 무조건 진입할 수 있게
            ai.IsBlocking = false;
            ai.ChangeState(new DeadState());
        }
    }
}
