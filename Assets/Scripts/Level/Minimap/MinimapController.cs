using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    private bool _isMapOn;
    private GameObject _minimap;

    [SerializeField]
    private MinimapCamera _minimapCamera;

    void Start(){
        _minimap = transform.Find("Mask").gameObject;
        _minimapCamera = FindObjectOfType<MinimapCamera>();

        if(!_isMapOn)
            EnableMap();
    }

    public void InitCamera()
    {
        _minimapCamera = Instantiate(Resources.Load("Prefabs/Minimap_Camera", typeof(Camera))).GetComponent<MinimapCamera>();
    }

    public void EnableMap()
    {
        _isMapOn = true;
        _minimap.SetActive(true);
    }

    public void DisableMap()
    {
        _isMapOn = false;
        _minimap.SetActive(false);
    }

    public void UpdateMinimap()
    {
        _minimapCamera.UpdateMinimap();
    }
}
