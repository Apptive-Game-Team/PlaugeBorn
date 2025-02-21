using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class SpiralBulletAttack : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed = 5f;

    private void OnEnable()
    {
        // TODO: 매직넘버 지우기
        Invoke("Destroy", 8f);
    }

    void Start()
    {
    }

    void Update()
    {
        transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
    }

    public void SetMoveDirection(Vector2 moveDirection)
    {
        this.moveDirection = moveDirection;
    }

    private void Destroy()
    {
        BulletPool.Instance.ReturnBullet(this.GameObject());
    }
}