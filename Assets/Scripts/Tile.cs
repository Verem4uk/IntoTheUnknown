public class Tile
{   public int X { get; private set; }
    public int Y { get; private set; }
    public TileType Type { get; set; }

    public Tile(int x, int y, TileType type = TileType.Traversable)
    {
        X = x;
        Y = y;
        Type = type;
    }
}

public enum TileType
{
    Traversable = 0, 
    Obstacle = 1,    
    Cover = 2        
}
