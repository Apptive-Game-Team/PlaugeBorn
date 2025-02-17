using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �����͸� ������ ����.(���� �� �� ���, ���� ��ǥ, �ٴ� ��ǥ)
/// </summary>
public class DungeonData
{
    public Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary;
    public HashSet<Vector2Int> floorPositions;
    public HashSet<Vector2Int> corridorPositions;

    /// <summary>
    /// ������ ������ �ش� ���� ��ǥ ��ȯ
    /// </summary>
    public HashSet<Vector2Int> GetRoomFloorWithoutCorridors(Vector2Int dictionaryKey)
    {
        HashSet<Vector2Int> roomFloorNoCorridors = new HashSet<Vector2Int>(roomsDictionary[dictionaryKey]);
        roomFloorNoCorridors.ExceptWith(corridorPositions); // ���� ��ǥ�� ����

        return roomFloorNoCorridors;
    }
}
