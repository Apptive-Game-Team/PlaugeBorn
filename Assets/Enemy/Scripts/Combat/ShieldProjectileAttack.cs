using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProjectileAttack : MonoBehaviour
{
    private ShieldBossAttack bossAttack;
    private Vector2 dir;
    private float speed = 5.0f; // 기본값
    private float duration = 2.0f; // 기본값

    public void SetBossAttack(ShieldBossAttack attack)
    {
        bossAttack = attack;
    }

    public void SetDir(Vector2 dir)
    {
        this.dir = dir;
    }

    public IEnumerator ShieldAttack()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(dir * speed, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);

        rb.velocity = Vector2.zero;

        rb.AddForce(-dir * speed, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);

        Destroy(gameObject); 
    }

    /// <summary>
    /// 기본 변수 setter
    /// </summary>
    /// <param name="speed">날아가는 속도 설정</param>
    /// <param name="duration">날아가는 기간 설정</param>
    public void SetBasicValue(float speed, float duration)
    {
        this.speed = speed;
        this.duration = duration;
    }

    private void OnDestroy()
    {
        if (bossAttack != null)
        {
            bossAttack.RemoveShield(gameObject);
        }
    }
}
