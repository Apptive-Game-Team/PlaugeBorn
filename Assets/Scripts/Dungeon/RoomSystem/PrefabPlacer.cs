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
                else
                {
                    Debug.Log("�߰� ����");
                }
            }
        }
        return placedObjects;
    }

    /// <summary>
    /// �������� ũ�� ������� �������� ���� ��, ��ġ
    /// </summary>
    /// <param name="itemPlacementData"></param>
    /// <param name="itemPlacementHelper"></param>
    /// <returns></returns>
    public List<GameObject> PlaceAllItems(List<ItemPlacementData> itemPlacementData, ItemPlacementHelper itemPlacementHelper)
    {
        List<GameObject> placedObjects = new List<GameObject>();

        IEnumerable<ItemPlacementData> sortedList = new List<ItemPlacementData>(itemPlacementData).OrderByDescending(placementData => placementData.ItemData.Size.x * placementData.ItemData.Size.y);

        foreach (ItemPlacementData placementData in sortedList)
        {
            for (int i = 0; i < placementData.Quantity; i++)
            {
                Vector2? possiblePlacementSpot = itemPlacementHelper.GetItemPlacementPosition(
                    placementData.ItemData.PlacementType,
                    100,
                    placementData.ItemData.Size,
                    placementData.ItemData.AddOffset);


                if (possiblePlacementSpot.HasValue)
                {
                    placedObjects.Add(PlaceItem(placementData.ItemData, possiblePlacementSpot.Value));
                }

                else Debug.Log("�߰� ����");
            }
        }
        return placedObjects;
    }
    /// <summary>
    /// CreateObject()�� ȣ���� �������� ��ġ�� ��, ������ ������ �ʱ�ȭ.
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
