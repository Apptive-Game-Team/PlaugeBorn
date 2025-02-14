using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDamage : MonoBehaviour // 데미지 계산함수
{
    private Stat stat;
    private void Awake()
    {
        stat = GetComponent<Stat>();
    }

    /// <summary>
    /// 데미지 계산 함수
    /// </summary>
    /// <param name="enemyDenfense">적 방어력</param>
    /// <param name="skillMultiplier">스킬 배수(데미지 올릴 때)</param>
    /// <returns> 데미지 = 공격력 × (100 / (100 + 적 방어력)) </returns>
    public float CalculateDamage(float enemyDenfense, float skillMultiplier = 1f)
    {
        float damage = stat.playerATK * skillMultiplier * (100f / (100f + enemyDenfense));
        return Mathf.Max(damage, 1); // 최소 데미지 1 보장
    }
}
