using UnityEngine;

public interface ITile
{
    TileType Type { get; }
    TileType NextType();
}
