using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : UnitView
{
    [SerializeField]
    public float MoveSpeed = 3f;

    public event Action OnMoveCompleted;

    private Animator Animator;
    private Queue<Vector3> Path = new Queue<Vector3>();
    private bool IsMoving = false;
    private Player Player;

    private void Start()
    {
        Animator = GetComponent<Animator>();        
    }

    public override void Init(Unit unit)
    {
        base.Init(unit);
        
        if (unit is Player player)
        {
            player.OnMove += OnMove;
            Player = player; 
        }
    }

    private void OnMove(List<TileCell> pathCells)
    {
        Path.Clear();
        foreach (var cell in pathCells)
        {
            Vector3 worldPos = new Vector3(cell.X, 0, cell.Y);
            Path.Enqueue(worldPos);
        }

        if(Path.Count == 0)
        {
            Player.CallBackMovementComplete();
            return;
        }

        if (!IsMoving)
        {
            StartCoroutine(MoveAlongPath());
        }            
    }

    private IEnumerator MoveAlongPath()
    {
        IsMoving = true;
        Animator.SetFloat("Speed", MoveSpeed); 

        while (Path.Count > 0)
        {
            Vector3 target = Path.Dequeue();

            while (Vector3.Distance(transform.position, target) > 0.05f)
            {
                Vector3 dir = (target - transform.position).normalized;
                dir.y = 0;

                transform.position += dir * MoveSpeed * Time.deltaTime;
                transform.forward = dir; 
                yield return null;
            }

            transform.position = target; 
            yield return null;
        }

        Animator.SetFloat("Speed", 0f); 
        IsMoving = false;
        Player.CallBackMovementComplete();
    }

    public void OnFootstep()
    {
        //for events from animator (errors without it)
    }

    private void OnDestroy()
    {
        Player.OnMove -= OnMove;
    }
}
