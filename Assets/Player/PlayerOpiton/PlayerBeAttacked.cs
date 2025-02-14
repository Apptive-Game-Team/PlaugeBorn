using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeAttacked : MonoBehaviour
{
    private Player player;
    private Stat stat;
    private CapsuleCollider2D playerbody;
    // 레이어 마스크 필요 시 추가

    void Awake()
    {
        player = GetComponent<Player>();
        stat = GetComponent<Stat>();
        playerbody = GetComponent<CapsuleCollider2D>();
        playerbody.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other) { // EnemyAttack 태그에 맞았을 때 피격되는 것으로 일단 구현
        if (other.CompareTag("EnemyAttack")) {
            stat.playerHealth -= 1;
            Debug.Log("피격, 체력 -1 감소");
            return;
        }
        if (stat.playerHealth <= 0)
        {
            player.Die();
            return;
        }
    }

}