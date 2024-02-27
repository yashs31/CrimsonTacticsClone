using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstacleManagerWithGrid : MonoBehaviour
{
    [SerializeField] ObstacleDB obstacleDB;
    public ObstacleGridLayout grid;
    private void Start()
    {

    }
    private void Update()
    {
        
    }

    public bool GetData(int x,int y)
    {
        return grid.X[y].Y[x];
    }

    public bool IsObstaclePresent(int xPosition, int yPosition)
    {
        //return XPos[xPosition].GetBool(yPosition);
        return grid.X[xPosition].Y[yPosition];
    }

    private void Awake()
    {
        //bits = new BitArray(initialBools);
    }
    public void SpawnObstacle(int xPosition, int yPosition)
    {
        GameObject obs = Instantiate(obstacleDB.GetObstacle(UnityEngine.Random.Range(0, obstacleDB.GetTotalObstacles())));
        obs.transform.SetParent(transform);
        Vector3 posToSpawn = new Vector3(xPosition, 0, yPosition);
        obs.transform.position = posToSpawn;
    }
}
