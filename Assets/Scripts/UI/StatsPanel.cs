using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : PanelBase
{
    [Header("�÷��̾��� ����")]
    [SerializeField] private TextMeshProUGUI hpValue;
    [SerializeField] private TextMeshProUGUI attackPowerValue;
    [SerializeField] private TextMeshProUGUI armorValue;
    [SerializeField] private TextMeshProUGUI speedValue;

    private void Awake()
    {
        InputManager.OnStatsPressed += TogglePanel;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateStats();
        gameObject.SetActive(false);
    }
    protected override void OnShow()
    {
        Debug.Log("���� �г��� ���Ƚ��ϴ�!");
        // ����, ���ϸ��̼� �߰� ����
    }
    protected override void OnHide()
    {
        Debug.Log("���� �г��� �������ϴ�!");
    }

    private void WriteHpValueText(int value)
    {
        hpValue.text = value.ToString();
    }

    private void WriteAttackPowerValueText(int value)
    {
        attackPowerValue.text = value.ToString();
    }

    private void WriteArmorValueText(int value)
    {
        armorValue.text = value.ToString();
    }

    private void WriteSpeedValueText(int value)
    {
        speedValue.text = value.ToString();
    }

    public void UpdateStats()
    {
        WriteHpValueText(StatsManager.Instance.Hp);
        WriteAttackPowerValueText(StatsManager.Instance.AttackPower);
        WriteArmorValueText(StatsManager.Instance.Armor);
        WriteSpeedValueText(StatsManager.Instance.Speed);
    }
}
