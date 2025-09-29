using System;
using System.Collections.Generic;
using System.Linq;

public class Map
{
    public TileCell[,] Cells { get; }
    private readonly TilePool Pool;

    public int SizeX { get; private set; }
    public int SizeY { get; private set; }
    
    public Player Player { get; private set; }
    public Enemy Enemy { get; private set; }


    public event Action<int, int> OnChanged;
    public event Action<Player> OnPlayerPlaced;
    public event Action<Enemy> OnEnemyPlaced;
    public event Action<Unit> OnUnitRemoved;

    public Map(int sizeX, int sizeY)
    {
        SizeX = sizeX;
        SizeY = sizeY;
        Pool = new TilePool();

        Cells = new TileCell[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                ITile tile = Pool.Get(TileType.Traversable);
                Cells[x, y] = new TileCell(x, y, tile);
            }
        }

        OnChanged?.Invoke(sizeX ,sizeY);
    }

    public bool TryToFindMovePath(TileCell cell, out List<TileCell> path)
    {        
        path = new Pathfinder(this).FindPath(Player.Tile, cell, forAttack: false);
        if (path == null)
        {
            return false;
        }

        CleanPath();

        if (path.Count - 1 <= Player.MoveRange)
        {
            for (int i = 1; i < path.Count; i++)
            {
                path[i].Mark(TileState.MovePath);
            }                
        }
        else
        {
            for (int i = 1; i < path.Count; i++)
            {
                path[i].Mark(TileState.UnavailablePath);
            }                
        }

        return true;
    }

    public bool TryToFindAttackPath(TileCell targetCell, out List<TileCell> path)
    {
        path = new List<TileCell>();

        // 1) Try to attack from the current position
        var directPath = new Pathfinder(this).FindPath(Player.Tile, targetCell, forAttack: true);
        if (directPath != null && directPath.Count - 1 <= Player.AttackRange)
        {
            CleanPath();
            for (int i = 1; i < directPath.Count; i++)
            {
                directPath[i].Mark(TileState.AttackPath);
            }             
                        
            return true;
        }

        // 2) Collect all the tiles around enemy in player's attack range
        List<TileCell> candidates = new List<TileCell>();
        foreach (var cell in GetCellsInRange(targetCell, Player.AttackRange))
        {
            if (cell.Tile.Type == TileType.Traversable && cell != targetCell)
            {
                candidates.Add(cell);
            }                
        }

        if (!candidates.Any())
        {
            return false;
        }            

        // 3) Check candidates if it's possible to attack from them
        List<TileCell> validCandidates = new List<TileCell>();

        foreach (var c in candidates)
        {
            var attackPath = new Pathfinder(this).FindPath(c, targetCell, forAttack: true);

            if (attackPath != null)
            {
                int distance = attackPath.Count - 1;
                if (distance <= Player.AttackRange)
                {
                    validCandidates.Add(c);
                }
            }
        }

        candidates = validCandidates;

        if (candidates.Count == 0)
        {
            return false;
        }            

        // 4) Check conditates if it's reachable for player
        var reachableCandidates = new List<(TileCell cell, List<TileCell> path)>();
        foreach (var candidate in candidates)
        {
            var playerPath = new Pathfinder(this).FindPath(Player.Tile, candidate, forAttack: false);
            if (playerPath != null)
            {
                reachableCandidates.Add((candidate, playerPath));
            }                
        }

        if (!reachableCandidates.Any())
        {
            return false;
        }            

        // 5) Choose the best one (the shortest way)
        var best = reachableCandidates.OrderBy(c => c.path.Count).First();
        var bestPath = best.path;

        // 6) If enemy is reacheble for attack
        if (bestPath.Count - 1 <= Player.MoveRange)
        {
            CleanPath();
                        
            for (int i = 1; i < bestPath.Count; i++)
            {
                bestPath[i].Mark(TileState.MovePath);
            }                
                        
            var attackPath = new Pathfinder(this).FindPath(best.cell, targetCell, forAttack: true);
            for (int i = 1; i < attackPath.Count; i++)
            {
                attackPath[i].Mark(TileState.AttackPath);
            }                

            path = bestPath;            
            return true;
        }
        else
        {
            // If enemy is reacheble but out of player moveRange
            CleanPath();

            for (int i = 1; i < bestPath.Count; i++)
                bestPath[i].Mark(TileState.UnavailablePath);

            var attackPath = new Pathfinder(this).FindPath(best.cell, targetCell, forAttack: true);
            if (attackPath != null)
            {
                for (int i = 1; i < attackPath.Count; i++)
                {
                    attackPath[i].Mark(TileState.UnavailableAttack);
                }                    
            }

            path = bestPath;
            return false;
        }
    }

    private IEnumerable<TileCell> GetCellsInRange(TileCell center, int range)
    {
        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = -range; dy <= range; dy++)
            {
                if (Math.Abs(dx) + Math.Abs(dy) <= range)
                {
                    int nx = center.X + dx;
                    int ny = center.Y + dy;
                                        
                    if (nx >= 0 && nx < Cells.GetLength(0) &&
                        ny >= 0 && ny < Cells.GetLength(1))
                    {
                        var cell = Cells[nx, ny];
                        if (cell != null)
                        {
                            yield return cell;
                        }                            
                    }
                }
            }
        }
    }

    public void CleanPath()
    {
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                var tile = Cells[x, y];
                tile.Mark(TileState.Default);                
            }
        }
    }

    public IEnumerable<TileCell> GetNeighbors(TileCell cell)
    {
        int x = cell.X;
        int y = cell.Y;

        if (x > 0)
        {
            yield return Cells[x - 1, y]; // West
        }
        if (x < SizeX - 1)
        {
            yield return Cells[x + 1, y]; // East
        }
        if (y > 0)
        {
            yield return Cells[x, y - 1]; // South
        }
        if (y < SizeY - 1)
        {
            yield return Cells[x, y + 1]; // North
        }
    }


    public void ChangeType(TileCell cell, TileType newType)
    {
        Pool.Return(cell.Tile);

        var newLogic = Pool.Get(newType);
        cell.UpdateTile(newLogic);
    }

    public void NextType(TileCell cell)
    {        
        var nextType = cell.Tile.NextType();
        ChangeType(cell, nextType);
    }

    public void PlacePlayer(TileCell cell, int moveRange, int attackRange)
    {
        Player = new Player(cell, moveRange, attackRange);
        OnPlayerPlaced?.Invoke(Player);
    }

    public void PlaceEnemy(TileCell cell)
    {
        Enemy = new Enemy(cell);
        OnEnemyPlaced?.Invoke(Enemy);
    }

    public void RemovePlayer()
    {
        if(Player != null)
        {
            OnUnitRemoved?.Invoke(Player);
            Player = null;
        }
    }

    public void RemoveEnemy()
    {
        if(Enemy != null)
        {
            OnUnitRemoved?.Invoke(Enemy);
            Enemy = null;
        }
    }
}
