using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackMove : MonoBehaviour // 기본 평타 및 스킬 모션, 데미지 
{
    private Stat stat;
    private Animator animator;
    private SwordSkillAttack swordSkillAttack;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        stat = GetComponent<Stat>();
        swordSkillAttack = GetComponent<SwordSkillAttack>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // 좌클릭 시 공격
        {
            PlayerAttackMethod();
        }
        else if (Input.GetMouseButtonDown(1)) // 우클릭 유지 차징 시작
        {
            PlayerSkillAttackStart();
        }
        else if (Input.GetMouseButtonUp(1)) // 우클릭 떼면 차징 중단
        {
            PlayerSkillAttackStop();
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    void PlayerAttackMethod() // 데미지 추가 필요 
    {   
        // 평타 모션
        animator.SetBool("Attack", true); 
        animator.SetFloat("AttackState", 0);
        // 데미지
        
    }

    void PlayerSkillAttackStart() 
    {
        swordSkillAttack.StartCharging();
    }

    void PlayerSkillAttackStop()
    {
        swordSkillAttack.StopCharging();
    }

}


