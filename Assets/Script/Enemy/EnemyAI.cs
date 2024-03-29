
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IAi
{
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;

    [Header("DEBUG")]
    public bool enableDebugPath = false;
    List<Tile> debugPathList = new List<Tile>();


    [Header("MOVEMENT CONFIG")]
    [SerializeField] bool disableDiagonalMovement = false;
    [SerializeField] float moveSpeed = 5f;
    List<Tile> pathList = new List<Tile>();

    [Header("SFX")]
    [SerializeField] AudioClip footStepsSFX;
    bool isMoving = false;

    //REFERENCES
    GridManager gridManager;
    PathFinding player;
    AudioSource audioSource;
    int prevXPos = 0;
    int prevYPos = 0;
    bool movedFromStartTile = false;
    private void Awake()
    {
        gridManager=FindObjectOfType<GridManager>();
        player = FindObjectOfType<PathFinding>();
        audioSource=GetComponent<AudioSource>();
    }
    void Start()
    {
        MoveTowardsPlayer();   
    }
    private void OnEnable()
    {
        ResetPosition();
        MoveTowardsPlayer();
    }

    private void ResetPosition()
    {
        int randomX = Random.Range(0, 10);
        int randomY = Random.Range(0, 10);
        Tile spawnTile = gridManager.GetTile(randomX, randomY);
        while (!spawnTile.isWalkable)
        {
            randomX = Random.Range(0, 10);
            randomY = Random.Range(0, 10);
            spawnTile = gridManager.GetTile(randomX, randomY);
        }
        transform.position = spawnTile.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void MoveTowardsPlayer()
    {
        if (isMoving)
        {
            return;
        }
        Tile closestTile = GetClosestPlayerNeighbourTile();
        Tile currentTile = gridManager.GetTile((int)transform.position.x, (int)transform.position.z);
        if(!movedFromStartTile)
        {
            currentTile.isWalkable = true;
        }
        
        if (closestTile == currentTile)
        {
            SetTileWalkable(closestTile, false);
            movedFromStartTile = false;
            return;
        }
        pathList = FindPath(currentTile, closestTile);
        
    }

    private void SetTileWalkable(Tile tile,bool status)
    {
        tile.isWalkable = status;
    }
    IEnumerator CheckForPlayerMovement()
    {
        yield return new WaitForSeconds(1);
        MoveTowardsPlayer();
        StartCoroutine(CheckForPlayerMovement());
    }
    public void Move()
    {
        if (pathList != null && pathList.Count > 0)
        {
            isMoving = true;
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(footStepsSFX);
            }
            Tile targetTile = pathList.ElementAt(0);
            if (enableDebugPath)
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
                    targetTile = null;
                    isMoving = false;
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                    StartCoroutine(CheckForPlayerMovement());
                }
            }
        }
    }
    private Tile GetClosestPlayerNeighbourTile()
    {
        List<Tile> playerNeighbours;
        playerNeighbours= player.Get4Neighbours();
        float shortestDistance = Vector3.Distance(transform.position,playerNeighbours.ElementAt(0).transform.position);
        Tile closestTile = playerNeighbours.ElementAt(0);
        for(int  i = 0; i < playerNeighbours.Count; i++)
        {
            if(Vector3.Distance(transform.position, playerNeighbours.ElementAt(i).transform.position)<shortestDistance)
            {
                shortestDistance = Vector3.Distance(transform.position, playerNeighbours.ElementAt(i).transform.position);
                closestTile= playerNeighbours[i];
            }
        }
        return closestTile;
    }
    public int CalculateDistance(Tile a, Tile b)
    {
        int xDistance = Mathf.RoundToInt(Mathf.Abs(a.GetX() - b.GetX()));
        int yDistance = Mathf.RoundToInt(Mathf.Abs(a.GetY() - b.GetY()));

        int remaining = Mathf.RoundToInt(Mathf.Abs(xDistance - yDistance));

        return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * remaining;
    }

    public List<Tile> CalculatePath(Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        path.Add(endTile);
        Tile currentTile = endTile;
        while (currentTile.cameFromTile != null)
        {
            path.Add(currentTile.cameFromTile);
            currentTile = currentTile.cameFromTile;
        }
        path.Reverse();
        return path;
    }

    public List<Tile> FindPath(Tile start, Tile end)
    {
        Tile startTile = start;
        List<Tile>openList = new List<Tile> { startTile };
        List<Tile> closedList = new List<Tile>();

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Tile tile = gridManager.GetTile(x, y);
                tile.gCost = int.MaxValue;
                tile.CalculateFCost();
                tile.cameFromTile = null;
            }
        }

        startTile.gCost = 0;
        startTile.hCost = CalculateDistance(startTile, end);
        startTile.CalculateFCost();
        while (openList.Count > 0)
        {
            Tile currentTile = GetLowestFCostTile(openList);
            if (currentTile == end)
            {
                return CalculatePath(end);
            }
            openList.Remove(currentTile);
            closedList.Add(currentTile);
            foreach (Tile neighbourTile in GetNeighbours(currentTile))
            {
                if (closedList.Contains(neighbourTile))
                    continue;
                if (!neighbourTile.isWalkable)
                {
                    closedList.Add(neighbourTile);
                    continue;
                }
                int tentativeGCost = currentTile.gCost + CalculateDistance(currentTile, neighbourTile);
                if (tentativeGCost < neighbourTile.gCost)
                {
                    neighbourTile.cameFromTile = currentTile;
                    neighbourTile.gCost = tentativeGCost;
                    neighbourTile.hCost = CalculateDistance(neighbourTile, end);
                    neighbourTile.CalculateFCost();

                    if (!openList.Contains(neighbourTile))
                    {
                        openList.Add(neighbourTile);
                    }
                }
            }
        }

        //OUT OF NODES ON OPEN LIST
        return null;
    }

    public Tile GetLowestFCostTile(List<Tile> pathTiles)
    {
        Tile lowestCostTile = pathTiles[0];
        for (int i = 1; i < pathTiles.Count; i++)
        {
            if (pathTiles[i].fCost < lowestCostTile.fCost)
            {
                lowestCostTile = pathTiles[i];
            }
        }

        return lowestCostTile;
    }

    public List<Tile> GetNeighbours(Tile currentTile)
    {
        List<Tile> neighbourList = new List<Tile>();
        int currentX = Mathf.RoundToInt(currentTile.transform.position.x);
        int currentY = Mathf.RoundToInt(currentTile.transform.position.z);
        if (currentX - 1 >= 0)
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

        if (currentX + 1 < 10)
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
        if (currentY - 1 >= 0)
            neighbourList.Add(GetTile(currentX, currentY - 1));
        //UP
        if (currentY + 1 < 10)
            neighbourList.Add(GetTile(currentX, currentY + 1));

        return neighbourList;
    }

    public Tile GetTile(int x, int y)
    {
        return gridManager.GetTile(x, y);
    }

    private void DisableAllDebugPathUI()
    {
        if (debugPathList.Count <= 0)
            return;
        for (int i = 0; i < debugPathList.Count; i++)
        {
            debugPathList.ElementAt(i).UpdateTileUI(false);
            debugPathList.RemoveAt(i);
        }
    }

}
