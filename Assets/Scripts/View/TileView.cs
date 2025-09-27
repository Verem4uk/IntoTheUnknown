using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class TileView : MonoBehaviour
{
    public TileCell Cell { get; private set; }
    private Renderer Renderer;

    [SerializeField]
    private TileViewConfig visualConfig;

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
        Renderer.material.color = visualConfig.GetColor(Cell.Tile.Type);
    }

    private void OnDestroy()
    {
        Cell.OnChanged -= UpdateView;
    }
}
