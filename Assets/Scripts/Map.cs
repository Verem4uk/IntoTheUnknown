using UnityEngine;

public class Map
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Tile[,] Tiles { get; private set; }

    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        Tiles = new Tile[width, height];
                
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tiles[x, y] = new Tile(x, y, TileType.Traversable);
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
        {
            Debug.LogError("Out of map range");
            return null;
        }            
        return Tiles[x, y];
    }
}
