using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;
using Unity.VisualScripting;

public class RoomContentGenerator : MonoBehaviour
{
    // playerRoom�� defaultRoom�� RoomGenerator�� RoomGenerator�� ĳ����.
    [SerializeField]
    private RoomGenerator playerRoom, fightingPitRoom;  

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject obj in spawnedObjects)
            {
                if (obj != null) Destroy(obj);
            }
            RegenerateDungeon?.Invoke();
        }
    }

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
        SelectEnemySpawnPoints(dungeonData);

        foreach (GameObject item in spawnedObjects)
        {
            if (item != null)
                item.transform.SetParent(itemParent, false);
        }
    }

    /// <summary>
    /// �÷��̾ ó�� ������ ���� �����ϰ� ���ϰ� �� ���� ó���� �濡�� ����
    /// </summary>
    private void SelectPlayerSpawnPoint(DungeonData dungeonData)
    {
        int randomRoomIndex = UnityEngine.Random.Range(0, dungeonData.RoomsDictionary.Count);
        Vector2Int playerRoomCenter = dungeonData.RoomsDictionary.Keys.ElementAt(randomRoomIndex);
        Vector2Int playerRoomDictKey = playerRoomCenter;

        // �÷��̾���� ����������, ���ͽ�Ʈ�� �˰��� �����Ͽ� �ٸ� ���� ���� ��� ���
        graphTest.RunDijkstraAlgorithm(playerRoomCenter, dungeonData.FloorPositions);

        // �÷��̾� �뿡 ������ ��ġ.
        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerRoomCenter,
            dungeonData.RoomsDictionary.Values.ElementAt(randomRoomIndex),
            dungeonData.GetRoomFloorWithoutCorridors(playerRoomDictKey)
            );

        spawnedObjects.AddRange(placedPrefabs);
        dungeonData.RemoveRoom(playerRoomCenter);
    }
    
    /// <summary>
    /// ó���� �� ����Ʈ�鿡 ���� ���� ä������
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
