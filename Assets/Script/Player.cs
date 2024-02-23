using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("DEBUG")]
    public bool enableDebugPath = false;
    List<Tile> debugPathList = new List<Tile>();

    [Header("MOVEMENT CONFIG")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;
    public bool isMoving = false;

    List<Tile> pathList = new List<Tile>();

    int prevXPos = 0;
    int prevYPos = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (pathList != null && pathList.Count > 0)
        {
            isMoving = true;
            Tile targetTile = pathList.ElementAt(0);
            if(enableDebugPath)
            {
                debugPathList.Add(targetTile);
                targetTile.UpdateTileUI(true);
            }
            
            transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, moveSpeed * Time.deltaTime);
            transform.LookAt(targetTile.transform.position);
            if (Vector3.Distance(transform.position, targetTile.transform.position) < 0.001f)
            {
                
                pathList.RemoveAt(0);
                if (pathList.Count == 0)
                {
                    DisableAllDebugPathUI();
                    targetTile = null;
                    isMoving = false;
                }

            }
        }
    }

    private void DisableAllDebugPathUI()
    {
        if (debugPathList.Count <= 0)
            return;
        for(int i=0;i<debugPathList.Count;i++)
        {
            debugPathList.ElementAt(i).UpdateTileUI(false);
            debugPathList.RemoveAt(i);
        }
    }

    public void HandleMovement(List<Tile> path)
    {
        pathList = path;
    }
}
