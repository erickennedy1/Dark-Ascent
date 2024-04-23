using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public float speed = 10f; 
    public Vector3 moveDirection = Vector3.down;

    void Start()
    {
        moveDirection = transform.up * -1;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
