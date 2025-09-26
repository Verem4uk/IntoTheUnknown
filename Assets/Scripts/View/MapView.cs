using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapView : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private TMP_InputField InputWidth;
    [SerializeField]
    private TMP_InputField InputHeight;
    [SerializeField]
    private Button CreateButton;

    [SerializeField]
    private GameObject TilePrefab;

    [SerializeField]
    private Transform TilesTransform;

    private Map CurrentMap;

    private void Start()
    {
        CreateButton.onClick.AddListener(OnCreateMapClicked);
    }

    private void OnCreateMapClicked()
    {
        int width = Mathf.Max(1, int.Parse(InputWidth.text));
        int height = Mathf.Max(1, int.Parse(InputHeight.text));
                
        foreach (Transform child in TilesTransform)
        {
            Destroy(child.gameObject);
        }
               
        CurrentMap = new Map(width, height);
                
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tileObj = Instantiate(TilePrefab, TilesTransform);
                tileObj.transform.position = new Vector3(x, 0, y);                    
            }
        }
    }
}
