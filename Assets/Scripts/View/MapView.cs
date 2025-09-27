using UnityEngine;

public class MapView : MonoBehaviour
{ 
    [SerializeField]
    private GameObject TilePrefab;

    [SerializeField]
    private GameObject PlayerPrefab;

    [SerializeField]
    private GameObject EnemyPrefab;

    [SerializeField]
    private Transform TilesTransform;

    private Map Map;

    public void Init(Map map)
    {
        Map = map;
        map.OnChanged += OnCreateMapClicked;

        map.OnPlayerPlaced += OnPlayerPlaced;
        map.OnEnemyPlaced += OnEnemyPlaced;
        map.OnUnitRemoved += OnDestroyUnit;        

        OnCreateMapClicked(map.SizeX, map.SizeY);
    }        

    private void OnCreateMapClicked(int width, int height)
    {
        foreach (Transform child in TilesTransform)
        {
            Destroy(child.gameObject);
        }
                          
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileView tileObj = Instantiate(TilePrefab, TilesTransform).GetComponent<TileView>();
                tileObj.transform.position = new Vector3(x, 0, y);
                var cell = Map.Cells[x, y];
                tileObj.Init(cell);
            }
        }
    }

    private void OnPlayerPlaced(Player player)
    {
        PlaceUnit(PlayerPrefab, player);
    }

    private void OnEnemyPlaced(Enemy enemy)
    {
        PlaceUnit(EnemyPrefab, enemy);
    }

    private void PlaceUnit(GameObject prefab, Unit unit)
    {
        var tile = unit.Tile;
        GameObject playerObj = Instantiate(prefab, new Vector3(tile.X, 2, tile.Y), new Quaternion(), TilesTransform);
        playerObj.GetComponent<UnitView>().Init(unit);   

    }

    private void OnDestroyUnit(Unit unit)
    {
        foreach (Transform child in TilesTransform)
        {
            var unitView = child.GetComponent<UnitView>();
            if(unitView == null)
            {
                continue;
            }
            if (unitView.Unit.Equals(unit))
            {
                Destroy(unitView.gameObject);
            }            
        }
    }

    private void OnDestroy()
    {
        Map.OnChanged -= OnCreateMapClicked;
    }
}
