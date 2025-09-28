using System;

public class TileCell
{
    public int X { get; }
    public int Y { get; }
    public ITile Tile { get; private set; }
    public TileState State { get; private set; }

    public event Action OnChanged;

    public TileCell(int x, int y, ITile tile)
    {
        X = x;
        Y = y;

        Tile = tile;
        State = TileState.Default;
    }

    public void UpdateTile(ITile tile)
    {
        Tile = tile;
        OnChanged?.Invoke();
    }

    public void Mark(TileState state)
    {
        State = state;
        OnChanged?.Invoke();
    }
}


public enum TileType
{
    Traversable,
    Obstacle,
    Cover
}

public enum TileState
{
    Default,
    MovePath,
    AttackPath,
    UnavailablePath
}
