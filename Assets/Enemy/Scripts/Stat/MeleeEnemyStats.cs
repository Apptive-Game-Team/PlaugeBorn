using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyStats : MonoBehaviour, IEnemyStats
{
    [SerializeField] private float Hp = 100.0f;
    // Attack Speed: It be handled in WAIT_SECONDS of IdleState
    // MoveSpeed: It be handled in MeleeMovement
    [SerializeField] private float Defence = 1.0f;
    
    public void TakeHit(float damage)
    {
        // Damage Calculation
        float finalDamage = damage * (100.0f - Defence) / 100.0f;
        SubHp(finalDamage);

        //// Change State to Stun State 
        //GetComponent<MeleeEnemyAI>().ChangeState(new StunState());

        CheckDie();
        GetComponent<MeleeEnemyAI>().ChangeState(new StunState());
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
