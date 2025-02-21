using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMovement : MonoBehaviour, IEnemyMovement
{
    public float moveSpeed = 5f;
    private Transform player;

    [Header("넉백 관련 변수")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.1f;

    [Header("인식 범위 변수")] public const float RecognizeRange = 10.0f;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set
        {
            if (value < 0f) value = 0f;
            if (value > 20f) value = 20f;
            moveSpeed = value;
        }
    }

    /// <summary>
    /// 적을 플레이어 방향으로 움직이게 하는 함수입니다.
    /// </summary>
    public void Move()
    {
        Vector3 scale = transform.localScale; 
        Vector3 direction = player.position - transform.position;
        if (direction.magnitude > RecognizeRange)
        {
            return;
        }
        
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
    // Start is called before the first frame update
    void Start()
    {
        // 태그로 플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    private IEnumerator StopKnockBackAfterDelay(Rigidbody2D rb, float knockbackDuration)
    {
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
    }
}
