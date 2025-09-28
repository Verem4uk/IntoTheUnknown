using System;
using System.Collections.Generic;

public class Map
{
    public TileCell[,] Cells { get; }
    private readonly TilePool Pool;

    public int SizeX { get; private set; }
    public int SizeY { get; private set; }

    public event Action<int, int> OnChanged;

    public event Action<Player> OnPlayerPlaced;
    public event Action<Enemy> OnEnemyPlaced;

    public event Action<Unit> OnUnitRemoved;
    
    public Player Player { get; private set; }
    public Enemy Enemy { get; private set; } 

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

    public void CheckCell(TileCell cell)
    {
        if (cell.Tile.Type == TileType.Obstacle || cell == Player.Tile)
            return;

        List<TileCell> path;

        if (cell == Enemy.Tile)
        {            
            path = new Pathfinder(this).FindPath(Player.Tile, cell, forAttack: true);
            if (path == null) return;
            CleanPath();

            if (path.Count - 1 <= Player.AttackRange)
            {
                for (int i = 1; i < path.Count; i++)
                    path[i].Mark(TileState.AttackPath);
            }
            else 
            {
                for (int i = 1; i < path.Count; i++)
                    path[i].Mark(TileState.UnavailablePath);
            }
        }
        else
        {            
            path = new Pathfinder(this).FindPath(Player.Tile, cell, forAttack: false);
            if (path == null) return;
            CleanPath();

            if (path.Count - 1 <= Player.MoveRange)
            {
                for (int i = 1; i < path.Count; i++)
                    path[i].Mark(TileState.MovePath);
            }
            else
            {
                for (int i = 1; i < path.Count; i++)
                    path[i].Mark(TileState.UnavailablePath);
            }
        }
    }


    private void CleanPath()
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

        if (x > 0) yield return Cells[x - 1, y];         // West
        if (x < SizeX - 1) yield return Cells[x + 1, y]; // East
        if (y > 0) yield return Cells[x, y - 1];         // South
        if (y < SizeY - 1) yield return Cells[x, y + 1]; // North
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
        if (Enemy != null)
        {
            OnUnitRemoved?.Invoke(Enemy);
            Enemy = null;
        }
    }
}
