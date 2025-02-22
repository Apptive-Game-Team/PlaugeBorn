using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Cinemachine;

public class PlayerRoom : RoomGenerator
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private List<ItemPlacementData> itemData;

    [SerializeField]
    private PrefabPlacer prefabPlacer;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

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

        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        // ī�޶� �÷��̾� ����ٴϰ� ����
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerObject.transform;
        }

        placedObjects.Add(playerObject);

        return placedObjects;
    }
}