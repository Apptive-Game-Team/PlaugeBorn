using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] public float playerATK; // 플레이어 공격력
    [SerializeField] public float playerSpeed; // 이동 속도
    [SerializeField] public int playerHealth; // 플레이어 체력 (절반 하트 당 1로 계산)
    [SerializeField] public float playerDefence; // 플레이어 방어력
}


    