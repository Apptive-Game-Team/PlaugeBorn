using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �����͸� ������ �ִ� ��ũ���ͺ� ������Ʈ
/// </summary>
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Vector2Int size = new Vector2Int(1, 1);
    [SerializeField] private PlacementType placementType;
    [SerializeField] private bool addOffset;
    [SerializeField] private int health = 1;
    [SerializeField] private bool nonDestructible;

    // ������Ƽ (Getter, Setter)

    public Sprite Sprite
    {
        get => sprite;
        set => sprite = value;
    }

    public Vector2Int Size
    {
        get => size;
        set => size = value;
    }

    public PlacementType PlacementType
    {
        get => placementType;
        set => placementType = value;
    }

    public bool AddOffset
    {
        get => addOffset;
        set => addOffset = value;
    }

    public int Health
    {
        get => health;
        set => health = Mathf.Max(0, value); // �ּ� 0 �̻� ����
    }

    public bool NonDestructible
    {
        get => nonDestructible;
        set => nonDestructible = value;
    }
}
