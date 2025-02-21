using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        HashSet<Vector2Int> basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList); // �������� �� ��ǥ ���
        HashSet<Vector2Int> cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList); // �밢���� �� ��ǥ ���

        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (Vector2Int pos in cornerWallPositions)
        {
            string neighbourBinaryType = "";

            // �� ������ �������� �ð�������� 8������ üũ
            foreach (Vector2Int direcction in Direction2D.eightDirectionsList)
            {
                Vector2Int neighbourPos = pos + direcction;

                // �ش� �ֺ� ���⿡ �ٴ��� ������ 1, ������ 0
                if (floorPositions.Contains(neighbourPos))
                    neighbourBinaryType += "1";
                else
                    neighbourBinaryType += "0";
            }

            // 2���� ��Ʈ�� �´� ������ �ڳ� ���� ����
            tilemapVisualizer.PaintSingleCornerWall(pos, neighbourBinaryType);
        }
    }

    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (Vector2Int pos in basicWallPositions)
        {
            string neighboursBinaryType = "";
            
            // �� -> �� -> �� -> �� ��(�ð����)���� üũ
            foreach (Vector2Int direction in Direction2D.cardinalDirectionsList)
            {
                var neighbourPos = pos + direction;

                // �ش� �ֺ� ���⿡ �ٴ��� ������ 1, ������ 0
                if (floorPositions.Contains(neighbourPos)) 
                    neighboursBinaryType += "1";
                else
                    neighboursBinaryType += "0";
            }

            // 2���� ��Ʈ�� �´� ������ �⺻ ���� ����
            tilemapVisualizer.PaintSingleWallTile(pos, neighboursBinaryType);
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
