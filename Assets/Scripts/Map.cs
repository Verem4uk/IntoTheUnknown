using System;

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
