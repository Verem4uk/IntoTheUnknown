using System;

public class TileCell
{
    public int X { get; }
    public int Y { get; }
    public ITile Tile { get; private set; }

    public event Action OnChanged;

    public TileCell(int x, int y, ITile tile)
    {
        X = x;
        Y = y;

        Tile = tile;
    }

    public void UpdateTile(ITile tile)
    {
        Tile = tile;
        OnChanged?.Invoke();
    }
}


public enum TileType
{
    Traversable = 0, 
    Obstacle = 1,    
    Cover = 2        
}
