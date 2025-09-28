using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private readonly Map Map;

    public Pathfinder(Map map)
    {
        Map = map;
    }

    public List<TileCell> FindPath(TileCell start, TileCell goal, bool forAttack = false)
    {
        var openSet = new PriorityQueue<TileCell>();
        var cameFrom = new Dictionary<TileCell, TileCell>();
        var gScore = new Dictionary<TileCell, int>();

        openSet.Enqueue(start, 0);
        gScore[start] = 0;

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }               

            foreach (var neighbor in Map.GetNeighbors(current))
            {                
                if (neighbor.Tile.Type == TileType.Obstacle)
                {
                    continue;
                }                    
                if (!forAttack && neighbor.Tile.Type == TileType.Cover)
                {
                    continue;
                }                    
                if (!forAttack && Map.Enemy.Tile.Equals(neighbor))
                {
                    continue;
                }                    

                int tentativeG = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    int fScore = tentativeG + Heuristic(neighbor, goal);
                    openSet.Enqueue(neighbor, fScore);
                }
            }
        }

        return null; 
    }


    private int Heuristic(TileCell a, TileCell b)
    {
        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);
    }

    private List<TileCell> ReconstructPath(Dictionary<TileCell, TileCell> cameFrom, TileCell current)
    {
        var path = new List<TileCell> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }
}
