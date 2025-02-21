using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class RangedEnemyMovement : MonoBehaviour, IEnemyMovement
{
    public float moveSpeed = 3f;
    private Transform player;

    [Header("넉백 관련 변수")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.1f;

    [Header("인식 범위 변수")] public const float RecognizeRange = 10.0f;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            // 하한선 상한선 설정
            if (value < 0f) value = 0f;
            if (value > 20f) value = 20f;
            moveSpeed = value;
        }
    }
    public void Move()
    {
        Vector3 direction = player.position - transform.position;
        if (direction.magnitude > RecognizeRange)
        {
            return;
        }
        Vector3 scale = transform.localScale; 
        if (player.position.x > transform.position.x)
        {
            scale.x = -1.0f;
        }
        else
        {
            scale.x = 1.0f;
        }
        transform.localScale = scale;

        direction.Normalize();

        transform.position += direction * (moveSpeed * Time.deltaTime);
    }
    public void KnockBack()
    {
        Vector2 direction = player.position - transform.position;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.AddForce(-direction * knockbackForce, ForceMode2D.Impulse);
        // TODO: 매직넘버 삭제
        StartCoroutine(StopKnockBackAfterDelay(rb, knockbackDuration));
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;    
    }
    private IEnumerator StopKnockBackAfterDelay(Rigidbody2D rb, float knockbackDuration)
    {
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
    }
}
