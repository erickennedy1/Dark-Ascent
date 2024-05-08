using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ManaEVIda : MonoBehaviour
{
    public Camera cameraToFollow;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (cameraToFollow == null)
        {
            cameraToFollow = Camera.main;  
        }

        if (cameraToFollow != null)
        {
            RectTransform rect = GetComponent<RectTransform>();
            float height = cameraToFollow.orthographicSize * 2;
            float width = height * Screen.width / Screen.height;
            rect.sizeDelta = new Vector2(width, height);
            rect.position = new Vector3(cameraToFollow.transform.position.x, cameraToFollow.transform.position.y, rect.position.z);
        }
    }
}
