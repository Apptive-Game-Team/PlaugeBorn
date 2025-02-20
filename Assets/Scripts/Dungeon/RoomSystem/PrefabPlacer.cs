using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PrefabPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;

    /// <summary>
    /// enemyPlacementData ����Ʈ�� ��ȸ�ϸ鼭, �� ���� ���ϴ� ������ŭ ��ġ
    /// </summary>
    public List<GameObject> PlaceEnemies(List<EnemyPlacementData> enemyPlacementData, ItemPlacementHelper itemPlacementHelper)
    {
        List<GameObject> placedObjects = new List<GameObject>();

        foreach (EnemyPlacementData placementData in enemyPlacementData)
        {
            for (int i = 0; i < placementData.Quantity; i++)
            {
                // ��ġ ������ ��ǥ�� ����
                Vector2? possiblePlacementSpot = itemPlacementHelper. GetItemPlacementPosition(
                    PlacementType.OpenSpace,
                    100,
                    placementData.EnemySize,
                    false
                    );

                // ��ǥ�� ���� �Ǿ�����, ����Ʈ�� �߰�
                if (possiblePlacementSpot.HasValue)
                { 
                    placedObjects.Add(CreateObject(placementData.EnemyPrefab, possiblePlacementSpot.Value + new Vector2(0.5f, 0.5f))); //Instantiate(placementData.enemyPrefab,possiblePlacementSpot.Value + new Vector2(0.5f, 0.5f), Quaternion.identity)
                }
            }
        }
        return placedObjects;
    }

    /// <summary>
    /// �������� ũ�� ������� �������� ���� ��, ��ġ
    /// </summary>
    public List<GameObject> PlaceAllItems(List<ItemPlacementData> itemPlacementData, ItemPlacementHelper itemPlacementHelper)
    {
        List<GameObject> placedObjects = new List<GameObject>();

        IEnumerable<ItemPlacementData> sortedList = new List<ItemPlacementData>(itemPlacementData).OrderByDescending(placementData => placementData.ItemData.Size.x * placementData.ItemData.Size.y);

        foreach (ItemPlacementData placementData in sortedList)
        {
            // �������� Quantity ��ŭ ��ġ
            for (int i = 0; i < placementData.Quantity; i++) 
            {
                // �ش� �������� ��ġ�� �� �ִ� Ÿ�� ��ǥ�� ���
                Vector2? possiblePlacementSpot = itemPlacementHelper.GetItemPlacementPosition(
                    placementData.ItemData.PlacementType,
                    100,
                    placementData.ItemData.Size,
                    placementData.ItemData.AddOffset);

                // ��ġ�� �� �ִ� ��ǥ�� ���� �Ǿ����� prefab�� �ش� ��ǥ�� �ν��Ͻ�ȭ
                if (possiblePlacementSpot.HasValue)
                {
                    placedObjects.Add(PlaceItem(placementData.ItemData, possiblePlacementSpot.Value));
                }
            }
        }
        return placedObjects;
    }
    /// <summary>
    /// CreateObject()�� ȣ���� �������� ��ġ(�ν��Ͻ�ȭ)�� ��, ������ ������ �ʱ�ȭ.
    /// </summary>
    private GameObject PlaceItem(ItemData item, Vector2 placementPosition)
    {
        GameObject newItem = CreateObject(itemPrefab, placementPosition);
        //GameObject newItem = Instantiate(itemPrefab, placementPosition, Quaternion.identity);
        newItem.GetComponent<Item>().Init(item);

        return newItem;
    }

    /// <summary>
    /// �������� �Ѱܹ��� ��ǥ�� ����
    /// </summary>
    public GameObject CreateObject(GameObject prefab, Vector3 placementPosition)
    {
        if (prefab == null) return null;

        GameObject newItem;

        // ������ ���� ���̸� Instantiate()��
        if (Application.isPlaying)
        {
            newItem = Instantiate(prefab, placementPosition, Quaternion.identity);
        }
        // �����Ϳ��� ���� ���̸�, PrefabUtility.InstantiatePrefab()�� 
        else
        {
            newItem = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            newItem.transform.position = placementPosition;
            newItem.transform.rotation = Quaternion.identity;
        }

        return newItem;
    }
}
