using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] 
    private int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    private float dungeonPercent = 0.8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstDungeonGeneration();
    }

    /// <summary>
    /// ������ �������ִ� ������ ���� �����, ������ ����� �Լ�
    /// </summary>
    private void CorridorFirstDungeonGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // ������ ������ ������(Ÿ���� ��ġ��) ��ǥ ����
        HashSet<Vector2Int> dungeonStartPositions = new HashSet<Vector2Int>(); // �������� ó�� ������ ���� ��ǥ ����(�� �������� ���� ������)

        /*
         * floorPositions�� ������ ������ ��ǥ ������ �߰��ϰ�
         * potentialRoomPostions���� ������ ������ ���� ��ǥ ������ �߰���
         */
        CreateCorridors(floorPositions, dungeonStartPositions);

        HashSet<Vector2Int> dungeonFloorPositions = CreateDungeons(dungeonStartPositions); // ������ ���� ���� ��ǥ�鿡 ���� ������ ������ ��ǥ ����
        floorPositions.UnionWith(dungeonFloorPositions); // ������ �������� ��ǥ ���� + �������� ��ǥ ����

        tilemapVisualizer.PaintFloorTiles(floorPositions); // ����� ��ǥ ���յ鿡 ���� Ÿ�� ��ġ -> ���� + ���� ����
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer); // �ٴڰ� ���Ҿ� �� ����
    }

    private HashSet<Vector2Int> CreateDungeons(HashSet<Vector2Int> dungeonStartPositions)
    {
        HashSet<Vector2Int> dungeonFloorPositions = new HashSet<Vector2Int>();
        int dungeonToCreateCount = Mathf.RoundToInt(dungeonStartPositions.Count * dungeonPercent); // dungeonPercent �ۼ�Ʈ��ŭ�� ���� ����

        // dungeonStartPositions �����ϰ� ��� dungeonToCreateCount ������ŭ ����
        List<Vector2Int> dungeonStartPositionsToCreate = dungeonStartPositions.OrderBy(x => Guid.NewGuid()).Take(dungeonToCreateCount).ToList();

        // �����ϰ� ���õ� ���� ��ǥ�鿡 ���� ���� ����
        foreach(Vector2Int dungeonStartPos in dungeonStartPositionsToCreate)
        {
            // RunRandomWalk�� ���� �� ��ġ���� ���� ��ũ ������� ���� ��ǥ���� ����
            HashSet<Vector2Int> dungeonPositions = RunRandomWalk(randomWalkParameters, dungeonStartPos); 
            dungeonFloorPositions.UnionWith(dungeonPositions);
        }

        return dungeonFloorPositions;
    }

    /// <summary>
    /// ���� ������ �ʿ��� ��ǥ ������ �����, ������� ��ǥ ������ ù ��° ������ floorPosition�� �߰�.
    /// �� ��° ������ potentialDungeonmPositions �������� ��������µ� �־� ������ǥ ������ �߰���.
    /// �������� �����ǰ�, ��� �����鿡 �־� ó�� ������ �����Ǵ� ���� ��ǥ���� ������ �����ϸ�, �������� ���� ����� �� ����.
    /// </summary>
    /// <param name="floorPositions"></param>
    /// <param name="dungeonStartPositions"></param>
    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> dungeonStartPositions)
    {
        Vector2Int currPos = base.startPos;
        dungeonStartPositions.Add(currPos);

        for (int i = 0; i < corridorLength; i++)
        {
            List<Vector2Int> corridorPositions = ProceduralGenerationAlgo.RandomWalkCorridor(currPos, corridorLength);
            currPos = corridorPositions[corridorPositions.Count - 1]; // ���������� ������ ���� ��ǥ�� ����������. �׷��� ������ �̾����鼭 ������

            dungeonStartPositions.Add(currPos); // ������ ��������� �������� ������ ����� ���� ��ǥ�� �߰�
            floorPositions.UnionWith(corridorPositions); // ������ ���� ��ǥ ������ �߰�
        }
    }
}
