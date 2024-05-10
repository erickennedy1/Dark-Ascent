using Unity.Collections;
using UnityEngine;

public class Follow_player : MonoBehaviour
{

    public Transform player;

    private Vector3 offset = new Vector3(0, 0, -10);
    public float smoothSpeed = 0.15f;

    [SerializeField]private Bounds _cameraBounds;

    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;

        player = GameObject.FindWithTag("Player").transform;
        _cameraBounds = GameObject.FindWithTag("Room").GetComponent<Collider2D>().bounds;

        float height = _mainCamera.orthographicSize;
        float width = height * _mainCamera.aspect;

        float minX = _cameraBounds.min.x + width;
        float maxX = _cameraBounds.extents.x - width;

        float minY = _cameraBounds.min.y + height;
        float maxY = _cameraBounds.extents.y - height;

        _cameraBounds.SetMinMax(
            new Vector3(minX, minY, 0),
            new Vector3(maxX, maxY, 0)
        );

    }

    void FixedUpdate()
    {
        transform.position = CalculatePosition();
    }

    private Vector3 CalculatePosition()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        return new Vector3(
            Mathf.Clamp(smoothedPosition.x, _cameraBounds.min.x, _cameraBounds.max.x),
            Mathf.Clamp(smoothedPosition.y, _cameraBounds.min.y, _cameraBounds.max.y),
            smoothedPosition.z
        );
    }
}