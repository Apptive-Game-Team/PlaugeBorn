using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� Ŭ������ ����
public static class ProceduralGenerationAlgo
{
    /*
     * HashSet�� C++�� unordered_set�� ������ ���. �����ߺ��� ��� ��� X
     * startPosition���κ��� walkLengh��ŭ ������ ���̸� walk
     * walkLength ����ŭ ������ ����� ������ HashSet<Vector2Int> ���·� ��ȯ
     */
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPos);
        Vector2Int prevPos = startPos;

        for (int i = 0; i < walkLength; i++)
        {
            Vector2Int next = prevPos + Direction2D.GetRandomCardinalDirection(); // �������� �̵��� ���� ��ġ

            path.Add(next);
            prevPos = next;
        }

        return path;
    }
}

// ��,��,��,�� �� ������ (Cardinal Direction)���� ������ ��ȯ
public static class Direction2D 
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
       new Vector2Int(0, 1), // Up
       new Vector2Int(1, 0), // Right
       new Vector2Int(0, -1), // Down
       new Vector2Int(-1, 0) // Left
    };

    public static Vector2Int GetRandomCardinalDirection() { return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)]; }
}

