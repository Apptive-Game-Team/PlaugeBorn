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
    /// ������ �������ִ� ������ ���� �����, ���� ���� ��ǥ�� ������� ������ ����� �Լ�
    /// </summary>
    private void CorridorFirstDungeonGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // ������ ������ ������(Ÿ���� ��ġ��) ��ü ��ǥ ����
        HashSet<Vector2Int> dungeonStartPositions = new HashSet<Vector2Int>(); // �������� ó�� ������ ���� ��ǥ ����(�� �������� ���� ������)

        // ������ ������ ��ǥ ����. floorPositions�� ������ ������ ��ǥ ������ �߰��ϰ� potentialRoomPostions���� ������ ������ ���� ��ǥ ������ �߰���
        List<List<Vector2Int>> corridorPositions = CreateCorridors(floorPositions, dungeonStartPositions);

        // ������ ������ ��ǥ ����
        HashSet<Vector2Int> dungeonFloorPositions = CreateDungeons(dungeonStartPositions);

        // ���� ���� ���� ã��
        List<Vector2Int> isolatedEnds = FindAllDeadEnds(floorPositions);

        // ������ ���鿡 ���� �߰� ������ �����Ͽ� �ڿ������� Ȯ��
        CreateRoomsAtIsolatedEnds(isolatedEnds, dungeonFloorPositions);
        floorPositions.UnionWith(dungeonFloorPositions);

        // ������ ũ�⸦ �������� �� �� ���� ���� ����
        for (int i = 0; i < corridorPositions.Count; i++)
        {
            //corridorPositions[i] = IncreaseCorridorSizeByOne(corridorPositions[i]);
            corridorPositions[i] = IncreaseCorridorBrush3by3(corridorPositions[i]);
            floorPositions.UnionWith(corridorPositions[i]);
        }

        // Ÿ�� ���� �� �� ����
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    /// <summary>
    /// ������ ���鿡 ���� �߰��� ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="isolatedEnds">������ ������ ������ ��ǥ ����</param>
    /// <param name="dungeonFloorPositions">�߰��� ������ ���� ��ǥ ������ �߰��� �з�����</param>
    private void CreateRoomsAtIsolatedEnds(List<Vector2Int> isolatedEnds, HashSet<Vector2Int> dungeonFloorPositions)
    {
        foreach (Vector2Int pos in isolatedEnds)
        {
            if (dungeonFloorPositions.Contains(pos)) continue;

            HashSet<Vector2Int> dungeonFloor = RunRandomWalk(randomWalkParameters, pos);
            dungeonFloorPositions.UnionWith(dungeonFloor);
        }
    }

    /// <summary>
    /// �ֺ��� �ƹ� �͵� ���� ���� ���� ã�� �Լ�
    /// </summary>
    /// <param name="floorPositions">���� ���� ã�� ��ǥ ����</param>
    /// <returns></returns>
    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> isolatedEnds = new List<Vector2Int>();

        foreach(Vector2Int pos in floorPositions)
        {
            int neighborCount = 0;
            // ��ǥ ���յ鿡 ���� ��,��,��,�� Ž��
            foreach(Vector2Int dir in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(pos + dir)) neighborCount++;
            }

            if (neighborCount <= 1) isolatedEnds.Add(pos); // ������ ���� �� �� ���϶��, ������ �ƹ� �������� ���� �����
        }

        return isolatedEnds;
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
    /// ���� ������ �ʿ��� ��ǥ ������ ����� ��ȯ. ������� ��ǥ ������ ù ��° ������ floorPosition�� �߰�.
    /// �� ��° ������ potentialDungeonmPositions���� �������� ��������µ� �־� ������ǥ ������ �߰���.
    /// �������� �����ǰ�, ��� �����鿡 �־� ó�� ������ �����Ǵ� ���� ��ǥ���� ������ �����ϸ�, �������� ���� ����� �� ����.
    /// </summary>
    /// <param name="floorPositions"></param>
    /// <param name="dungeonStartPositions"></param>
    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> dungeonStartPositions)
    {
        Vector2Int currPos = base.startPos;
        dungeonStartPositions.Add(currPos);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < corridorLength; i++)
        {
            List<Vector2Int> corridorPositions = ProceduralGenerationAlgo.RandomWalkCorridor(currPos, corridorLength);

            corridors.Add(corridorPositions);
            currPos = corridorPositions[corridorPositions.Count - 1]; // ���������� ������ ���� ��ǥ�� ����������. �׷��� ������ �̾����鼭 ������

            dungeonStartPositions.Add(currPos); // ������ ��������� �������� ������ ����� ���� ��ǥ�� �߰�
            floorPositions.UnionWith(corridorPositions); // ������ ���� ��ǥ ������ �߰�
        }

        return corridors;
    }

    /// <summary>
    /// ���� ũ�� 2x2 ũ��� ����. �ڳʴ� 3x3
    /// </summary>
    /// <param name="corridorPositions"></param>
    public List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridorPositions)
    {
        List<Vector2Int> newCorridorPositions = new List<Vector2Int>();
        Vector2Int previousDir = Vector2Int.zero;

        for (int i = 1; i < corridorPositions.Count; i++)
        {
            Vector2Int directionalFromCell = corridorPositions[i] - corridorPositions[i - 1]; // ���� ĭ -> ����ĭ ���⺤��

            // ���̴� �κ�(�ڳ�)�� �� 3x3 ũ��� Ȯ��
            if (previousDir != Vector2Int.zero && previousDir != directionalFromCell)
            {
                // 3x3 (-1 ~ 1)
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        newCorridorPositions.Add(corridorPositions[i - 1] + new Vector2Int(x, y));
                    }
                }         
            }
            // ���� ������ �� 2x2 ũ��� Ȯ��
            else
            {
                Vector2Int newCorridorTileOffset = GetDirection90From(directionalFromCell);

                // ���� Ÿ���� �߰��ϰ�, 90�� ȸ���� �������� 1ĭ Ȯ��
                newCorridorPositions.Add(corridorPositions[i - 1]);
                newCorridorPositions.Add(corridorPositions[i - 1] + newCorridorTileOffset);
            }

            previousDir = directionalFromCell;
        }

        return newCorridorPositions;
    }

    /// <summary>
    /// ���� ũ�� �ϰ��ǰ� 3x3 ũ��� Ȯ��
    /// </summary>
    /// <param name="corridorPositions"></param>
    public List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridorPositions)
    {
        List<Vector2Int> newCorridorPositions = new List<Vector2Int>();
        for (int i = 1; i < corridorPositions.Count; i++)
        {
            // 3x3 ũ��� Ȯ��(-1 ~ 1)
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    newCorridorPositions.Add(corridorPositions[i - 1] + new Vector2Int(x, y));
                }
            }
        }

        return newCorridorPositions;
    }

    /// <summary>
    /// ���� ���⿡�� �ð�������� 90�� ȸ���� ���� ��ȯ
    /// </summary>
    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return Vector2Int.right;
        if (direction == Vector2Int.right) return Vector2Int.down;
        if (direction == Vector2Int.down) return Vector2Int.left;
        if (direction == Vector2Int.left) return Vector2Int.up;

        return Vector2Int.zero;
    }
}
