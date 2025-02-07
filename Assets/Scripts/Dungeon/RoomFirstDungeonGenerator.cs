using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;

    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;

    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;

    [SerializeField]
    private bool randomWalkRooms = false; // �� ���θ� ���� �������� ä���� ����

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        List<BoundsInt> roomsList = ProceduralGenerationAlgo.BinarySpacePartitioning(new BoundsInt((Vector3Int)base.startPos,
            new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight); // ���� ������ �����Ͽ� �� ����Ʈ ����

        // ���ҵ� ��鿡 ���� Ÿ���� ä��� ����, �� ���� ���� ���� ��� ��ǥ�� ���
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateSimpleRooms(roomsList);

        // ��鿡 ���� Ÿ�� ä��
        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    /// <summary>
    /// �� �� ���ο� ���� ��� ��ǥ ������ ����ؼ� ��ȯ
    /// </summary>
    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        
        // ���� ��ǥ�� ����� ���� �� ������ ��� ��ǥ ���
        foreach (BoundsInt room in roomsList)
        {
            // offset�� ���� �� ����� ������ �����
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++) 
                {
                    Vector2Int pos = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(pos);
                }
            }
        }

        return floor;
    }
}
