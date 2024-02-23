using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObstacleDB : ScriptableObject
{
    [SerializeField] ObstacleData[] obstacleData;

    public GameObject GetObstacle(int index)
    {
        return obstacleData[index].obstaclePrefab;
    }

    public int GetTotalObstacles()
    {
        return obstacleData.Length;
    }
}
