using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingPitRoom : RoomGenerator
{
    [SerializeField]
    private PrefabPlacer prefabPlacer;

    [SerializeField]
    private List<EnemyPlacementData> enemyPlacementData;

    [SerializeField]
    private List<ItemPlacementData> itemData;

    public override List<GameObject> ProcessRoom(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridors)
    {
        ItemPlacementHelper itemPlacementHelper = new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);

        // 아이템과 적을 배치
        List<GameObject> placedObjects = prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);
        placedObjects.AddRange(prefabPlacer.PlaceEnemies(enemyPlacementData, itemPlacementHelper));

        return placedObjects;
    }
}
