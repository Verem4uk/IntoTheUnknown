using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;

    [Header("Map References")]
    [SerializeField]
    private TMP_InputField InputWidth;
    [SerializeField]
    private TMP_InputField InputHeight;
    [SerializeField]
    private Button CreateMapButton;   
    [SerializeField]
    private MapView MapView;

    [Header("Unit References")]
    [SerializeField]
    private TMP_InputField InputMoveRange;
    [SerializeField]
    private TMP_InputField InputAttackRange;
    [SerializeField]
    private Button PlacePlayerButton;
    [SerializeField]
    private Button PlaceEnemyButton;

    private Map Map;    
    private EditMode Mode = EditMode.Tiles;

    private enum EditMode
    {
        Tiles,
        Player,
        Enemy
    }
    
    private void Start()
    {
        CreateMapButton.onClick.AddListener(OnCreateMapClicked);
        PlacePlayerButton.onClick.AddListener(OnPlacePlayerClicked);
        PlaceEnemyButton.onClick.AddListener(OnPlaceEnemyClicked);
    }

    private void OnCreateMapClicked()
    {
        int width = Mathf.Max(1, int.Parse(InputWidth.text));
        int height = Mathf.Max(1, int.Parse(InputHeight.text));

        Map = new Map(width, height);
        MapView.Init(Map);
    }

    private void OnPlacePlayerClicked() 
    {
        Mode = EditMode.Player;
        Map.RemovePlayer();
    }
    private void OnPlaceEnemyClicked()
    {        
        Mode = EditMode.Enemy;
        Map.RemoveEnemy();
    }

    private void OnPlayerPlaced(TileCell tile)
    {
        int moveRange = Mathf.Max(1, int.Parse(InputMoveRange.text));
        int attackRange = Mathf.Max(1, int.Parse(InputAttackRange.text));
        Map.PlacePlayer(tile, moveRange, attackRange);
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

                    if (Map.Player != null && Map.Player.Tile.Equals(cell))
                    {
                        Map.RemovePlayer();
                    }
                    if (Map.Enemy != null && Map.Enemy.Tile.Equals(cell))
                    {
                        Map.RemoveEnemy();
                    }

                    switch (Mode)
                    {
                        case EditMode.Tiles:                           
                            Map.NextType(cell);
                            break;
                        case EditMode.Player:
                            if(cell.Tile.Type != TileType.Traversable)
                            {
                                Map.ChangeType(cell, TileType.Traversable);
                            }
                            OnPlayerPlaced(cell);
                            Mode = EditMode.Tiles;
                            break;
                        case EditMode.Enemy:
                            if (cell.Tile.Type != TileType.Traversable)
                            {
                                Map.ChangeType(cell, TileType.Traversable);
                            }
                            Map.PlaceEnemy(cell);
                            Mode = EditMode.Tiles;
                            break;
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        CreateMapButton.onClick.RemoveAllListeners();
        PlacePlayerButton.onClick.RemoveAllListeners();
        PlaceEnemyButton.onClick.RemoveAllListeners();
    }
}
