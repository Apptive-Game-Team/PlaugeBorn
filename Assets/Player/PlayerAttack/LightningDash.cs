using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningDash : MonoBehaviour
{
    private Player player;
    public TrailRenderer trail;
    private Animator animator;
    private Rigidbody2D rigid;
    private int lightningCharges = 0; // 현재 충전된 양
    private int maxLightningCharges = 3; // 최대 스택 가능 개수
    private float lightningDashSpeed = 30f; // 번개 대쉬 속도
    private float lightningDashDuration = 0.2f; // 번개 대쉬 지속 시간
    public BoxCollider2D lightningCollider;
    private CapsuleCollider2D playerbody;
    void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
        lightningCollider = GetComponentInChildren<BoxCollider2D>();
        playerbody = GetComponent<CapsuleCollider2D>();
        lightningCollider.enabled = false;
        rigid = GetComponent<Rigidbody2D>();
    }

    // 벽력일섬 충전 스택 관리하는 함수
    public void ChargeLightning()
    {
        if (lightningCharges < maxLightningCharges)
        {
            lightningCharges++;
        }
    }

    // 벽력일섬 실행 함수
    public void TryUseLightDash()
    {
        if (lightningCharges > 0 && !player.isDashing)
        {
            StartCoroutine(LightDash(lightningDashSpeed, lightningDashDuration));
            lightningCharges--; // 사용 후 스택 감소
        }
    }

    IEnumerator LightDash(float speed, float duration) // 벽력일섬 함수 (회피 추가 필요(플레이어 콜라이더 비활성화))
    {
        player.isDashing = true;
        playerbody.enabled = false;
        animator.speed = 0.1f;
        animator.SetBool("Attack", true);
        trail.enabled = true;  // 궤적 효과 켜기

        if (lightningCollider != null) // 공격 콜라이더 활성화
        {
            lightningCollider.enabled = true;
            Debug.Log("벽력일섬 콜라이더 활성화");
        }

        yield return new WaitForSeconds(1f); // 선딜

        animator.SetFloat("AttackState", 0);
        Vector2 dashDirection = player.inputVec.normalized;
        if (dashDirection == Vector2.zero)
        {
            dashDirection = Vector2.left * Mathf.Sign(transform.localScale.x);
        }

        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            rigid.MovePosition(rigid.position + dashDirection * speed * Time.fixedDeltaTime);
            yield return null;
        }

        player.isDashing = false;
        animator.SetBool("Attack", false);
        animator.speed = 1f;
        trail.enabled = false;  // 궤적 효과 끄기
        playerbody.enabled = true;
        // 공격 범위 비활성화
        if (lightningCollider != null)
        {
            lightningCollider.enabled = false;
            Debug.Log("벽력일섬 콜라이더 비활성화");
        }
    }
}