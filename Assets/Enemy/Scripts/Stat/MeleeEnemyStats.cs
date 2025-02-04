using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyStats : MonoBehaviour, IEnemyStats
{
    [SerializeField]
    private float Hp = 100.0f;
    // ���ݼӵ�: IdleState���� WAIT_SECONDS ���� ����
    // �̵��ӵ�: MeleeMovement���� ����
    [SerializeField]
    private float Defence = 1.0f;
    
    public void TakeHit(float damage)
    {
        // ������ ����
        float finalDamage = damage * (100.0f - Defence) / 100.0f;
        SubHp(finalDamage);

        // ���� ���⼭ ������ȯ�� ���������� ��.
        GetComponent<MeleeEnemyAI>().ChangeState(new StunState());

        // �켱 ������ 1�� ����, StunState���� ���
    }

    private void SubHp(float damage)
    {
        this.Hp -= damage;
    }
}
