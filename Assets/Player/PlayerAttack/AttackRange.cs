using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour // 평타 범위 관련 코드
{
    private Vector2 mousePos;
    private float angle;
    private PolygonCollider2D attackCollider;
    public LayerMask enemyLayer;

    void Awake() 
    {
        attackCollider = GetComponent<PolygonCollider2D>();
        attackCollider.enabled = false;
    }
    

   void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 마우스 좌표 얻어옴
        angle = Mathf.Atan2(transform.position.y - mousePos.y, transform.position.x - mousePos.x) * Mathf.Rad2Deg; // 마우스 방향으로 회전전
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        // 좌클릭(평타)시에만 콜라이더 활성화
        if (Input.GetMouseButtonDown(0))
        {
           StartCoroutine(EnableColliderForSeconds(0.2f));
        }
    }

    IEnumerator EnableColliderForSeconds(float duration)
    {
        attackCollider.enabled = true;  // 콜라이더 활성화
        Debug.Log("콜라이더 켜짐");
        yield return new WaitForSeconds(duration); // 일정 시간 대기
        attackCollider.enabled = false; // 다시 비활성화
        Debug.Log("콜라이더 꺼짐");
    }

}
