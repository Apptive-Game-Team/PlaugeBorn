using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRange : playerDamage // 콜라이더 활성화 시 공격하는 함수
{
    public LayerMask enemyLayer; // 적 레이어 감지 LayerMask

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
    if (((1 << other.gameObject.layer) & enemyLayer) != 0 && other.CompareTag("Enemy"))
        {
            Debug.Log("적 감지, 공격");
            // 적의 방어력을 가져오기
            /*float enemyDefense = other.GetComponent<Enemy>().defense;

            // 스킬 배수 (필요시 값 변경 가능)
            float skillMultiplier = 1f;

            // 데미지 계산
            float damage = CalculateDamage(enemyDefense, skillMultiplier);

            // 적에게 데미지 적용
            other.GetComponent<Enemy>().TakeDamage(damage); */
        }
    }
}
