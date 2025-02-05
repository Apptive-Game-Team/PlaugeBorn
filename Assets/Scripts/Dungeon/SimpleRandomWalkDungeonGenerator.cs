using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField] private int iterations = 10;
    [SerializeField] public int walkLength = 10;
    [SerializeField] public bool startRandomlyEachIteration = true; // �����Ǵ� ���� ���¸� ����

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPostioins = RunRandomWalk(); // walkLength��ŭ iterations(�ݺ�)�� random�ϰ� walk�� ��θ� ���� 

        tilemapVisualizer.Clear(); // �� ���� ����, Ÿ�ϸ� �ʱ�ȭ
        tilemapVisualizer.PaintFloorTiles(floorPostioins); // random�ϰ� walk�� ��ο� tilemap�� �׷��� ������ �ð�ȭ
    }

    // walkLength��ŭ walk�ϴ� ������ iterations ��ŭ �ݺ��� ��� ������ ��ȯ.
    protected HashSet<Vector2Int> RunRandomWalk()
    {
        Vector2Int curr = startPos;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // �̶����� walk�� ��θ� ����

        for (int i = 0; i < iterations; i++)
        {
            HashSet<Vector2Int> path = ProceduralGenerationAlgo.SimpleRandomWalk(curr, walkLength); // curr�� �������� walkLength��ŭ walk
            floorPositions.UnionWith(path); // curr���� walk�� ��θ� �߰�. UnionWith �Լ��� ���� �ߺ��� ��δ� ���ŵ�

            /* 
             * startRandomlyEachIteration Ȱ��ȭ ��, walk�������� �Ź� ���ο� �������
             * ������ walk�� ��� �߿��� �����ϰ� ��
             * ���� �� �л�� ������ ���� ������ ����
             * 
             * �ݴ�� startRandomlyEachIteration ��Ȱ��ȭ ��, ó�� �������� �������� ��� walk�� ����
             * ���� �� �������� �����, ������(�߾�) �������� ���ߵ� ������ ������ ����
             */
            if (startRandomlyEachIteration)
                curr = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count)); 
        }

        return floorPositions;
    }
}
