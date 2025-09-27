public class Player : Unit
{
    public int MoveRange { get; private set; }
    public int AttackRange { get; private set; }

    public Player(TileCell tile, int moveRange, int attackRange) : base(tile)
    {
        MoveRange = moveRange;
        AttackRange = attackRange;
    }
}
