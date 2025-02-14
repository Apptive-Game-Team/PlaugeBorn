using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillAttack : MonoBehaviour // 차징 공격 스크립트
{
    public float maxChargeTime = 2f; // 최대 차징 시간
    public float maxSkillRange = 1f; // 최대 공격 범위'
    public float cooldownTime = 5f; // 쿨타임
    public float cooldownTimer = 0f; // 현재 쿨타임 상태
    //skillDamage (Stat.cs)
    //stunDuration 
    private float chargeTime = 0f; // 현재 차징 시간
    private bool isCharging = false;
    public CircleCollider2D chargeCollider;
    public LayerMask enemyLayer;
    [SerializeField] private GameObject chargeEffect; // 원 스프라이트
    private Animator playerAnimator;
    private Stat stat;
    void Start()
    {
        chargeCollider = GetComponentInChildren<CircleCollider2D>();
        chargeCollider.enabled = false; // 처음엔 비활성화
        chargeEffect.SetActive(false);
        playerAnimator = FindObjectOfType<Player>().GetComponentInChildren<Animator>();
        stat = GetComponent<Stat>();
    }

    public void StartCharging()
    {
        if (cooldownTimer > 0) return;
        isCharging = true;
        chargeTime = 0f;
        chargeCollider.enabled = true;
        Debug.Log("스킬 콜라이더 활성화");
        chargeEffect.SetActive(true);
    }

    public void StopCharging()
    {
        isCharging = false;
        chargeCollider.enabled = false;
        Debug.Log("스킬 콜라이더 꺼짐");
        chargeEffect.SetActive(false);
        if (cooldownTimer == 0) 
        {
            playerAnimator.SetBool("Attack", true); 
            playerAnimator.SetFloat("AttackState", 1);
        }
        cooldownTimer = cooldownTime; // 쿨타임 시작
    }

    void Update()
    {
        Player player = FindObjectOfType<Player>(); 
        if (player != null && player.GetComponent<Stat>().playerHealth <= 0) 
        {
            return;
        }  
        // 쿨타임 감소 (0 이하로 내려가지 않도록 제한)
        cooldownTimer = Mathf.Max(0, cooldownTimer - Time.deltaTime);

        // 쿨타임 중이면 실행 금지
        if (cooldownTimer > 0) return;


        // 우클릭을 계속 누르고 있을 때 차징 시작
        if (Input.GetMouseButton(1)) // 우클릭이 눌려 있는 동안
        {
            if (!isCharging)
            {
                StartCharging();
            }

            if (isCharging)
            {
                chargeTime += Time.deltaTime;
                chargeTime = Mathf.Min(chargeTime, maxChargeTime);

                // 범위 조정
                float scale = Mathf.Lerp(0, maxSkillRange, chargeTime / maxChargeTime);
                chargeCollider.radius = scale;
                chargeEffect.transform.localScale = new Vector3(scale, scale, 1f);
                
                // 차징 완료 시 공격 실행
                if (chargeTime >= maxChargeTime)
                {
                    PerformAttack();
                    StopCharging();
                }
            }
        }
        else
        {
            // 우클릭을 떼면 차징 중지
            if (isCharging)
            {
                StopCharging();
            }
        }
    }

    void PerformAttack()
    {
        // 차징 공격 범위 안의 적들을 감지하기 위한 Collider2D 배열
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, chargeCollider.radius, enemyLayer);
        
        foreach (Collider2D enemy in enemiesInRange)
        {
            // 적이 충돌하면 데미지를 적용
            /* Stat enemyStat = enemy.GetComponent<Stat>(); // 적의 Stat 스크립트를 가져옴

            if (enemyStat != null)
            {
                float skillMultiplier = 2; // 스킬 배수
                float enemyDefense = enemyStat.enemyDefense; // 적의 방어력 가져오기
                playerDamage damageCalculator = GetComponent<playerDamage>(); // playerDamage 컴포넌트 가져오기
                
                if (damageCalculator != null)
                {
                    // 데미지 계산
                    float finalDamage = damageCalculator.CalculateDamage(enemyDefense, skillMultiplier);
                    enemyStat.TakeDamage(finalDamage); // 적에게 데미지를 입힘
                }
            } */
            Debug.Log("스킬 공격");
        } 
        
    }
}
