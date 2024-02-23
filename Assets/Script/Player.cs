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
    [SerializeField] float moveSpeed = 5f;
    [Header("MOVEMENT CONFIG")]
    [SerializeField] float rotationSpeed = 5f;
    List<Tile> pathList = new List<Tile>();
    public bool isMoving = false;

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
            transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, moveSpeed * Time.deltaTime);
            transform.LookAt(targetTile.transform.position);
            if (Vector3.Distance(transform.position, targetTile.transform.position) < 0.001f)
            {
                pathList.RemoveAt(0);
                if (pathList.Count == 0)
                {
                    targetTile = null;
                    isMoving = false;
                }

            }
        }
    }

    public void HandleMovement(List<Tile> path)
    {
        pathList = path;
    }
}
