using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    // ��ũ���ͺ� ������Ʈ�� ���� ���� ������ �ʿ��� �з�����(�� ���� walk�� Ƚ��, walk�� �ݺ� Ƚ��, startRandomlyEachIteration)�� ������.
    [SerializeField] protected SimpleRandomWalkSO randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPostioins = RunRandomWalk(randomWalkParameters, base.startPos); // walkLength��ŭ iterations(�ݺ�)�� random�ϰ� walk�� ��θ� ���� 

        tilemapVisualizer.Clear(); // �� ���� ����, Ÿ�ϸ� �ʱ�ȭ
        tilemapVisualizer.PaintFloorTiles(floorPostioins); // random�ϰ� walk�� ��ο� Ÿ���� �׷��� ������ �ð�ȭ
        WallGenerator.CreateWalls(floorPostioins, tilemapVisualizer); // ������ �ٴڿ� ����, �����ڸ��� �� Ÿ���� �׷��� �� ����
    }

    /// <summary>
    /// walkLength��ŭ walk�ϴ� ������ iterations ��ŭ �ݺ��� ��� ������ ��ȯ.
    /// </summary>
    /// <param name="parameters">���� ������ �ʿ��� �з����͸� ������ �ִ� ��ũ���ͺ� ������Ʈ</param>
    /// <param name="pos">Random Walk�� ������ ��ǥ</param>
    /// <returns></returns>
    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int pos)
    {
        Vector2Int curr = pos;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); // �̶����� walk�� ��θ� ����

        for (int i = 0; i < parameters.iterations; i++)
        {
            HashSet<Vector2Int> path = ProceduralGenerationAlgo.SimpleRandomWalk(curr, parameters.walkLength); // curr�� �������� walkLength��ŭ walk
            floorPositions.UnionWith(path);

            // startRandomlyEachIteration Ȱ��ȭ��, �� �ݺ����� walk �������� ����
            if (parameters.startRandomlyEachIteration)
                curr = path.ElementAt(UnityEngine.Random.Range(0, path.Count)); 
        }

        return floorPositions;
    }
}
