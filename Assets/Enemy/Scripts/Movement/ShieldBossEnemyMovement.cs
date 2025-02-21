using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBossEnemyMovement : MonoBehaviour, IEnemyMovement
{
    [Header("이동 속도 관련 변수")]
    public float moveSpeed = 3f; // 보스는 살짝  느리게

    [Header("플레이어 변수(레이어로 잡음)")]
    private Transform player;
    // 넉백은 없으므로 관련 변수 없습니다.

    [Header("방향관련변수")]
    private Vector3 direction; 
    [Header("인식 범위 변수")]
    public const float RecognizeRange = 10.0f;
    public float MoveSpeed 
    {
        get { return moveSpeed; }
        set { moveSpeed = value; } // 가속은 무제한
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public void Move()
    {
        direction = player.position - transform.position;
        if (direction.magnitude > RecognizeRange)
        {
            return;
        }
        
        direction.Normalize();
        
        Vector3 scale = transform.localScale;
        if (player.position.x > transform.position.x)
        {
            scale.x = -3.0f;
        }
        else
        {
            scale.x = 3.0f;
        }
        transform.localScale = scale;

        transform.position += direction * (moveSpeed * Time.deltaTime);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
