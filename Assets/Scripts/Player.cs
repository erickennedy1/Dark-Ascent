using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canMove = true;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        if (objs.Length > 1)
        {
            Debug.Log("Destroy, Player > 1: "+objs.Length);
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        
    }
    public void ResetPlayer()
    {
        transform.position = Vector3.zero;
    }
}
