using UnityEngine;

public class MapView : MonoBehaviour
{ 
    [SerializeField]
    private GameObject TilePrefab;

    [SerializeField]
    private Transform TilesTransform;

    private Map Map;

    public void Init(Map map)
    {
        Map = map;
        map.OnChanged += OnCreateMapClicked;
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

    private void OnDestroy()
    {
        Map.OnChanged -= OnCreateMapClicked;
    }
}
