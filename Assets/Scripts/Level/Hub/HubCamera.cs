using UnityEngine;

public class Follow_player : MonoBehaviour
{

    public Transform player;

    private Vector3 offset = new Vector3(0, 0, -10);
    public float smoothSpeed = 0.15f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

}