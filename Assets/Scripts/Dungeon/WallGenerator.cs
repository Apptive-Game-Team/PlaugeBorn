using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        HashSet<Vector2Int> basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        foreach(Vector2Int pos in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleWallTile(pos);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>(); // �ٴ��� ��������, ���� �� �� �ִ� �����ڸ� ��ġ ����

        // ��� �ٴ� Ÿ�� ��ġ�� ���� �˻�
        foreach(Vector2Int pos in floorPositions)
        {
            // ��� �ٴ� Ÿ�� ��ġ�� ���� ��,��,��,�� ������ üũ
            foreach(Vector2Int dir in directionList)
            {
                Vector2Int neighbor = pos + dir; // �ٴ� Ÿ�� ��ġ�� �������� �ٷ� ������ ��ġ�� ���
                if (floorPositions.Contains(neighbor)) continue; // ����� ��ġ�� �ٴ� Ÿ�� ��ġ�� ���ԵǸ� �ٴ���. ��, ���� ������ �� ��輱 ��ġ�� �ƴϹǷ� �н�

                wallPositions.Add(neighbor);
            }
        }

        return wallPositions;
    }
}
