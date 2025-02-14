using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{   
    private LightningDash lightningDash;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LightningDash lightningDash = other.GetComponent<LightningDash>();
            if (lightningDash != null)
            {
                lightningDash.ChargeLightning(); // 플레이어에게 벽력일섬 충전 부여
            }
            Destroy(gameObject); // 충전 후 스프라이트 삭제
        }
    }
}
