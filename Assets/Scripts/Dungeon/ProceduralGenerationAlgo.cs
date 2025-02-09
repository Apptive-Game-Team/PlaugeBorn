using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� Ŭ������ ����
public static class ProceduralGenerationAlgo
{
    /// <summary>
    /// HashSet�� C++�� unordered_set�� ������ ���. �����ߺ��� ��� ��� X
    /// startPosition���κ��� walkLengh��ŭ ������ ���̸� walk
    /// walkLength ����ŭ ������ ����� ������ HashSet<Vector2Int> ���·� ��ȯ
    /// </summary>
    /// <param name="startPos"> random walk�� ������</param>
    /// <param name="walkLength">�� ���� �ݺ����� ������ walk ��</param>
    /// <returns></returns>
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

    /// <summary>
    /// ������ ���� ��ġ���� �����Ͽ� ������ �������� ���� ���̸�ŭ ������ ����.
    /// ���������� ������ walk ��ǥ �ε����� �����ϱ� ���� List ���. HashSet�� �ε��� ������ �ȵ�.
    /// ���������� ������ walk ��ǥ���� �ٽ� ������ ������ ��� �̾����� ����.
    /// </summary>
    /// <param name="startPos">������ ������ ������</param>
    /// <param name="corridorLength">�� ���� �ݺ����� ������ ������ ����</param>
    /// <returns></returns>
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>(); // ���� ��θ� �����ϴ� ����Ʈ
        corridor.Add(startPos);

        Vector2Int dir = Direction2D.GetRandomCardinalDirection(); // ������ ������ ����
        Vector2Int curr = startPos;

        for (int i = 0; i < corridorLength; i++)
        {
            curr += dir; // ���� ��ġ���� �� �������� ���ư�
            corridor.Add(curr);
        }

        return corridor;
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

