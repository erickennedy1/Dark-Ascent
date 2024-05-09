using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Camera cam;
    public float offset = 10;

    void Start(){
        UpdateMinimap();
    }
    public void UpdateMinimap(){
        Bounds bounds = new Bounds();

        foreach(GameObject room in GameObject.FindGameObjectsWithTag("Room"))
        {
            if(room.GetComponent<Room>().isKnown)
                bounds.Encapsulate(room.GetComponent<Collider2D>().bounds);
        }

        bounds.Expand(offset);

        float vertical = bounds.size.y;
        float horizontal = bounds.size.x * cam.pixelHeight / cam.pixelWidth;

        float size = Mathf.Max(horizontal, vertical) * 0.5f;
        Vector3 center = bounds.center + new Vector3(0,0,-10);

        cam.transform.position = center;
        cam.orthographicSize = size;
    }
}