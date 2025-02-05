using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    // ��ũ���ͺ� ������Ʈ�� ���� ���� ������ �ʿ��� �з�����(�� ���� walk�� Ƚ��, walk�� �ݺ� Ƚ��, startRandomlyEachIteration)�� ������.
    [SerializeField] private SimpleRandomWalkSO randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPostioins = RunRandomWalk(randomWalkParameters); // walkLength��ŭ iterations(�ݺ�)�� random�ϰ� walk�� ��θ� ���� 

        tilemapVisualizer.Clear(); // �� ���� ����, Ÿ�ϸ� �ʱ�ȭ
        tilemapVisualizer.PaintFloorTiles(floorPostioins); // random�ϰ� walk�� ��ο� Ÿ���� �׷��� ������ �ð�ȭ
        WallGenerator.CreateWalls(floorPostioins, tilemapVisualizer); // ������ �ٴڿ� ����, �����ڸ��� �� Ÿ���� �׷��� �� ����
    }

    // walkLength��ŭ walk�ϴ� ������ iterations ��ŭ �ݺ��� ��� ������ ��ȯ.
    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters)
    {
        Vector2Int curr = base.startPos;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // �̶����� walk�� ��θ� ����

        for (int i = 0; i < parameters.iterations; i++)
        {
            HashSet<Vector2Int> path = ProceduralGenerationAlgo.SimpleRandomWalk(curr, parameters.walkLength); // curr�� �������� walkLength��ŭ walk
            floorPositions.UnionWith(path); // curr���� walk�� ��θ� �߰�. UnionWith �Լ��� ���� �ߺ��� ��δ� ���ŵ�

            /* 
             * startRandomlyEachIteration Ȱ��ȭ ��, walk�������� �Ź� ���ο� �������
             * ������ walk�� ��� �߿��� �����ϰ� ��
             * ���� �� �л�� ������ ���� ������ ����
             * 
             * �ݴ�� startRandomlyEachIteration ��Ȱ��ȭ ��, ó�� �������� �������� ��� walk�� ����
             * ���� �� �������� �����, ������(�߾�) �������� ���ߵ� ������ ������ ����
             */
            if (parameters.startRandomlyEachIteration)
                curr = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count)); 
        }

        return floorPositions;
    }
}
