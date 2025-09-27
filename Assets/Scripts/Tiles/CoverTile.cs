public class CoverTile : ITile
{
    public TileType Type => TileType.Cover;

    public TileType NextType() => TileType.Traversable;    
}