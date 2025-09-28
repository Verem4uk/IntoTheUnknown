public class Player : Unit
{
    public int MoveRange { get; private set; }
    public int AttackRange { get; private set; }

    public Player(TileCell tile, int moveRange, int attackRange) : base(tile)
    {
        UpdateParametres(moveRange, attackRange);
    }

    public void UpdateParametres(int moveRange, int attackRange)
    {
        MoveRange = moveRange;
        AttackRange = attackRange;
    }
}
