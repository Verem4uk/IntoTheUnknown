public class ObstacleTile : ITile
{
    public TileType Type => TileType.Obstacle;

    public TileType NextType() => TileType.Cover;    
}