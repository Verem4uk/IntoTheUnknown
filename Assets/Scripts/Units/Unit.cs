using System;

public abstract class Unit
{    
    public TileCell Tile { get; protected set; }
        
    public Unit(TileCell tile)
    {
        Tile = tile;
    }
}
