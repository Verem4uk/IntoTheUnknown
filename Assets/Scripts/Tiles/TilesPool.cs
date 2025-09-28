using System.Collections.Generic;

public class TilePool
{    
    private readonly Dictionary<TileType, Queue<ITile>> Pool = new();

    public ITile Get(TileType type)
    {
        if (!Pool.ContainsKey(type))
        {
            Pool[type] = new Queue<ITile>();
        }
            
        if (Pool[type].Count > 0)
        {
            return Pool[type].Dequeue();
        }
                
        return type switch
        {
            TileType.Obstacle => new ObstacleTile(),
            TileType.Cover => new CoverTile(),
            _ => new TraversableTile(),
        };
    }

    public void Return(ITile tile)
    {
        if (!Pool.ContainsKey(tile.Type))
        {
            Pool[tile.Type] = new Queue<ITile>();
        }

        Pool[tile.Type].Enqueue(tile);
    }
}
