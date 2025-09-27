using System;

public class Map
{
    public TileCell[,] Cells { get; }
    private readonly TilePool Pool;

    public int SizeX { get; private set; }
    public int SizeY { get; private set; }

    public event Action<int, int> OnChanged;

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

    public void ChangeType(int x, int y, TileType newType)
    {
        var cell = Cells[x, y];

        Pool.Return(cell.Tile);

        var newLogic = Pool.Get(newType);
        cell.UpdateTile(newLogic);
    }

    public void NextType(int x, int y)
    {
        var cell = Cells[x, y];
        var nextType = cell.Tile.NextType();

        ChangeType(x, y, nextType);
    }
}
