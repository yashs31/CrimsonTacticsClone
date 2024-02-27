using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("GRID OBJECT")]
    [SerializeField] GameObject defaultTile;
    [SerializeField] GameObject wall;
    int dimension = 10;
    Tile[,] tilesGrid;

    RaycastHit hit;
    List<Tile> pathList;
    int prevXPos=-1, prevYPos=-1;
    Tile previousTile, currentTile,startTile;

    ObstacleManagerWithGrid obstacleManager;
    PathFinding pathFinding;
    Player player;
    private void Awake()
    {
        pathList = new List<Tile>();
        obstacleManager=FindObjectOfType<ObstacleManagerWithGrid>();
        pathFinding=FindObjectOfType<PathFinding>();
        tilesGrid=new Tile[dimension,dimension];
        player = FindObjectOfType<Player>();
        GenerateGrid(dimension);
        startTile = tilesGrid[0, 0];
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            startTile= tilesGrid[Mathf.RoundToInt(pathFinding.transform.position.x), Mathf.RoundToInt(pathFinding.transform.position.z)];
            int xPos=Mathf.RoundToInt(hit.transform.position.x);
            int yPos = Mathf.RoundToInt(hit.transform.position.z);
            if (xPos >= dimension || xPos < 0 || yPos >= dimension || yPos < 0)
                return;
            if (CheckIfPositionUpdated(xPos, yPos,previousTile))
            {
                prevXPos = xPos;
                prevYPos = yPos;
                currentTile=hit.collider.GetComponent<Tile>();
                previousTile = currentTile;
                if(currentTile != null)
                {
                    currentTile.UpdateTileUI(true);
                }
            }
            if(pathList!=null&& Input.GetMouseButton(0) && !player.isMoving)
            {
                if (currentTile != null && currentTile.isWalkable)
                {
                    pathList = pathFinding.FindPath(startTile, currentTile);
                    player.HandleMovement(pathList);
                }
            }
        }
    }

    bool CheckIfPositionUpdated(int currXPos,int currYPos, Tile prevTile)
    {

        if(currXPos!=prevXPos || currYPos != prevYPos)
        {
            if(previousTile!=null)
            {
                previousTile.UpdateTileUI(false);
            }
            return true;
        }
        return false;
    }
    private void GenerateGrid(int size)
    {
        for(int y=0; y < size; y++)
        {
            for(int x=0;x<size;x++)
            {
                GameObject go= Instantiate(defaultTile,transform);
                go.transform.position = new Vector3(x, 0, y);
                tilesGrid[x, y] = go.GetComponent<Tile>();
                if(obstacleManager.IsObstaclePresent(x,y))
                {
                    tilesGrid[x, y].isWalkable = false;
                    //Debug.Log(x + "," + y + "is not walkable");
                    obstacleManager.SpawnObstacle(x, y);
                }
            }
        }

        //SPAWN WATER
        for(int i=0;i<size;i++)
        {
            GameObject go1 = Instantiate(wall, transform);
            go1.transform.position = new Vector3(i, 0, 10);

            GameObject go2 = Instantiate(wall, transform);
            go2.transform.position = new Vector3(i, 0, -1);

            GameObject go3 = Instantiate(wall, transform);
            go3.transform.position = new Vector3(-1, 0, i);

            GameObject go4 = Instantiate(wall, transform);
            go4.transform.position = new Vector3(10, 0, i);
        }
    }

    public Tile GetTile(int x,int y)
    {
        return tilesGrid[x, y];
    }
}
