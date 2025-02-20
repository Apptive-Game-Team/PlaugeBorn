using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomGenerator : RoomGenerator
{
    [SerializeField]
    private GameObject boss;

    [SerializeField]
    private List<ItemPlacementData> itemData;

    [SerializeField]
    private PrefabPlacer prefabPlacer;

    public override List<GameObject> ProcessRoom(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridors)
    {
        ItemPlacementHelper itemPlacementHelper = new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);

        // �������� ��ġ�ϰ�, ��ġ�� �����۵��� ����Ʈ�� ����
        List<GameObject> placedObjects = prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

        // �÷��̾�� �� �߾ӿ� ����
        Vector2Int bossSpawnPoint = roomCenter;

        // �߽ɿ� ��ġ�ϱ� ���� 0.5 ������ ������
        GameObject bossObject = prefabPlacer.CreateObject(boss, bossSpawnPoint + new Vector2(0.5f, 0.5f));

        placedObjects.Add(bossObject);

        return placedObjects;
    }
}
