using UnityEngine;

public abstract class UnitView : MonoBehaviour
{    
    public Unit Unit { private set; get; }
    public virtual void Init(Unit unit)
    {
        Unit = unit;
    }
}
