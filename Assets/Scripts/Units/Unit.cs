using System;
using System.Collections.Generic;

public abstract class Unit
{
    public event Action<List<TileCell>> OnMove;
    public TileCell Tile { get; protected set; }
        
    public Unit(TileCell tile)
    {
        Tile = tile;
    }

    public void MoveTo(List<TileCell> path)
    {
        Tile = path[path.Count - 1];
        OnMove?.Invoke(path);
    }
}
