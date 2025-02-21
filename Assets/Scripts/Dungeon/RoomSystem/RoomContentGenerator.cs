using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;
using Unity.VisualScripting;

public class RoomContentGenerator : MonoBehaviour
{
    // playerRoom�� defaultRoom�� RoomGenerator�� RoomGenerator�� ĳ����. boosRoom�� ���� ����
    [SerializeField]
    private RoomGenerator playerRoom, bossRoom, fightingPitRoom;  

    List<GameObject> spawnedObjects = new List<GameObject>();

    [SerializeField]
    private GraphTest graphTest;

    [SerializeField]
    private Transform itemParent;

    [SerializeField]
    private UnityEvent RegenerateDungeon;
    
    // Update is called once per frame
    void Update()
    {
        // �׽�Ʈ ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject obj in spawnedObjects)
            {
                if (obj != null) Destroy(obj);
            }
            RegenerateDungeon?.Invoke();
        }
    }

    /// <summary>
    /// ��� ���� �濡 ����, �� ������ ���ϰ� ������Ʈ�� ��ġ
    /// </summary>
    public void GenerateRoomContent(DungeonData dungeonData)
    {
        // �����ϱ� ����, ��ġ�� �����۵� ����
        foreach(GameObject item in spawnedObjects)
        {
            DestroyImmediate(item);
        }
        spawnedObjects.Clear();

        // �÷��̾���� ������ ������ ����, ���� �ִ� ���� ��
        SelectPlayerSpawnPoint(dungeonData);
        SelectBossSpawnPoint(dungeonData);
        SelectEnemySpawnPoints(dungeonData);

        foreach (GameObject item in spawnedObjects)
        {
            // ��ġ�� �������� RoomContent ������Ʈ�� �ڽ�����
            if (item != null)
                item.transform.SetParent(itemParent, false);
        }
    }

    /// <summary>
    /// �÷��̾ ó�� ������ ���� �����ϰ� ���ϰ�, �������� ��ġ. �� ���� ó���� �濡�� ����
    /// </summary>
    private void SelectPlayerSpawnPoint(DungeonData dungeonData)
    {
        int playerRoomIndex = UnityEngine.Random.Range(0, dungeonData.RoomsDictionary.Count);
        Vector2Int playerRoomCenter = dungeonData.RoomsDictionary.Keys.ElementAt(playerRoomIndex);
        Vector2Int playerRoomDictKey = playerRoomCenter;

        // �÷��̾���� ����������, ���ͽ�Ʈ�� �˰��� �����Ͽ� �ٸ� ���� ���� ��� ���
        graphTest.RunDijkstraAlgorithm(playerRoomCenter, dungeonData.FloorPositions);

        // �÷��̾� �뿡 ������ ��ġ
        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerRoomCenter,
            dungeonData.RoomsDictionary.Values.ElementAt(playerRoomIndex),
            dungeonData.GetRoomFloorWithoutCorridors(playerRoomDictKey)
            );

        spawnedObjects.AddRange(placedPrefabs);
        dungeonData.RemoveRoom(playerRoomCenter);
    }


    /// <summary>
    /// �÷��̾ ó�� ������ ���� �����ϰ� ���ϰ�, �������� ��ġ. �� ���� ó���� �濡�� ����.
    /// </summary>
    private void SelectBossSpawnPoint(DungeonData dungeonData)
    {
        // ���ͽ�Ʈ�� �˰�������, �÷��̾� �濡�� ���� ����� ���� Ÿ�� ��ǥ
        Vector2Int highestDijkstraValueTile = graphTest.getHighestValueTile(); 
        Vector2Int bossRoomCenter = Vector2Int.zero;

        // ����� ��ȸ�ϸ�, ���� ���� ����� Ÿ���� ���� ���� ã��.
        foreach (var room in dungeonData.RoomsDictionary)
        {
            if (room.Value.Contains(highestDijkstraValueTile))
            {
                bossRoomCenter = room.Key;
                break;
            }
        }

        Vector2Int bossRoomDictKey = bossRoomCenter;

        // ���� �뿡 ������ ��ġ.
        List<GameObject> placedPrefabs = bossRoom.ProcessRoom(
            bossRoomCenter,
            dungeonData.RoomsDictionary[bossRoomCenter],
            dungeonData.GetRoomFloorWithoutCorridors(bossRoomDictKey)
            );

        spawnedObjects.AddRange(placedPrefabs);
        dungeonData.RemoveRoom(bossRoomCenter);
    }

    /// <summary>
    /// ���� ���� �濡 ����, ���� �������� ��ġ.
    /// </summary>
    private void SelectEnemySpawnPoints(DungeonData dungeonData)
    {
        foreach (KeyValuePair<Vector2Int, HashSet<Vector2Int>> roomData in dungeonData.RoomsDictionary) 
        {
            spawnedObjects.AddRange(fightingPitRoom.ProcessRoom(
                roomData.Key,
                roomData.Value,
                dungeonData.GetRoomFloorWithoutCorridors(roomData.Key))
            );
        }
    }
}
