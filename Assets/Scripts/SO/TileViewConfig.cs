using UnityEngine;

[CreateAssetMenu(fileName = "TileViewConfig", menuName = "Configs/Tile View Config")]
public class TileViewConfig : ScriptableObject
{
    [System.Serializable]
    public struct TileColor
    {
        public TileType type;
        public Color color;
    }

    public TileColor[] tileColors;

    public Color GetColor(TileType type)
    {
        foreach (var entry in tileColors)
        {
            if (entry.type == type)
                return entry.color;
        }

        return Color.white; 
    }
}
