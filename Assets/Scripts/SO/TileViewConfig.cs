using UnityEngine;

[CreateAssetMenu(fileName = "TileViewConfig", menuName = "Configs/Tile View Config")]
public class TileViewConfig : ScriptableObject
{
    [System.Serializable]
    public struct TileTypeColor
    {
        public TileType type;
        public Color color;
    }

    [System.Serializable]
    public struct TileStateColor
    {
        public TileState state;
        public Color color;
    }

    public TileTypeColor[] tileTypeColors;
    public TileStateColor[] tileStateColors;

    public Color GetColor(TileType type)
    {
        foreach (var entry in tileTypeColors)
        {
            if (entry.type == type)
                return entry.color;
        }

        return Color.white; 
    }

    public Color GetColor(TileState state)
    {
        foreach (var entry in tileStateColors)
        {
            if (entry.state == state)
                return entry.color;
        }

        return Color.white;
    }
}
