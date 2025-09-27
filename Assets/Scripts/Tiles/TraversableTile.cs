public class TraversableTile : ITile
{
    public TileType Type => TileType.Traversable;
    public TileType NextType() => TileType.Obstacle;    
}