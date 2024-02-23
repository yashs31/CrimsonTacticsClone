using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PathFinding:MonoBehaviour
{
    
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;
    // Start is called before the first frame update
    GridManager gridManager;
    private List<Tile> openList;
    private List<Tile> closedList;
    List<Tile> pathList= new List<Tile>();
    [SerializeField] bool disableDiagonalMovement = false;
    private void Awake()
    {
        gridManager=FindObjectOfType<GridManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Tile> FindPath(Tile start, Tile endTile)
    {
        Tile startTile = start;
        openList = new List<Tile> { startTile };
        closedList = new List<Tile>();

        for(int y=0;y<10;y++)
        {
            for(int x=0;x<10;x++)
            {
                Tile tile = gridManager.GetTile(x, y);
                tile.gCost = int.MaxValue;
                tile.CalculateFCost();
                tile.cameFromTile = null;
            }
        }

        startTile.gCost = 0;
        startTile.hCost = CalculateDistance(startTile, endTile);
        startTile.CalculateFCost();

        while(openList.Count>0)
        {
            Tile currentTile = GetLowestFCostTile(openList);
            if (currentTile==endTile)
            {
                return CalculatePath(endTile);
            }
            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach(Tile neighbourTile in GetNeighbours(currentTile))
            {
                if (closedList.Contains(neighbourTile))
                    continue;
                if(!neighbourTile.isWalkable)
                {
                    closedList.Add(neighbourTile);
                    continue;
                }
                int tentativeGCost=currentTile.gCost+CalculateDistance(currentTile,neighbourTile);
                if(tentativeGCost<neighbourTile.gCost)
                {
                    neighbourTile.cameFromTile = currentTile;
                    neighbourTile.gCost = tentativeGCost;
                    neighbourTile.hCost = CalculateDistance(neighbourTile, endTile);
                    neighbourTile.CalculateFCost();

                    if(!openList.Contains(neighbourTile))
                    {
                        openList.Add(neighbourTile);
                    }
                }
            }
        }

        //OUT OF NODES ON OPEN LIST
        return null;
    }

    private List<Tile> GetNeighbours(Tile currentTile)
    {
        List<Tile> neighbourList=new List<Tile>();
        int currentX = currentTile.GetX();
        int currentY = currentTile.GetY();
        if (currentX-1>=0)
        {
            //LEFT
            neighbourList.Add(GetTile(currentX - 1, currentY));
            if (!disableDiagonalMovement)
            {
                //LEFT DOWN
                if (currentY - 1 >= 0)
                    neighbourList.Add(GetTile(currentX - 1, currentY - 1));
                //LEFT UP
                if (currentY + 1 < 10)
                    neighbourList.Add(GetTile(currentX - 1, currentY + 1));
            }   
        }

        if(currentX+1<10)
        {
            //RIGHT
            neighbourList.Add(GetTile(currentX + 1, currentY));
            if (!disableDiagonalMovement)
            {
                //RIGHT DOWN
                if (currentY - 1 >= 0)
                    neighbourList.Add(GetTile(currentX + 1, currentY - 1));
                //RIGHT UP
                if (currentY + 1 < 10)
                    neighbourList.Add(GetTile(currentX + 1, currentY + 1));
            }

        }

        //DOWN
        if(currentY-1>=0)
            neighbourList.Add(GetTile(currentX,currentY-1));
        //UP
        if (currentY + 1 <10)
            neighbourList.Add(GetTile(currentX, currentY + 1));

        return neighbourList;
    }

    public List<Tile> Get4Neighbours()
    {
        List<Tile> neighbourList = new List<Tile>();
        int currentX = Mathf.RoundToInt(transform.position.x);
        int currentY = Mathf.RoundToInt(transform.position.z);
        if (currentX - 1 >= 0)
        {
            //LEFT
            neighbourList.Add(GetTile(currentX - 1, currentY));
        }

        if (currentX + 1 < 10)
        {
            //RIGHT
            neighbourList.Add(GetTile(currentX + 1, currentY));
        }

        //DOWN
        if (currentY - 1 >= 0)
            neighbourList.Add(GetTile(currentX, currentY - 1));
        //UP
        if (currentY + 1 < 10)
            neighbourList.Add(GetTile(currentX, currentY + 1));

        return neighbourList;
    }
    private Tile GetTile(int x,int y)
    {
        return gridManager.GetTile(x, y);
    }
    private List<Tile> CalculatePath(Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        path.Add(endTile);
        Tile currentTile = endTile;
        while(currentTile.cameFromTile!=null)
        {
            path.Add(currentTile.cameFromTile);
            currentTile=currentTile.cameFromTile;
        }
        path.Reverse();
        return path;
    }
    private int CalculateDistance(Tile a,Tile b)
    {
        int xDistance = Mathf.Abs(a.GetX() - b.GetX());
        int yDistance = Mathf.Abs(a.GetY() - b.GetY());

        int remaining = Mathf.RoundToInt(Mathf.Abs(xDistance - yDistance));

        return DIAGONAL_COST*Mathf.Min(xDistance,yDistance)+STRAIGHT_COST*remaining;
    }

    private Tile GetLowestFCostTile(List<Tile>pathTiles)
    {
        Tile lowestCostTile = pathTiles[0];
        for(int i=1; i<pathTiles.Count; i++)
        {
            if (pathTiles[i].fCost<lowestCostTile.fCost)
            {
                lowestCostTile = pathTiles[i];
            }
        }

        return lowestCostTile;
    }
}
