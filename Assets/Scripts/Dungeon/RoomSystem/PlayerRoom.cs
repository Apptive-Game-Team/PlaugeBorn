using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerRoom : RoomGenerator
{
    public GameObject player;

    public List<ItemPlacementData> itemData;

    [SerializeField]
    private PrefabPlacer prefabPlacer;

    /// <summary>
    /// ���� �߾��� ��������, �÷��̾�� �������� ��ġ�ϰ� ��ġ�� ������Ʈ���� ����Ʈ�� ��ȯ
    /// </summary>
    public override List<GameObject> ProcessRoom(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridors)
    {
        ItemPlacementHelper itemPlacementHelper = new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);

        // �������� ��ġ�ϰ�, ��ġ�� �����۵��� ����Ʈ�� ����
        List<GameObject> placedObjects = prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

        // �÷��̾�� �� �߾ӿ� ����
        Vector2Int playerSpawnPoint = roomCenter;

        // �߽ɿ� ��ġ�ϱ� ���� 0.5 ������ ������
        GameObject playerObject = prefabPlacer.CreateObject(player, playerSpawnPoint + new Vector2(0.5f, 0.5f));

        placedObjects.Add(playerObject);

        return placedObjects;
    }
}

// ��ġ�� ���� �����ϰ� ����
public abstract class PlacementData
{
    [SerializeField]
    [Min(0)]
    private int minQuantity = 0;

    [SerializeField]
    [Min(0)]
    [Tooltip("Max is inclusive")]
    private int maxQuantity = 0;

    [SerializeField]
    private int quantity => UnityEngine.Random.Range(minQuantity, maxQuantity + 1);

    public int Quantity
    {
        get => quantity;
    }
}

// ������ ��ġ ������
[SerializeField]
public class ItemPlacementData : PlacementData
{
    private ItemData itemData;

    public ItemData ItemData
    {
        get { return itemData; }
        set { itemData = value; }
    }
}

// �� ��ġ ������
[SerializeField]
public class EnemyPlacementData : PlacementData
{
    private GameObject enemyPrefab;
    private Vector2Int enemySize = Vector2Int.one;

    public GameObject EnemyPrefab
    {
        get { return enemyPrefab; }
        set { enemyPrefab = value; }
    }

    public Vector2Int EnemySize
    {
        get { return enemySize; }
        set { enemySize = value; }
    }
}