using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���� �� Ư�� ��ġ�� �������� ��ġ�ϴ� Ŭ����.
/// �� ���� Ÿ���� OpenSpace(���� ����) / NearWall(�� ��ó)�� �з�.
/// Ư�� ������ ������ �������� ��ġ�� �� �ִ� ��ǥ�� ��ȯ.
/// ũ�Ⱑ ū �����۵� ������ ��ġ�� ��ġ ����.
/// </summary>
public class ItemPlacementHelper
{
    // NearWall, OpenSpace Ÿ�Կ� ���� Ÿ�� ��ǥ�� �����ϴ� ��ųʸ�
    Dictionary<PlacementType, HashSet<Vector2Int>> tileByType 
        = new Dictionary<PlacementType, HashSet<Vector2Int>>();

    HashSet<Vector2Int> roomFloorNoCorridor;

    /// <summary>
    /// ������. ���� �ٴ� Ÿ�� ������ �޾Ƽ�, OpenSpace, NearWall Ÿ������ �з�
    /// </summary>
    public ItemPlacementHelper(HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridor)
    {
        Graph graph = new Graph(roomFloor); // ���� �ٴ� ��ǥ ������, �׷����� ��ȯ

        this.roomFloorNoCorridor = roomFloorNoCorridor;

        // �� �ٴ� ��ǥ�� Ÿ�� ���
        foreach (Vector2Int position in roomFloorNoCorridor)
        {
            // ���� 8������ üũ�ؼ� �ش� ��ġ�� � Ÿ������ ���
            int neighboursCount8Dir = graph.GetNeighbours8Directions(position).Count;
            PlacementType type = neighboursCount8Dir < 8 ? PlacementType.NearWall : PlacementType.OpenSpace;

            if (tileByType.ContainsKey(type) == false)
                tileByType[type] = new HashSet<Vector2Int>();

            // �� ��ó�ε�, ����� ���������� ����
            if (type == PlacementType.NearWall && graph.GetNeighbours4Directions(position).Count == 4)
                continue;

            tileByType[type].Add(position);
        }
    }

    /// <summary>
    /// Ư�� Ÿ���� ������ �������� ��ġ�� ��ǥ ��ȯ
    /// </summary>
    /// <param name="placementType">��ġ�� ��ǥ�� Ÿ��</param>
    /// <param name="iterationsMax">�ִ� �ݺ� Ƚ��(�ڸ� ã�� ���� ����)</param>
    /// <param name="size">������ ũ��</param>
    /// <param name="addOffset">�������� ������ ������ ���� ����</param>
    /// <returns>��ġ ��ǥ or null</returns>
    public Vector2? GetItemPlacementPosition(PlacementType placementType, int iterationsMax, Vector2Int size, bool addOffset)
    {
        // �������� ũ�Ⱑ, ��ġ�� �� �ִ� �������� ũ�� ���� �� ����.
        int itemArea = size.x * size.y;
        if (tileByType[placementType].Count < itemArea)
            return null;

        int iteration = 0;
        while (iteration < iterationsMax)
        {
            iteration++;
            int index = UnityEngine.Random.Range(0, tileByType[placementType].Count);
            Vector2Int pos = tileByType[placementType].ElementAt(index);

            // ������ ũ�Ⱑ 1x1���� ũ��, �ش� ��ǥ�� �������� �׸�ŭ ���� ������ �ִ��� Ȯ���ؾ� ��.
            if (itemArea > 1)
            {
                (bool result, List<Vector2Int> placementPositions) = PlaceBigItem(pos, size, addOffset);

                // ��ġ�� ���� ������ �ٸ� ��ġ �õ�
                if (result == false) continue;

                // ����� ��ǥ�� ����
                tileByType[placementType].ExceptWith(placementPositions);
                tileByType[PlacementType.NearWall].ExceptWith(placementPositions);
            }
            // 1x1 ũ��� �׳� ��ġ�� �� ����.
            else
            {
                tileByType[placementType].Remove(pos);
            }

            return pos;
        }

        return null;
    }

    /// <summary>
    /// 1x1 ���� ū �������� ��ġ�� ������ ������� Ȯ���ϰ�, �����ϸ� ��ǥ ����Ʈ�� ��ȯ
    /// </summary>
    /// <param name="originPos"></param>
    /// <param name="size"></param>
    /// <param name="addOffset"></param>
    private (bool, List<Vector2Int>) PlaceBigItem(Vector2Int originPos, Vector2Int size, bool addOffset)
    {
        List<Vector2Int> positions = new List<Vector2Int>() { originPos };

        // offset ���
        int maxX = addOffset ? size.x + 1 : size.x;
        int maxY = addOffset ? size.y + 1 : size.y;
        int minX = addOffset ? -1 : 0;
        int minY = addOffset ? -1 : 0;

        // �������� ������ Ÿ�� ��ǥ ���
        for (int row = minX; row <= maxX; row++)
        {
            for (int col = minY; col <= maxY; col++)
            {
                // originalPos�� üũ�� �ʿ� X
                if (col == 0 && row == 0) continue;

                Vector2Int newPosToCheck = new Vector2Int(originPos.x + row, originPos.y + col);
                
                // �������� ���� ���� �߿� �ٴ��� �ƴ� ���� ������, originalPos�� �������� ���� �� ���� ��ǥ�̹Ƿ� ��� �Ұ���.
                if (roomFloorNoCorridor.Contains(newPosToCheck) == false)
                    return (false, positions);

                positions.Add(newPosToCheck);
            }
        }

        return (true, positions);
    }
}

public enum PlacementType
{
    OpenSpace,
    NearWall
}