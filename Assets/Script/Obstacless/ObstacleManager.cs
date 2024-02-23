
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] ObstacleDB obstacleDB;
    [Serializable]
    class YPos 
    { 
        public List<bool> yPos; 

        public bool GetBool(int yPosition)
        {
            return yPos[yPosition];
        }
    }

    [SerializeField] YPos[] XPos;

    public bool IsObstaclePresent(int xPosition,int yPosition)
    {
        return XPos[xPosition].GetBool(yPosition);
    }

    public void SpawnObstacle(int xPosition,int yPosition)
    {
        GameObject obs = Instantiate(obstacleDB.GetObstacle(UnityEngine.Random.Range(0,obstacleDB.GetTotalObstacles())));
        obs.transform.SetParent(transform);
        Vector3 posToSpawn = new Vector3(xPosition, 0, yPosition);
        obs.transform.position = posToSpawn;
    }
}
