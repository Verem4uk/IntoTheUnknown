using System;
using System.Collections.Generic;

public class Player : Unit
{
    public int MoveRange { get; private set; }
    public int AttackRange { get; private set; }

    public event Action<List<TileCell>> OnMove;

    public event Action OnMoveCompleted;

    public Player(TileCell tile, int moveRange, int attackRange) : base(tile)
    {
        UpdateParametres(moveRange, attackRange);
    }

    public void CallBackMovementComplete()
    {
        OnMoveCompleted?.Invoke();
    }

    public void MoveTo(List<TileCell> path)
    {
        if(path.Count > 0)
        {
            Tile = path[path.Count - 1];            
        }
        OnMove?.Invoke(path);
    }

    public void UpdateParametres(int moveRange, int attackRange)
    {
        MoveRange = moveRange;
        AttackRange = attackRange;
    }
}
