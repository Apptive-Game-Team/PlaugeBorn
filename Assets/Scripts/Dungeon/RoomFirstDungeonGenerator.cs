﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;

    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;

    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;

    [SerializeField]
    private bool randomWalkRooms = false; // 방 내부를 랜덤 패턴으로 채울지 여부

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        List<BoundsInt> roomsList = ProceduralGenerationAlgo.BinarySpacePartitioning(new BoundsInt((Vector3Int)base.startPos,
            new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight); // 던전 영역을 분할하여 방 리스트 생성

        // 분할된 방들에 대해 타일을 채우기 위해, 방 내부 범위 안의 모든 좌표를 계산
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateSimpleRooms(roomsList);

        // 분할된 각 방의 중심 좌표를 저장
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (BoundsInt room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        // 방들을 연결하는 복도 생성
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        // 방들에 대해 타일 채움
        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

        // 랜덤한 방 하나를 시작점으로
        Vector2Int currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        // 모든 방들을 연결할 때까지 반복
        while (roomCenters.Count > 0)
        {
            // 현재 방에서 가장 가까운 방의 중심 좌표 찾기
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);

            // 현재 방의 중심과 가장 가까운 방의 중심을 연결하는 복도 생성
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }

        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        Vector2Int pos = currentRoomCenter;
        corridor.Add(pos);

        // y 좌표가 같아지도록 먼저 연결
        while (pos.y != destination.y)
        {
            if (destination.y > pos.y)
            {
                pos += Vector2Int.up;
            }
            else if (destination.y < pos.y)
            {
                pos += Vector2Int.down;
            }

            corridor.Add(pos);
        }

        // 다음에 x 좌표가 같아지도록 연결
        while (pos.x != destination.x)
        {
            if (destination.x > pos.x)
            {
                pos += Vector2Int.right;
            }
            else if (destination.x < pos.x)
            {
                pos += Vector2Int.left;
            }

            corridor.Add(pos);
        }

        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;

        foreach(Vector2Int pos in roomCenters)
        {
            float currDistance = Vector2.Distance(pos, currentRoomCenter);

            if (currDistance < distance)
            {
                distance = currDistance;
                closest = pos;
            }
        }

        return closest;
    }

    /// <summary>
    /// 각 방 내부에 대한 모든 좌표 정보를 계산해서 반환
    /// </summary>
    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        
        // 시작 좌표와 사이즈를 통해 방 내부의 모든 좌표 계산
        foreach (BoundsInt room in roomsList)
        {
            // offset을 통해 각 방들의 간격이 띄어짐
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++) 
                {
                    Vector2Int pos = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(pos);
                }
            }
        }

        return floor;
    }
}
