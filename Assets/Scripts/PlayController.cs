using UnityEngine;
using UnityEngine.InputSystem;

public class PlayController : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;

    private Map Map;
    public void Init(Map map)
    {
        Map = map;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var view = hit.collider.GetComponent<TileView>();
                if (view != null)
                {
                    var cell = view.Cell;
                    if (cell.Tile.Type == TileType.Obstacle || cell == Map.Player.Tile || cell.Tile.Type == TileType.Cover)
                        return;
                    if(cell == Map.Enemy.Tile && Map.TryToFindAttackPath(cell, out var attackPath))
                    {
                        Map.Player.MoveTo(attackPath);
                        return;
                    }

                    if(Map.TryToFindMovePath(cell, out var movePath) && movePath[1].State == TileState.MovePath)
                    {
                        Map.Player.MoveTo(movePath);
                    }
                    return;
                }                
            }
        }
    }
}
