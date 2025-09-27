using System;

public abstract class Unit
{
    public event Action<TileCell> OnMove;
    public TileCell Tile { get; protected set; }
        
    public Unit(TileCell tile)
    {
        Tile = tile;
    }

    public void MoveTo(TileCell newTile)
    {
        Tile = newTile;
        OnMove?.Invoke(newTile);
    }
}
