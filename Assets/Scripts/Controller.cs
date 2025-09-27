using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private TMP_InputField InputWidth;
    [SerializeField]
    private TMP_InputField InputHeight;
    [SerializeField]
    private Button CreateButton;
    [SerializeField]
    private Camera MainCamera;
    [SerializeField]
    private MapView MapView;

    private Map Map;
    
    private void Start()
    {
        CreateButton.onClick.AddListener(OnCreateMapClicked);
    }

    private void OnCreateMapClicked()
    {
        int width = Mathf.Max(1, int.Parse(InputWidth.text));
        int height = Mathf.Max(1, int.Parse(InputHeight.text));

        Map = new Map(width, height);
        MapView.Init(Map);
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
                    Map.NextType(cell.X, cell.Y);
                }
            }
        }
    }

    private void OnDestroy()
    {
        CreateButton.onClick.RemoveAllListeners();
    }
}
