using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class TileView : MonoBehaviour
{
    [SerializeField]
    private TileViewConfig VisualConfig;

    public TileCell Cell { get; private set; }
    private Renderer Renderer;    

    private void Awake()
    {
        Renderer = GetComponent<Renderer>();
    }

    public void Init(TileCell cell)
    {
        this.Cell = cell;
        UpdateView();
        cell.OnChanged += UpdateView;
    }

    public void UpdateView()
    {
        if(Cell.State != TileState.Default)
        {
            Renderer.material.color = VisualConfig.GetColor(Cell.State);
            return;
        }
        Renderer.material.color = VisualConfig.GetColor(Cell.Tile.Type);
    }

    private void OnDestroy()
    {
        Cell.OnChanged -= UpdateView;
    }
}
