using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : UnitView
{
    [SerializeField]
    public float moveSpeed = 3f;

    private Animator animator;

    private Queue<Vector3> path = new Queue<Vector3>();
    private bool isMoving = false;

    private void Start()
    {
        animator = GetComponent<Animator>();        
    }

    public override void Init(Unit unit)
    {
        base.Init(unit);
        Unit.OnMove += OnMove;        
    }

    private void OnMove(List<TileCell> pathCells)
    {
        path.Clear();
        foreach (var cell in pathCells)
        {
            Vector3 worldPos = new Vector3(cell.X, 0, cell.Y);
            path.Enqueue(worldPos);
        }

        if (!isMoving && path.Count > 0)
            StartCoroutine(MoveAlongPath());
    }

    private IEnumerator MoveAlongPath()
    {
        isMoving = true;
        animator.SetFloat("Speed", moveSpeed); 

        while (path.Count > 0)
        {
            Vector3 target = path.Dequeue();

            while (Vector3.Distance(transform.position, target) > 0.05f)
            {
                Vector3 dir = (target - transform.position).normalized;
                dir.y = 0;

                transform.position += dir * moveSpeed * Time.deltaTime;
                transform.forward = dir; 
                yield return null;
            }

            transform.position = target; 
            yield return null;
        }

        animator.SetFloat("Speed", 0f); 
        isMoving = false;
    }

    public void OnFootstep()
    {
        //for events from animator (errors without it)
    }

    private void OnDestroy()
    {
        Unit.OnMove -= OnMove;
    }
}
