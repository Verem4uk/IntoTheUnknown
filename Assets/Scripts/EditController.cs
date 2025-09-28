using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EditController : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;

    [SerializeField]
    private Button PlayButton;

    [SerializeField]
    private Button EditButton;

    [SerializeField]
    private PlayController PlayController;

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
    private Button ApplyForExisted;
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
        Enemy,
        Disabled
    }
    
    private void Start()
    {
        CreateMapButton.onClick.AddListener(OnCreateMapClicked);
        PlacePlayerButton.onClick.AddListener(OnPlacePlayerClicked);
        PlaceEnemyButton.onClick.AddListener(OnPlaceEnemyClicked);
        PlayButton.onClick.AddListener(OnGameStarted);
        EditButton.onClick.AddListener(OnEditStarted);
        ApplyForExisted.onClick.AddListener(OnApplyNewParametres);
    }

    private void OnCreateMapClicked()
    {
        int width = Mathf.Max(1, int.Parse(InputWidth.text));
        int height = Mathf.Max(1, int.Parse(InputHeight.text));

        Map = new Map(width, height);
        MapView.Init(Map);
        Mode = EditMode.Tiles;
        PlayController.enabled = false;
    }

    private void OnApplyNewParametres()
    {
        if(Map != null && Map.Player != null)
        {
            int moveRange = Mathf.Max(1, int.Parse(InputMoveRange.text));
            int attackRange = Mathf.Max(1, int.Parse(InputAttackRange.text));
            Map.Player.UpdateParametres(moveRange, attackRange);
        }
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

    private void OnGameStarted()
    {
        if(Map != null && Map.Player != null && Map.Enemy != null)
        {
            Mode = EditMode.Disabled;
            PlayController.Init(Map);
            PlayController.enabled = true;
        }
    }

    private void OnEditStarted()
    {
        Mode = EditMode.Tiles;
        PlayController.enabled = false;
        Map.CleanPath();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) 
        {
            var ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (Mode != EditMode.Disabled)
                {
                    var view = hit.collider.GetComponent<TileView>();
                    if (view != null)
                    {
                        var cell = view.Cell;
                        Operate(cell);
                        return;
                    }
                }               

                var enemy = hit.collider.GetComponent<EnemyView>();
                if (enemy != null && enemy.Unit.Tile.State == TileState.AttackPath)
                {
                    Map.RemoveEnemy();
                    OnEditStarted();
                }
            }
        }
    }

    private void Operate(TileCell cell)
    {
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
                if (cell.Tile.Type != TileType.Traversable)
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

    private void OnDestroy()
    {
        CreateMapButton.onClick.RemoveAllListeners();
        PlacePlayerButton.onClick.RemoveAllListeners();
        PlaceEnemyButton.onClick.RemoveAllListeners();
        PlayButton.onClick.RemoveAllListeners();
        ApplyForExisted.onClick.RemoveAllListeners();
    }
}
