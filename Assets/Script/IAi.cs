using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAi
{
    List<Tile> FindPath(Tile sstart,Tile end);
    List<Tile> GetNeighbours(Tile currentTile);
    Tile GetTile(int x, int y);
    List<Tile> CalculatePath(Tile endTile);
    int CalculateDistance(Tile a, Tile b);
    Tile GetLowestFCostTile(List<Tile> pathTiles);
}
