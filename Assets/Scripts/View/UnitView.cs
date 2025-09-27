using UnityEngine;

public class UnitView : MonoBehaviour
{
    public Unit Unit { private set; get; }
    public void Init(Unit unit)
    {
        Unit = unit;
    }
}
