using System.Collections.Generic;

public class TilePool
{    
    private readonly Dictionary<TileType, Queue<ITile>> pool = new();

    public ITile Get(TileType type)
    {
        if (!pool.ContainsKey(type))
            pool[type] = new Queue<ITile>();

        if (pool[type].Count > 0)
        {
            return pool[type].Dequeue();
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
        if (!pool.ContainsKey(tile.Type))
        {
            pool[tile.Type] = new Queue<ITile>();
        }

        pool[tile.Type].Enqueue(tile);
    }
}
